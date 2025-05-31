using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Media;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;


namespace SpotDifferenceGame
{
    public partial class MainForm : Form
    {
        
        private Bitmap image1, image2;
        private List<Point> differences = new List<Point>();
        private List<Point> foundDifferences = new List<Point>();
        private int totalDifferences = 3;
        private int remainingDifferences;
        private int attemptsLeft;
        private int timeLeft;
        private GameMode currentGameMode;
        private DifficultyLevel currentDifficulty;

       
        private Pen circlePen = new Pen(Color.Red, 3);
      //  private Brush correctBrush = new SolidBrush(Color.FromArgb(100, Color.Green));
       // private Brush wrongBrush = new SolidBrush(Color.FromArgb(100, Color.Red));

        
        private SoundPlayer correctSound = new SoundPlayer(WindowsFormsApp1.Properties.Resources.correct);
        private SoundPlayer wrongSound = new SoundPlayer(WindowsFormsApp1.Properties.Resources.inccorect);

        public MainForm()
        {
            InitializeComponent();
            InitializeGame();
            UpdateDifficultySettings();
        }

        private void InitializeGame()
        {
            differences.Clear();
            foundDifferences.Clear();

            lblFound.Text = "Found: 0";
            lblRemaining.Text = "Remaining: " + totalDifferences;

            pictureBox1.Refresh();
            pictureBox2.Refresh();
        }

        private void btnLoadImages_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "Select Two Images";

            if (openFileDialog.ShowDialog() == DialogResult.OK && openFileDialog.FileNames.Length == 2)
            {
                try
                {
                    image1 = new Bitmap(openFileDialog.FileNames[0]);
                    image2 = new Bitmap(openFileDialog.FileNames[1]);

              
                    pictureBox1.Image = ScaleImage(image1, pictureBox1.Width, pictureBox1.Height);
                    pictureBox2.Image = ScaleImage(image2, pictureBox2.Width, pictureBox2.Height);

             
                    DetectDifferences();

                    btnStartGame.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading images: " + ex.Message);
                }
            }
        }

        private Mat BitmapToMat(Bitmap bmp)
        {
            BitmapData bmpData = bmp.LockBits(
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            try
            {
                Mat mat = new Mat(bmp.Height, bmp.Width, DepthType.Cv8U, 4, bmpData.Scan0, bmpData.Stride);
                Mat result = new Mat();
                CvInvoke.CvtColor(mat, result, ColorConversion.Bgra2Bgr);
                return result;
            }
            finally
            {
                bmp.UnlockBits(bmpData);
            }
        }


        private void DetectDifferences()
        {
            if (pictureBox1.Image == null || pictureBox2.Image == null)
                return;

            try
            {

                using (Mat mat1 = BitmapToMat((Bitmap)pictureBox1.Image))
                using (Mat mat2 = BitmapToMat((Bitmap)pictureBox2.Image))
                using (Image<Bgr, byte> img1 = mat1.ToImage<Bgr, byte>())
                using (Image<Bgr, byte> img2 = mat2.ToImage<Bgr, byte>())

                {
                    if (img1.Size != img2.Size)
                    {
                        MessageBox.Show("Images must be the same size after scaling!");
                        return;
                    }

                    using (var gray1 = img1.Convert<Gray, byte>())
                    using (var gray2 = img2.Convert<Gray, byte>())
                    {
                        
                        using (var diff = gray1.AbsDiff(gray2))
                        {
                           
                            CvInvoke.Imshow("Before Threshold", diff); 

                          
                            CvInvoke.Threshold(diff, diff, 40, 255, ThresholdType.Binary);

                            CvInvoke.Imshow("After Threshold", diff);            
   
                            Mat kernel = CvInvoke.GetStructuringElement(ElementShape.Ellipse, new Size(3, 3), Point.Empty);
                            CvInvoke.MorphologyEx(diff, diff, MorphOp.Close, kernel, Point.Empty, 3, BorderType.Default, new MCvScalar(0));

                            CvInvoke.Imshow("After Morph", diff);

                            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
                            using (Mat hierarchy = new Mat())
                            {
                                CvInvoke.FindContours(diff, contours, hierarchy, RetrType.List, ChainApproxMethod.ChainApproxSimple);

                                List<(int idx, double area, Point center)> contourInfos = new List<(int, double, Point)>();

                                for (int i = 0; i < contours.Size; i++)
                                {
                                    using (VectorOfPoint contour = contours[i])
                                    {
                                        double area = CvInvoke.ContourArea(contour);
                                        if (area < 10) continue; 

                                        Rectangle rect = CvInvoke.BoundingRectangle(contour);
                                        double aspectRatio = (double)rect.Width / rect.Height;
                                        if (aspectRatio > 4 || aspectRatio < 0.25) continue; 

                                        Point center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
                                        contourInfos.Add((i, area, center));
                                    }
                                }

                                contourInfos.Sort((a, b) => b.area.CompareTo(a.area));
                                differences.Clear();
                                int diffsToTake = Math.Min(totalDifferences, contourInfos.Count);
                               
                                for (int i = 0; i < diffsToTake; i++)
                                {
                                    differences.Add(contourInfos[i].center);
                                }

                               
                                if (differences.Count < totalDifferences)
                                {
                                    totalDifferences = differences.Count;
                                    UpdateDifficultySettings();
                                    MessageBox.Show($"Adjusted to {totalDifferences} detectable differences.");
                                }

                                remainingDifferences = totalDifferences;
                                lblRemaining.Text = "Remaining: " + remainingDifferences;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error detecting differences: " + ex.Message);
            }
        }

        private Bitmap ScaleImage(Bitmap original, int maxWidth, int maxHeight)
        {
            double ratioX = (double)maxWidth / original.Width;
            double ratioY = (double)maxHeight / original.Height;
            double ratio = Math.Min(ratioX, ratioY);

            int newWidth = (int)(original.Width * ratio);
            int newHeight = (int)(original.Height * ratio);

            Bitmap newImage = new Bitmap(newWidth, newHeight);

            using (Graphics graphics = Graphics.FromImage(newImage))
            {
                graphics.DrawImage(original, 0, 0, newWidth, newHeight);
            }

            return newImage;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            DrawFeedback(e.Graphics, pictureBox1);
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            DrawFeedback(e.Graphics, pictureBox2);
        }

        private void DrawFeedback(Graphics g, PictureBox pictureBox)
        {
            foreach (Point point in differences)
            {
                if (foundDifferences.Contains(point))
                {
                    g.DrawEllipse(circlePen, point.X - 15, point.Y - 15, 30, 30);
                }
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            HandlePictureBoxClick(e.Location, pictureBox1);
        }

        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
        {
            HandlePictureBoxClick(e.Location, pictureBox2);
        }

        private void HandlePictureBoxClick(Point clickPoint, PictureBox pictureBox)
        {
            if (currentGameMode == GameMode.AttemptLimit && attemptsLeft <= 0)
            {
                MessageBox.Show("No attempts left!");
                return;
            }

            bool isCorrect = false;
            foreach (Point diff in differences)
            {
                double distance = Math.Sqrt(Math.Pow(clickPoint.X - diff.X, 2) + Math.Pow(clickPoint.Y - diff.Y, 2));
                if (distance < 15 && !foundDifferences.Contains(diff))
                {
                    foundDifferences.Add(diff);
                    remainingDifferences--;
                    isCorrect = true;
                    correctSound.Play();
                    break;
                }
            }

            if (!isCorrect)
            {
                wrongSound.Play();
                if (currentGameMode == GameMode.AttemptLimit)
                {
                    attemptsLeft--;
                    lblAttempts.Text = "Attempts: " + attemptsLeft;
                    if (attemptsLeft <= 0) MessageBox.Show("Game Over!");
                }
            }

            UpdateGameDisplay();
            if (remainingDifferences <= 0) EndGameWithVictory();
        }

        private void UpdateGameDisplay()
        {
            lblFound.Text = "Found: " + foundDifferences.Count;
            lblRemaining.Text = "Remaining: " + remainingDifferences;
            pictureBox1.Refresh();
            pictureBox2.Refresh();
        }

        private void EndGameWithVictory()
        {
            timer1.Stop();
            MessageBox.Show("Congratulations! All differences found!");
        }

        private void btnStartGame_Click(object sender, EventArgs e)
        {
            if (differences.Count == 0)
            {
                MessageBox.Show("No differences detected! Try different images.");
                return;
            }

            remainingDifferences = totalDifferences;
            foundDifferences.Clear();

            if (currentGameMode == GameMode.TimeLimit)
            {
                timer1.Interval = 1000;
                timer1.Start();
            }

            UpdateGameDisplay();
        }

        private void UpdateDifficultySettings()
        {
            switch (currentDifficulty)
            {
                case DifficultyLevel.Easy:
                    totalDifferences = 3;
                    timeLeft = 180;
                    attemptsLeft = 15;
                    break;
                case DifficultyLevel.Medium:
                    totalDifferences = 5;
                    timeLeft = 120;
                    attemptsLeft = 10;
                    break;
                case DifficultyLevel.Hard:
                    totalDifferences = 8;
                    timeLeft = 60;
                    attemptsLeft = 5;
                    break;
            }

            lblTime.Text = "Time: " + timeLeft + "s";
            lblAttempts.Text = "Attempts: " + attemptsLeft;
        }

        private void cmbGameMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentGameMode = cmbGameMode.SelectedIndex == 0 ? GameMode.TimeLimit : GameMode.AttemptLimit;
            lblTime.Visible = currentGameMode == GameMode.TimeLimit;
            lblAttempts.Visible = !lblTime.Visible;
            UpdateDifficultySettings();
        }

        private void cmbDifficulty_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentDifficulty = (DifficultyLevel)cmbDifficulty.SelectedIndex;
            UpdateDifficultySettings();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timeLeft--;
            lblTime.Text = "Time: " + timeLeft + "s";
            if (timeLeft <= 0)
            {
                timer1.Stop();
                MessageBox.Show("Time's up!");
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetGame();
        }

        private void ResetGame()
        {
            timer1.Stop();

          
            image1?.Dispose();
            image2?.Dispose();
            image1 = null;
            image2 = null;

           
            pictureBox1.Image = null;
            pictureBox2.Image = null;

           
            differences.Clear();
            foundDifferences.Clear();

     
            lblFound.Text = "Found: 0";
            lblRemaining.Text = "Remaining: 0";
            lblTime.Text = "Time: 0s";
            lblAttempts.Text = "Attempts: 0";

           
            cmbGameMode.SelectedIndex = 0;
            cmbDifficulty.SelectedIndex = 0;
            currentGameMode = GameMode.TimeLimit;
            currentDifficulty = DifficultyLevel.Easy;

            UpdateDifficultySettings();

            btnStartGame.Enabled = false;
        }

        private enum GameMode { TimeLimit, AttemptLimit }
        private enum DifficultyLevel { Easy, Medium, Hard }
    }
}
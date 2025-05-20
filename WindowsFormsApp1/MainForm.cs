using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace SpotDifferenceGame
{
    public partial class MainForm : Form
    {
        // Game variables
        private Bitmap image1, image2;
        private List<Point> differences = new List<Point>();
        private List<Point> foundDifferences = new List<Point>();
        private int totalDifferences = 5;
        private int remainingDifferences;
        private int attemptsLeft;
        private int timeLeft;
        private GameMode currentGameMode;
        private DifficultyLevel currentDifficulty;
        private bool isSetupMode = true;
        private int setupClicksCount = 0;

        // Visual elements
        private Pen circlePen = new Pen(Color.Red, 3);
        private Pen setupPen = new Pen(Color.LimeGreen, 3);
        private Brush correctBrush = new SolidBrush(Color.FromArgb(100, Color.Green));
        private Brush wrongBrush = new SolidBrush(Color.FromArgb(100, Color.Red));

        // Sounds
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
            isSetupMode = true;
            setupClicksCount = 0;
            differences.Clear();
            foundDifferences.Clear();

            lblFound.Text = "Found: 0";
            lblRemaining.Text = "Remaining: " + totalDifferences;
            lblSetupStatus.Text = $"Marked: 0/{totalDifferences} points";

            pictureBox1.Refresh();
            pictureBox2.Refresh();

            btnStartGame.Enabled = false;
            btnStartSetup.Enabled = true;
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

                    btnStartSetup.Enabled = true;
                    btnStartGame.Enabled = false;
                    lblSetupStatus.Text = $"Marked: 0/{totalDifferences} points";
                    isSetupMode = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading images: " + ex.Message);
                }
            }
        }


        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            DrawFeedback(e.Graphics, pictureBox1);
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            DrawFeedback(e.Graphics, pictureBox2);
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
            if (isSetupMode)
            {
                HandleSetupClick(clickPoint, pictureBox);
            }
            else
            {
                CheckDifference(clickPoint, pictureBox);
            }
        }

        private void HandleSetupClick(Point clickPoint, PictureBox pictureBox)
        {
            if (setupClicksCount >= totalDifferences)
            {
                MessageBox.Show("Maximum differences marked!");
                return;
            }

            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                var toRemove = differences.FindLast(p =>
                    Math.Abs(p.X - clickPoint.X) < 20 &&
                    Math.Abs(p.Y - clickPoint.Y) < 20);

                if (toRemove != null)
                {
                    differences.Remove(toRemove);
                    setupClicksCount--;
                }
            }
            else
            {
                differences.Add(clickPoint);
                setupClicksCount++;
            }

            lblSetupStatus.Text = $"Marked: {setupClicksCount}/{totalDifferences} points";
            btnStartGame.Enabled = (setupClicksCount == totalDifferences);
            pictureBox.Refresh();
        }

        private void CheckDifference(Point clickPoint, PictureBox pictureBox)
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
                if (distance < 20 && !foundDifferences.Contains(diff))
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

        private void DrawFeedback(Graphics g, PictureBox pictureBox)
        {
            foreach (Point point in differences)
            {
                if (isSetupMode)
                {
                    g.DrawEllipse(setupPen, point.X - 15, point.Y - 15, 30, 30);
                }
                else if (foundDifferences.Contains(point))
                {
                    g.DrawEllipse(circlePen, point.X - 15, point.Y - 15, 30, 30);
                }
            }
        }

        private void btnStartSetup_Click(object sender, EventArgs e)
        {
            isSetupMode = true;
            differences.Clear();
            setupClicksCount = 0;
            lblSetupStatus.Text = $"Marked: 0/{totalDifferences} points";
            btnStartGame.Enabled = false;
        }

        private void btnStartGame_Click(object sender, EventArgs e)
        {
            if (differences.Count != totalDifferences)
            {
                MessageBox.Show("Please mark all required differences first!");
                return;
            }

            isSetupMode = false;
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
            lblSetupStatus.Text = $"Marked: 0/{totalDifferences} points";
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

            // Dispose images
            image1?.Dispose();
            image2?.Dispose();
            image1 = null;
            image2 = null;

            // Clear picture boxes
            pictureBox1.Image = null;
            pictureBox2.Image = null;

            // Reset game state
            differences.Clear();
            foundDifferences.Clear();
            isSetupMode = true;
            setupClicksCount = 0;

            // Reset UI
            lblFound.Text = "Found: 0";
            lblRemaining.Text = "Remaining: 0";
            lblTime.Text = "Time: 0s";
            lblAttempts.Text = "Attempts: 0";

            // Reset comboboxes
            cmbGameMode.SelectedIndex = 0;
            cmbDifficulty.SelectedIndex = 0;
            currentGameMode = GameMode.TimeLimit;
            currentDifficulty = DifficultyLevel.Easy;

            UpdateDifficultySettings();

            // Update button states
            btnStartGame.Enabled = false;
            btnStartSetup.Enabled = false;
            btnLoadImages.Enabled = true;

            pictureBox1.Refresh();
            pictureBox2.Refresh();
        }

        private enum GameMode { TimeLimit, AttemptLimit }
        private enum DifficultyLevel { Easy, Medium, Hard }

        // Designer-generated code should include:
        // - Two PictureBox controls (pictureBox1 and pictureBox2)
        // - Buttons: btnLoadImages, btnStartSetup, btnStartGame
        // - Labels: lblFound, lblRemaining, lblTime, lblAttempts, lblSetupStatus
        // - ComboBoxes: cmbGameMode, cmbDifficulty
        // - Timer: timer1
    }
}
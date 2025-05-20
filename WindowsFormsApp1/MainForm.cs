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
        private int totalDifferences = 5; // Default number of differences
        private int remainingDifferences;
        private int attemptsLeft;
        private int timeLeft;
        private GameMode currentGameMode;
        private DifficultyLevel currentDifficulty;

        // Visual elements
        private Pen circlePen = new Pen(Color.Red, 3);
        private Brush correctBrush = new SolidBrush(Color.FromArgb(100, Color.Green));
        private Brush wrongBrush = new SolidBrush(Color.FromArgb(100, Color.Red));

        // Sounds
        private SoundPlayer correctSound = new SoundPlayer(WindowsFormsApp1.Properties.Resources.correct);
        private SoundPlayer wrongSound = new SoundPlayer(WindowsFormsApp1.Properties.Resources.inccorect);

        public MainForm()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            // Set default values
            currentGameMode = GameMode.TimeLimit;
            currentDifficulty = DifficultyLevel.Medium;
            UpdateDifficultySettings();

            // Initialize UI
            lblFound.Text = "Found: 0";
            lblRemaining.Text = "Remaining: " + totalDifferences;
            lblTime.Visible = currentGameMode == GameMode.TimeLimit;
            lblAttempts.Visible = currentGameMode == GameMode.AttemptLimit;

            // Reset game state
            foundDifferences.Clear();
            differences.Clear();
            pictureBox1.Refresh();
            pictureBox2.Refresh();
        }



       

        private void btnLoadImages_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "Select Two Images";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (openFileDialog.FileNames.Length == 2)
                {
                    try
                    {
                        image1 = new Bitmap(openFileDialog.FileNames[0]);
                        image2 = new Bitmap(openFileDialog.FileNames[1]);

                        // Scale images to fit picture boxes while maintaining aspect ratio
                        pictureBox1.Image = ScaleImage(image1, pictureBox1.Width, pictureBox1.Height);
                        pictureBox2.Image = ScaleImage(image2, pictureBox2.Width, pictureBox2.Height);

                        // Generate random differences (for demo purposes)
                        GenerateDifferences();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading images: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Please select exactly two images.");
                }
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

        private void GenerateDifferences()
        {
            differences.Clear();
            Random rand = new Random();

            for (int i = 0; i < totalDifferences; i++)
            {
                int x = rand.Next(20, pictureBox1.Width - 20);
                int y = rand.Next(20, pictureBox1.Height - 20);
                differences.Add(new Point(x, y));
            }

            remainingDifferences = totalDifferences;
            lblRemaining.Text = "Remaining: " + remainingDifferences;
            lblFound.Text = "Found: 0";
            foundDifferences.Clear();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (image1 == null || image2 == null) return;

            CheckDifference(e.Location, pictureBox1);
        }

        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
        {
            if (image1 == null || image2 == null) return;

            CheckDifference(e.Location, pictureBox2);
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
                // Calculate distance between click and difference point
                double distance = Math.Sqrt(Math.Pow(clickPoint.X - diff.X, 2) + Math.Pow(clickPoint.Y - diff.Y, 2));

                if (distance < 20) // Within 20 pixels is considered correct
                {
                    if (!foundDifferences.Contains(diff))
                    {
                        foundDifferences.Add(diff);
                        remainingDifferences--;
                        isCorrect = true;

                        // Play correct sound
                        correctSound.Play();

                        break;
                    }
                }
            }

            if (!isCorrect)
            {
                // Play wrong sound
                wrongSound.Play();

                if (currentGameMode == GameMode.AttemptLimit)
                {
                    attemptsLeft--;
                    lblAttempts.Text = "Attempts: " + attemptsLeft;

                    if (attemptsLeft <= 0)
                    {
                        MessageBox.Show("Game Over! You've used all your attempts.");
                    }
                }
            }

            // Update UI
            lblFound.Text = "Found: " + foundDifferences.Count;
            lblRemaining.Text = "Remaining: " + remainingDifferences;

            // Redraw to show feedback
            pictureBox1.Refresh();
            pictureBox2.Refresh();

            // Check if game is won
            if (remainingDifferences <= 0)
            {
                timer1.Stop();
                MessageBox.Show("Congratulations! You found all the differences!");
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

        private void DrawFeedback(Graphics g, PictureBox pictureBox)
        {
            foreach (Point found in foundDifferences)
            {
                // Draw a circle around found differences
                g.DrawEllipse(circlePen, found.X - 15, found.Y - 15, 30, 30);
            }
        }

        private void cmbGameMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbGameMode.SelectedIndex == 0)
            {
                currentGameMode = GameMode.TimeLimit;
                lblTime.Visible = true;
                lblAttempts.Visible = false;
            }
            else
            {
                currentGameMode = GameMode.AttemptLimit;
                lblTime.Visible = false;
                lblAttempts.Visible = true;
            }

            UpdateDifficultySettings();
        }

        private void cmbDifficulty_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbDifficulty.SelectedIndex)
            {
                case 0: currentDifficulty = DifficultyLevel.Easy; break;
                case 1: currentDifficulty = DifficultyLevel.Medium; break;
                case 2: currentDifficulty = DifficultyLevel.Hard; break;
            }

            UpdateDifficultySettings();
        }

        private void btnStartGame_Click(object sender, EventArgs e)
        {
            if (image1 == null || image2 == null)
            {
                MessageBox.Show("Please load images first!");
                return;
            }

            InitializeGame();
            GenerateDifferences();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timeLeft--;
            lblTime.Text = "Time: " + timeLeft + "s";

            if (timeLeft <= 0)
            {
                timer1.Stop();
                MessageBox.Show("Time's up! Game over.");
            }
        }

        private void UpdateDifficultySettings()
        {
            switch (currentDifficulty)
            {
                case DifficultyLevel.Easy:
                    totalDifferences = 3;
                    timeLeft = 180; // 3 minutes
                    attemptsLeft = 15;
                    break;
                case DifficultyLevel.Medium:
                    totalDifferences = 5;
                    timeLeft = 120; // 2 minutes
                    attemptsLeft = 10;
                    break;
                case DifficultyLevel.Hard:
                    totalDifferences = 8;
                    timeLeft = 60; // 1 minute
                    attemptsLeft = 5;
                    break;
            }

            remainingDifferences = totalDifferences;
            lblRemaining.Text = "Remaining: " + remainingDifferences;

            if (currentGameMode == GameMode.TimeLimit)
            {
                lblTime.Text = "Time: " + timeLeft + "s";
                timer1.Start();
            }
            else
            {
                lblAttempts.Text = "Attempts: " + attemptsLeft;
            }
        }

        // Enum for game modes
        private enum GameMode
        {
            TimeLimit,
            AttemptLimit
        }

        // Enum for difficulty levels
        private enum DifficultyLevel
        {
            Easy,
            Medium,
            Hard
        }
    }
}
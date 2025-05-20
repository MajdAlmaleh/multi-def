namespace SpotDifferenceGame
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnLoadImages = new System.Windows.Forms.Button();
            this.lblFound = new System.Windows.Forms.Label();
            this.lblRemaining = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblAttempts = new System.Windows.Forms.Label();
            this.cmbGameMode = new System.Windows.Forms.ComboBox();
            this.cmbDifficulty = new System.Windows.Forms.ComboBox();
            this.btnStartGame = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();


            // Adjust lblTime and lblAttempts positions to prevent overlapping
            // Original lblTime position
            this.lblTime.Location = new System.Drawing.Point(340, 440);

            // Modified lblAttempts position
            this.lblAttempts.Location = new System.Drawing.Point(420, 440);

            // pictureBox1
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(20, 20);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(400, 400);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);

            // pictureBox2
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Location = new System.Drawing.Point(440, 20);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(400, 400);
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox2_Paint);
            this.pictureBox2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox2_MouseClick);

            // btnLoadImages
            this.btnLoadImages.Location = new System.Drawing.Point(20, 440);
            this.btnLoadImages.Name = "btnLoadImages";
            this.btnLoadImages.Size = new System.Drawing.Size(120, 30);
            this.btnLoadImages.TabIndex = 2;
            this.btnLoadImages.Text = "Load Images";
            this.btnLoadImages.UseVisualStyleBackColor = true;
            this.btnLoadImages.Click += new System.EventHandler(this.btnLoadImages_Click);

            // lblFound
            this.lblFound.AutoSize = true;
            this.lblFound.Location = new System.Drawing.Point(160, 440);
            this.lblFound.Name = "lblFound";
            this.lblFound.Size = new System.Drawing.Size(50, 13);
            this.lblFound.TabIndex = 3;
            this.lblFound.Text = "Found: 0";

            // lblRemaining
            this.lblRemaining.AutoSize = true;
            this.lblRemaining.Location = new System.Drawing.Point(240, 440);
            this.lblRemaining.Name = "lblRemaining";
            this.lblRemaining.Size = new System.Drawing.Size(80, 13);
            this.lblRemaining.TabIndex = 4;
            this.lblRemaining.Text = "Remaining: 0";

            // lblTime
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(340, 440);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(50, 13);
            this.lblTime.TabIndex = 5;
            this.lblTime.Text = "Time: 0s";

            // lblAttempts
            this.lblAttempts.AutoSize = true;
            this.lblAttempts.Location = new System.Drawing.Point(340, 440);
            this.lblAttempts.Name = "lblAttempts";
            this.lblAttempts.Size = new System.Drawing.Size(70, 13);
            this.lblAttempts.TabIndex = 6;
            this.lblAttempts.Text = "Attempts: 0";
            this.lblAttempts.Visible = false;

            // cmbGameMode
            this.cmbGameMode.FormattingEnabled = true;
            this.cmbGameMode.Items.AddRange(new object[] {
            "Time Limit",
            "Attempt Limit"});
            this.cmbGameMode.Location = new System.Drawing.Point(440, 440);
            this.cmbGameMode.Name = "cmbGameMode";
            this.cmbGameMode.Size = new System.Drawing.Size(120, 21);
            this.cmbGameMode.TabIndex = 7;
            this.cmbGameMode.Text = "Select Game Mode";
            this.cmbGameMode.SelectedIndexChanged += new System.EventHandler(this.cmbGameMode_SelectedIndexChanged);

            // cmbDifficulty
            this.cmbDifficulty.FormattingEnabled = true;
            this.cmbDifficulty.Items.AddRange(new object[] {
            "Easy",
            "Medium",
            "Hard"});
            this.cmbDifficulty.Location = new System.Drawing.Point(580, 440);
            this.cmbDifficulty.Name = "cmbDifficulty";
            this.cmbDifficulty.Size = new System.Drawing.Size(120, 21);
            this.cmbDifficulty.TabIndex = 8;
            this.cmbDifficulty.Text = "Select Difficulty";
            this.cmbDifficulty.SelectedIndexChanged += new System.EventHandler(this.cmbDifficulty_SelectedIndexChanged);

            // btnStartGame
            this.btnStartGame.Location = new System.Drawing.Point(720, 440);
            this.btnStartGame.Name = "btnStartGame";
            this.btnStartGame.Size = new System.Drawing.Size(120, 30);
            this.btnStartGame.TabIndex = 9;
            this.btnStartGame.Text = "Start Game";
            this.btnStartGame.UseVisualStyleBackColor = true;
            this.btnStartGame.Click += new System.EventHandler(this.btnStartGame_Click);

            // timer1
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);

            // MainForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 491);
            this.Controls.Add(this.btnStartGame);
            this.Controls.Add(this.cmbDifficulty);
            this.Controls.Add(this.cmbGameMode);
            this.Controls.Add(this.lblAttempts);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblRemaining);
            this.Controls.Add(this.lblFound);
            this.Controls.Add(this.btnLoadImages);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Name = "MainForm";
            this.Text = "Spot the Difference Game";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnLoadImages;
        private System.Windows.Forms.Label lblFound;
        private System.Windows.Forms.Label lblRemaining;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblAttempts;
        private System.Windows.Forms.ComboBox cmbGameMode;
        private System.Windows.Forms.ComboBox cmbDifficulty;
        private System.Windows.Forms.Button btnStartGame;
        private System.Windows.Forms.Timer timer1;
    }
}
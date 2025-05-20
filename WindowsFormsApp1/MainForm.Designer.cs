using System.Windows.Forms;
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
            this.components = new System.ComponentModel.Container();
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
            this.btnReset = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnStartSetup = new System.Windows.Forms.Button();
            this.lblSetupStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();

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

            // btnReset
            this.btnReset.Location = new System.Drawing.Point(20, 480);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(120, 30);
            this.btnReset.TabIndex = 12;
            this.btnReset.Text = "Reset Game";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);

            // lblFound
            this.lblFound.AutoSize = true;
            this.lblFound.Location = new System.Drawing.Point(300, 445);
            this.lblFound.Name = "lblFound";
            this.lblFound.Size = new System.Drawing.Size(50, 13);
            this.lblFound.TabIndex = 3;
            this.lblFound.Text = "Found: 0";

            // lblRemaining
            this.lblRemaining.AutoSize = true;
            this.lblRemaining.Location = new System.Drawing.Point(380, 445);
            this.lblRemaining.Name = "lblRemaining";
            this.lblRemaining.Size = new System.Drawing.Size(80, 13);
            this.lblRemaining.TabIndex = 4;
            this.lblRemaining.Text = "Remaining: 0";

            // lblTime
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(480, 445);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(50, 13);
            this.lblTime.TabIndex = 5;
            this.lblTime.Text = "Time: 0s";

            // lblAttempts
            this.lblAttempts.AutoSize = true;
            this.lblAttempts.Location = new System.Drawing.Point(480, 445);
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
            this.cmbGameMode.Location = new System.Drawing.Point(560, 440);
            this.cmbGameMode.Name = "cmbGameMode";
            this.cmbGameMode.Size = new System.Drawing.Size(120, 21);
            this.cmbGameMode.TabIndex = 7;
            this.cmbGameMode.SelectedIndexChanged += new System.EventHandler(this.cmbGameMode_SelectedIndexChanged);

            // cmbDifficulty
            this.cmbDifficulty.FormattingEnabled = true;
            this.cmbDifficulty.Items.AddRange(new object[] {
            "Easy",
            "Medium",
            "Hard"});
            this.cmbDifficulty.Location = new System.Drawing.Point(690, 440);
            this.cmbDifficulty.Name = "cmbDifficulty";
            this.cmbDifficulty.Size = new System.Drawing.Size(120, 21);
            this.cmbDifficulty.TabIndex = 8;
            this.cmbDifficulty.SelectedIndexChanged += new System.EventHandler(this.cmbDifficulty_SelectedIndexChanged);

            // btnStartGame
            this.btnStartGame.Enabled = false;
            this.btnStartGame.Location = new System.Drawing.Point(820, 440);
            this.btnStartGame.Name = "btnStartGame";
            this.btnStartGame.Size = new System.Drawing.Size(120, 30);
            this.btnStartGame.TabIndex = 9;
            this.btnStartGame.Text = "Start Game";
            this.btnStartGame.UseVisualStyleBackColor = true;
            this.btnStartGame.Click += new System.EventHandler(this.btnStartGame_Click);

            // timer1
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);

            // btnStartSetup
            this.btnStartSetup.Enabled = false;
            this.btnStartSetup.Location = new System.Drawing.Point(150, 440);
            this.btnStartSetup.Name = "btnStartSetup";
            this.btnStartSetup.Size = new System.Drawing.Size(120, 30);
            this.btnStartSetup.TabIndex = 10;
            this.btnStartSetup.Text = "Setup Mode";
            this.btnStartSetup.UseVisualStyleBackColor = true;
            this.btnStartSetup.Click += new System.EventHandler(this.btnStartSetup_Click);

            // lblSetupStatus
            this.lblSetupStatus.AutoSize = true;
            this.lblSetupStatus.Location = new System.Drawing.Point(280, 445);
            this.lblSetupStatus.Name = "lblSetupStatus";
            this.lblSetupStatus.Size = new System.Drawing.Size(100, 13);
            this.lblSetupStatus.TabIndex = 11;
            this.lblSetupStatus.Text = "Marked: 0/0 points";

            // MainForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 530);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.lblSetupStatus);
            this.Controls.Add(this.btnStartSetup);
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
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnStartSetup;
        private System.Windows.Forms.Label lblSetupStatus;
    }
}
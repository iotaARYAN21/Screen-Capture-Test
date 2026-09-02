namespace ScreenCaptureGUI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblNumber = new Label();
            lblInterval = new Label();
            lblFolder = new Label();
            txtNumber = new TextBox();
            txtInterval = new TextBox();
            txtFolder = new TextBox();
            btnBrowse = new Button();
            btnStart = new Button();
            lblStatus = new Label();
            rbCount = new RadioButton();
            rbDuration = new RadioButton();
            txtDuration = new TextBox();
            btnStop = new Button();
            SuspendLayout();
            // 
            // lblNumber
            // 
            lblNumber.AutoSize = true;
            lblNumber.Location = new Point(599, 68);
            lblNumber.Name = "lblNumber";
            lblNumber.Size = new Size(66, 20);
            lblNumber.TabIndex = 0;
            lblNumber.Text = "No. of ss";
            lblNumber.Click += label1_Click;
            // 
            // lblInterval
            // 
            lblInterval.AutoSize = true;
            lblInterval.Location = new Point(599, 187);
            lblInterval.Name = "lblInterval";
            lblInterval.Size = new Size(93, 20);
            lblInterval.TabIndex = 1;
            lblInterval.Text = "Interval (sec)";
            // 
            // lblFolder
            // 
            lblFolder.AutoSize = true;
            lblFolder.Location = new Point(599, 324);
            lblFolder.Name = "lblFolder";
            lblFolder.Size = new Size(98, 20);
            lblFolder.TabIndex = 2;
            lblFolder.Text = "Save location";
            // 
            // txtNumber
            // 
            txtNumber.Location = new Point(351, 82);
            txtNumber.Name = "txtNumber";
            txtNumber.Size = new Size(125, 27);
            txtNumber.TabIndex = 3;
            txtNumber.Text = "10";
            // 
            // txtInterval
            // 
            txtInterval.Location = new Point(351, 187);
            txtInterval.Name = "txtInterval";
            txtInterval.Size = new Size(125, 27);
            txtInterval.TabIndex = 4;
            txtInterval.Text = "2";
            // 
            // txtFolder
            // 
            txtFolder.Location = new Point(351, 317);
            txtFolder.Name = "txtFolder";
            txtFolder.Size = new Size(125, 27);
            txtFolder.TabIndex = 5;
            txtFolder.Text = "D:\\Screenshot";
            // 
            // btnBrowse
            // 
            btnBrowse.Location = new Point(109, 82);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(94, 29);
            btnBrowse.TabIndex = 6;
            btnBrowse.Text = "Browse";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += btnBrowse_Click;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(109, 187);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(94, 29);
            btnStart.TabIndex = 7;
            btnStart.Text = "Start";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += button2_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(621, 394);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(93, 20);
            lblStatus.TabIndex = 8;
            lblStatus.Text = "Status:Ready";
            // 
            // rbCount
            // 
            rbCount.AutoSize = true;
            rbCount.Checked = true;
            rbCount.Location = new Point(519, 263);
            rbCount.Name = "rbCount";
            rbCount.Size = new Size(80, 24);
            rbCount.TabIndex = 9;
            rbCount.TabStop = true;
            rbCount.Text = "no.of ss";
            rbCount.UseVisualStyleBackColor = true;
            // 
            // rbDuration
            // 
            rbDuration.AutoSize = true;
            rbDuration.Location = new Point(518, 306);
            rbDuration.Name = "rbDuration";
            rbDuration.Size = new Size(142, 24);
            rbDuration.TabIndex = 10;
            rbDuration.Text = "Capture duration";
            rbDuration.UseVisualStyleBackColor = true;
            rbDuration.CheckedChanged += radioButton2_CheckedChanged;
            // 
            // txtDuration
            // 
            txtDuration.Location = new Point(120, 317);
            txtDuration.Name = "txtDuration";
            txtDuration.Size = new Size(125, 27);
            txtDuration.TabIndex = 11;
            txtDuration.Text = "30";
            // 
            // btnStop
            // 
            btnStop.Location = new Point(297, 376);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(94, 29);
            btnStop.TabIndex = 12;
            btnStop.Text = "stop";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnStop);
            Controls.Add(txtDuration);
            Controls.Add(rbDuration);
            Controls.Add(rbCount);
            Controls.Add(lblStatus);
            Controls.Add(btnStart);
            Controls.Add(btnBrowse);
            Controls.Add(txtFolder);
            Controls.Add(txtInterval);
            Controls.Add(txtNumber);
            Controls.Add(lblFolder);
            Controls.Add(lblInterval);
            Controls.Add(lblNumber);
            KeyPreview = true;
            Name = "Form1";
            Text = "ScreenCapture";
            Load += Form1_Load;
            KeyDown += Form1_KeyDown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblNumber;
        private Label lblInterval;
        private Label lblFolder;
        private TextBox txtNumber;
        private TextBox txtInterval;
        private TextBox txtFolder;
        private Button btnBrowse;
        private Button btnStart;
        private Label lblStatus;
        private RadioButton rbCount;
        private RadioButton rbDuration;
        private TextBox txtDuration;
        private Button btnStop;
    }
}

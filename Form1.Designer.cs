namespace Gestion_des_Employés
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            labelconn = new Label();
            label2 = new Label();
            Pconnexioninscription = new Panel();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // labelconn
            // 
            labelconn.AutoSize = true;
            labelconn.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            labelconn.Location = new Point(21, 7);
            labelconn.Name = "labelconn";
            labelconn.Size = new Size(125, 25);
            labelconn.TabIndex = 0;
            labelconn.Text = "CONNEXION";
            labelconn.Click += labelconn_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            label2.Location = new Point(151, 9);
            label2.Name = "label2";
            label2.Size = new Size(132, 25);
            label2.TabIndex = 1;
            label2.Text = "INSCRIPTION";
            label2.Click += label2_Click;
            // 
            // Pconnexioninscription
            // 
            Pconnexioninscription.Location = new Point(26, 37);
            Pconnexioninscription.Name = "Pconnexioninscription";
            Pconnexioninscription.Size = new Size(275, 579);
            Pconnexioninscription.TabIndex = 3;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(302, 13);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(18, 20);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // Form1
            // 
            AllowDrop = true;
            AutoScaleMode = AutoScaleMode.None;
            AutoScroll = true;
            AutoSize = true;
            AutoValidate = AutoValidate.Disable;
            ClientSize = new Size(334, 628);
            Controls.Add(pictureBox1);
            Controls.Add(Pconnexioninscription);
            Controls.Add(label2);
            Controls.Add(labelconn);
            Cursor = Cursors.Hand;
            FormBorderStyle = FormBorderStyle.None;
            FormScreenCaptureMode = ScreenCaptureMode.HideContent;
            HelpButton = true;
            Icon = (Icon)resources.GetObject("$this.Icon");
            ImeMode = ImeMode.On;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "connexion";
            WindowState = FormWindowState.Minimized;
            Load += Form1_Load;
            Shown += Form1_Shown;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelconn;
        private Label label2;
        private Panel Pconnexioninscription;
        private PictureBox pictureBox1;
    }
}

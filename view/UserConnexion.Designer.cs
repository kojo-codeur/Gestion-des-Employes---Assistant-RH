namespace Gestion_des_Employés.view
{
    partial class UserConnexion
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserConnexion));
            butlogin = new Button();
            TBPassword = new TextBox();
            TBEmail = new TextBox();
            pictureboxuserlogin = new PictureBox();
            checkBox1 = new CheckBox();
            labelOublier = new Label();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureboxuserlogin).BeginInit();
            SuspendLayout();
            // 
            // butlogin
            // 
            butlogin.BackColor = Color.Navy;
            butlogin.Font = new Font("Elephant", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            butlogin.ForeColor = SystemColors.ButtonHighlight;
            butlogin.Location = new Point(11, 379);
            butlogin.Name = "butlogin";
            butlogin.Size = new Size(265, 47);
            butlogin.TabIndex = 9;
            butlogin.Text = "CONNECTER";
            butlogin.UseVisualStyleBackColor = false;
            butlogin.Click += butlogin_Click;
            // 
            // TBPassword
            // 
            TBPassword.Location = new Point(11, 277);
            TBPassword.Multiline = true;
            TBPassword.Name = "TBPassword";
            TBPassword.PasswordChar = '*';
            TBPassword.Size = new Size(265, 34);
            TBPassword.TabIndex = 8;
            // 
            // TBEmail
            // 
            TBEmail.Location = new Point(11, 201);
            TBEmail.Multiline = true;
            TBEmail.Name = "TBEmail";
            TBEmail.Size = new Size(265, 37);
            TBEmail.TabIndex = 7;
            // 
            // pictureboxuserlogin
            // 
            pictureboxuserlogin.Image = (Image)resources.GetObject("pictureboxuserlogin.Image");
            pictureboxuserlogin.Location = new Point(75, 13);
            pictureboxuserlogin.Name = "pictureboxuserlogin";
            pictureboxuserlogin.Size = new Size(129, 126);
            pictureboxuserlogin.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureboxuserlogin.TabIndex = 6;
            pictureboxuserlogin.TabStop = false;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(11, 334);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(131, 19);
            checkBox1.TabIndex = 10;
            checkBox1.Text = "se souvenire de moi";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // labelOublier
            // 
            labelOublier.AutoSize = true;
            labelOublier.Location = new Point(159, 334);
            labelOublier.Name = "labelOublier";
            labelOublier.Size = new Size(117, 15);
            labelOublier.TabIndex = 11;
            labelOublier.Text = "mot de passe oublier";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(11, 183);
            label1.Name = "label1";
            label1.Size = new Size(66, 15);
            label1.TabIndex = 12;
            label1.Text = "Votre Email";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(11, 259);
            label2.Name = "label2";
            label2.Size = new Size(77, 15);
            label2.TabIndex = 13;
            label2.Text = "Mot de passe";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(36, 441);
            label3.Name = "label3";
            label3.Size = new Size(202, 15);
            label3.TabIndex = 14;
            label3.Text = "connecter vous pour vous enregistre ";
            // 
            // UserConnexion
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(labelOublier);
            Controls.Add(checkBox1);
            Controls.Add(butlogin);
            Controls.Add(TBPassword);
            Controls.Add(TBEmail);
            Controls.Add(pictureboxuserlogin);
            Name = "UserConnexion";
            Size = new Size(291, 544);
            Load += UserConnexion_Load;
            ((System.ComponentModel.ISupportInitialize)pictureboxuserlogin).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button butlogin;
        private TextBox TBPassword;
        private TextBox TBEmail;
        private PictureBox pictureboxuserlogin;
        private CheckBox checkBox1;
        private Label labelOublier;
        private Label label1;
        private Label label2;
        private Label label3;
    }
}

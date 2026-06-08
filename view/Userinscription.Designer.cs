namespace Gestion_des_Employés.view
{
    partial class Userinscription
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Userinscription));
            pictureboxuser = new PictureBox();
            TBName = new TextBox();
            TBEmail = new TextBox();
            TBPassword = new TextBox();
            TBConfirmationPass = new TextBox();
            butInscription = new Button();
            label2 = new Label();
            label1 = new Label();
            label4 = new Label();
            label5 = new Label();
            checkTerme = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)pictureboxuser).BeginInit();
            SuspendLayout();
            // 
            // pictureboxuser
            // 
            pictureboxuser.Image = (Image)resources.GetObject("pictureboxuser.Image");
            pictureboxuser.Location = new Point(83, 22);
            pictureboxuser.Name = "pictureboxuser";
            pictureboxuser.Size = new Size(122, 122);
            pictureboxuser.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureboxuser.TabIndex = 0;
            pictureboxuser.TabStop = false;
            pictureboxuser.Click += pictureboxuser_Click;
            // 
            // TBName
            // 
            TBName.Location = new Point(17, 190);
            TBName.Multiline = true;
            TBName.Name = "TBName";
            TBName.Size = new Size(253, 32);
            TBName.TabIndex = 1;
            // 
            // TBEmail
            // 
            TBEmail.Location = new Point(17, 251);
            TBEmail.Multiline = true;
            TBEmail.Name = "TBEmail";
            TBEmail.Size = new Size(253, 35);
            TBEmail.TabIndex = 2;
            // 
            // TBPassword
            // 
            TBPassword.Location = new Point(17, 307);
            TBPassword.Multiline = true;
            TBPassword.Name = "TBPassword";
            TBPassword.Size = new Size(253, 34);
            TBPassword.TabIndex = 3;
            // 
            // TBConfirmationPass
            // 
            TBConfirmationPass.Location = new Point(17, 360);
            TBConfirmationPass.Multiline = true;
            TBConfirmationPass.Name = "TBConfirmationPass";
            TBConfirmationPass.Size = new Size(253, 37);
            TBConfirmationPass.TabIndex = 4;
            // 
            // butInscription
            // 
            butInscription.BackColor = Color.Navy;
            butInscription.Font = new Font("Elephant", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            butInscription.ForeColor = SystemColors.ButtonHighlight;
            butInscription.Location = new Point(17, 416);
            butInscription.Name = "butInscription";
            butInscription.Size = new Size(253, 47);
            butInscription.TabIndex = 5;
            butInscription.Text = "INSCRIPTION";
            butInscription.UseVisualStyleBackColor = false;
            butInscription.Click += butInscription_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(17, 233);
            label2.Name = "label2";
            label2.Size = new Size(96, 15);
            label2.TabIndex = 16;
            label2.Text = "Entre votre Email";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(17, 176);
            label1.Name = "label1";
            label1.Size = new Size(113, 15);
            label1.TabIndex = 15;
            label1.Text = "Votre Nom Complet";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(17, 289);
            label4.Name = "label4";
            label4.Size = new Size(107, 15);
            label4.TabIndex = 18;
            label4.Text = "Entre Mot de passe";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(17, 344);
            label5.Name = "label5";
            label5.Size = new Size(160, 15);
            label5.TabIndex = 19;
            label5.Text = "Confirme votre Mot de passe";
            // 
            // checkTerme
            // 
            checkTerme.AutoSize = true;
            checkTerme.Location = new Point(17, 491);
            checkTerme.Name = "checkTerme";
            checkTerme.Size = new Size(255, 19);
            checkTerme.TabIndex = 20;
            checkTerme.Text = "j'accepter le condition et terme d'utilisation";
            checkTerme.UseVisualStyleBackColor = true;
            // 
            // Userinscription
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(checkTerme);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(butInscription);
            Controls.Add(TBConfirmationPass);
            Controls.Add(TBPassword);
            Controls.Add(TBEmail);
            Controls.Add(TBName);
            Controls.Add(pictureboxuser);
            Name = "Userinscription";
            Size = new Size(291, 544);
            Load += Userinscription_Load;
            ((System.ComponentModel.ISupportInitialize)pictureboxuser).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureboxuser;
        private TextBox TBName;
        private TextBox TBEmail;
        private TextBox TBPassword;
        private TextBox TBConfirmationPass;
        private Button butInscription;
        private Label label2;
        private Label label1;
        private Label label4;
        private Label label5;
        private CheckBox checkTerme;
    }
}

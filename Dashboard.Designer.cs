namespace Gestion_des_Employés
{
    partial class Dashboard
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Dashboard));
            paneldashboard = new Panel();
            titreApp = new Label();
            BTNDeconnexion = new Button();
            BTNpresence = new Button();
            BTNEmployer = new Button();
            BTNUtils = new Button();
            AdminName = new Label();
            LabelRole = new Label();
            pictureBoxProfile = new PictureBox();
            ButnDashboard = new Button();
            panelNT = new Panel();
            labelCountNotificationIa = new Label();
            BTNAssistant = new PictureBox();
            labeltitre = new Label();
            Labelwelcom = new Label();
            countNotification = new Label();
            PictureNNotifica = new PictureBox();
            PictureNOTUserconnecter = new PictureBox();
            PanelMain = new Panel();
            panelfooter = new Panel();
            linkLabelfooter = new LinkLabel();
            paneldashboard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxProfile).BeginInit();
            panelNT.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)BTNAssistant).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PictureNNotifica).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PictureNOTUserconnecter).BeginInit();
            PanelMain.SuspendLayout();
            panelfooter.SuspendLayout();
            SuspendLayout();
            // 
            // paneldashboard
            // 
            paneldashboard.Controls.Add(titreApp);
            paneldashboard.Controls.Add(BTNDeconnexion);
            paneldashboard.Controls.Add(BTNpresence);
            paneldashboard.Controls.Add(BTNEmployer);
            paneldashboard.Controls.Add(BTNUtils);
            paneldashboard.Controls.Add(AdminName);
            paneldashboard.Controls.Add(LabelRole);
            paneldashboard.Controls.Add(pictureBoxProfile);
            paneldashboard.Controls.Add(ButnDashboard);
            paneldashboard.Location = new Point(5, 6);
            paneldashboard.Name = "paneldashboard";
            paneldashboard.Size = new Size(205, 866);
            paneldashboard.TabIndex = 0;
            // 
            // titreApp
            // 
            titreApp.AutoSize = true;
            titreApp.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            titreApp.ForeColor = Color.Navy;
            titreApp.Location = new Point(9, 9);
            titreApp.Name = "titreApp";
            titreApp.Size = new Size(195, 30);
            titreApp.TabIndex = 10;
            titreApp.Text = "Gestion Employers";
            // 
            // BTNDeconnexion
            // 
            BTNDeconnexion.BackColor = Color.Red;
            BTNDeconnexion.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BTNDeconnexion.Image = Properties.Resources.logout_20dp_E3E3E3_FILL0_wght400_GRAD0_opsz20;
            BTNDeconnexion.ImageAlign = ContentAlignment.MiddleLeft;
            BTNDeconnexion.Location = new Point(0, 809);
            BTNDeconnexion.Name = "BTNDeconnexion";
            BTNDeconnexion.Size = new Size(205, 50);
            BTNDeconnexion.TabIndex = 8;
            BTNDeconnexion.Text = "Deconnexion";
            BTNDeconnexion.UseVisualStyleBackColor = false;
            // 
            // BTNpresence
            // 
            BTNpresence.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BTNpresence.ImageAlign = ContentAlignment.MiddleLeft;
            BTNpresence.Location = new Point(0, 533);
            BTNpresence.Name = "BTNpresence";
            BTNpresence.Size = new Size(205, 50);
            BTNpresence.TabIndex = 7;
            BTNpresence.Text = "Presence";
            BTNpresence.UseVisualStyleBackColor = true;
            // 
            // BTNEmployer
            // 
            BTNEmployer.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BTNEmployer.ImageAlign = ContentAlignment.MiddleLeft;
            BTNEmployer.Location = new Point(0, 476);
            BTNEmployer.Name = "BTNEmployer";
            BTNEmployer.Size = new Size(205, 50);
            BTNEmployer.TabIndex = 5;
            BTNEmployer.Text = "Employers";
            BTNEmployer.UseVisualStyleBackColor = true;
            // 
            // BTNUtils
            // 
            BTNUtils.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BTNUtils.ImageAlign = ContentAlignment.MiddleLeft;
            BTNUtils.Location = new Point(-1, 417);
            BTNUtils.Name = "BTNUtils";
            BTNUtils.Size = new Size(205, 50);
            BTNUtils.TabIndex = 4;
            BTNUtils.Text = "Utilisateurs";
            BTNUtils.UseVisualStyleBackColor = true;
            // 
            // AdminName
            // 
            AdminName.AutoSize = true;
            AdminName.Font = new Font("Segoe UI", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            AdminName.ForeColor = Color.Navy;
            AdminName.Location = new Point(18, 221);
            AdminName.Name = "AdminName";
            AdminName.Size = new Size(162, 21);
            AdminName.TabIndex = 3;
            AdminName.Text = "Nom administrateur";
            // 
            // LabelRole
            // 
            LabelRole.AutoSize = true;
            LabelRole.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LabelRole.ForeColor = Color.Navy;
            LabelRole.Location = new Point(51, 191);
            LabelRole.Name = "LabelRole";
            LabelRole.Size = new Size(87, 30);
            LabelRole.TabIndex = 2;
            LabelRole.Text = "ADMIN";
            // 
            // pictureBoxProfile
            // 
            pictureBoxProfile.Image = (Image)resources.GetObject("pictureBoxProfile.Image");
            pictureBoxProfile.Location = new Point(37, 46);
            pictureBoxProfile.Name = "pictureBoxProfile";
            pictureBoxProfile.Size = new Size(129, 135);
            pictureBoxProfile.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxProfile.TabIndex = 1;
            pictureBoxProfile.TabStop = false;
            // 
            // ButnDashboard
            // 
            ButnDashboard.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ButnDashboard.ImageAlign = ContentAlignment.MiddleLeft;
            ButnDashboard.Location = new Point(-1, 358);
            ButnDashboard.Name = "ButnDashboard";
            ButnDashboard.Size = new Size(205, 50);
            ButnDashboard.TabIndex = 0;
            ButnDashboard.Text = "Dashboard";
            ButnDashboard.UseVisualStyleBackColor = true;
            // 
            // panelNT
            // 
            panelNT.Controls.Add(labelCountNotificationIa);
            panelNT.Controls.Add(BTNAssistant);
            panelNT.Controls.Add(labeltitre);
            panelNT.Controls.Add(Labelwelcom);
            panelNT.Controls.Add(countNotification);
            panelNT.Controls.Add(PictureNNotifica);
            panelNT.Controls.Add(PictureNOTUserconnecter);
            panelNT.Location = new Point(216, 10);
            panelNT.Name = "panelNT";
            panelNT.Size = new Size(1393, 78);
            panelNT.TabIndex = 1;
            // 
            // labelCountNotificationIa
            // 
            labelCountNotificationIa.AutoSize = true;
            labelCountNotificationIa.BackColor = Color.Red;
            labelCountNotificationIa.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelCountNotificationIa.ForeColor = SystemColors.Window;
            labelCountNotificationIa.Location = new Point(1191, 13);
            labelCountNotificationIa.Name = "labelCountNotificationIa";
            labelCountNotificationIa.Size = new Size(19, 21);
            labelCountNotificationIa.TabIndex = 9;
            labelCountNotificationIa.Text = "1";
            // 
            // BTNAssistant
            // 
            BTNAssistant.Image = Properties.Resources.assistant_incon;
            BTNAssistant.Location = new Point(1165, 14);
            BTNAssistant.Name = "BTNAssistant";
            BTNAssistant.Size = new Size(45, 47);
            BTNAssistant.SizeMode = PictureBoxSizeMode.StretchImage;
            BTNAssistant.TabIndex = 8;
            BTNAssistant.TabStop = false;
            // 
            // labeltitre
            // 
            labeltitre.AutoSize = true;
            labeltitre.Font = new Font("Segoe UI", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            labeltitre.ForeColor = Color.Navy;
            labeltitre.Location = new Point(18, 42);
            labeltitre.Name = "labeltitre";
            labeltitre.Size = new Size(223, 21);
            labeltitre.TabIndex = 6;
            labeltitre.Text = "Gestion employer Dashboard";
            // 
            // Labelwelcom
            // 
            Labelwelcom.AutoSize = true;
            Labelwelcom.Font = new Font("Segoe UI", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            Labelwelcom.ForeColor = Color.Navy;
            Labelwelcom.Location = new Point(18, 11);
            Labelwelcom.Name = "Labelwelcom";
            Labelwelcom.Size = new Size(422, 21);
            Labelwelcom.TabIndex = 5;
            Labelwelcom.Text = "Bienvenu sur votre espace Utilisateur ou administrateur";
            // 
            // countNotification
            // 
            countNotification.AutoSize = true;
            countNotification.BackColor = Color.Red;
            countNotification.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            countNotification.ForeColor = SystemColors.Window;
            countNotification.Location = new Point(1262, 14);
            countNotification.Name = "countNotification";
            countNotification.Size = new Size(19, 21);
            countNotification.TabIndex = 4;
            countNotification.Text = "1";
            // 
            // PictureNNotifica
            // 
            PictureNNotifica.Image = (Image)resources.GetObject("PictureNNotifica.Image");
            PictureNNotifica.Location = new Point(1236, 14);
            PictureNNotifica.Name = "PictureNNotifica";
            PictureNNotifica.Size = new Size(45, 47);
            PictureNNotifica.SizeMode = PictureBoxSizeMode.StretchImage;
            PictureNNotifica.TabIndex = 3;
            PictureNNotifica.TabStop = false;
            // 
            // PictureNOTUserconnecter
            // 
            PictureNOTUserconnecter.Image = (Image)resources.GetObject("PictureNOTUserconnecter.Image");
            PictureNOTUserconnecter.Location = new Point(1321, 9);
            PictureNOTUserconnecter.Name = "PictureNOTUserconnecter";
            PictureNOTUserconnecter.Size = new Size(54, 58);
            PictureNOTUserconnecter.SizeMode = PictureBoxSizeMode.StretchImage;
            PictureNOTUserconnecter.TabIndex = 2;
            PictureNOTUserconnecter.TabStop = false;
            // 
            // PanelMain
            // 
            PanelMain.Controls.Add(panelfooter);
            PanelMain.Location = new Point(216, 94);
            PanelMain.Name = "PanelMain";
            PanelMain.Size = new Size(1393, 778);
            PanelMain.TabIndex = 3;
            // 
            // panelfooter
            // 
            panelfooter.Controls.Add(linkLabelfooter);
            panelfooter.Location = new Point(3, 746);
            panelfooter.Name = "panelfooter";
            panelfooter.Size = new Size(1387, 26);
            panelfooter.TabIndex = 15;
            // 
            // linkLabelfooter
            // 
            linkLabelfooter.AutoSize = true;
            linkLabelfooter.Location = new Point(468, 8);
            linkLabelfooter.Name = "linkLabelfooter";
            linkLabelfooter.Size = new Size(336, 15);
            linkLabelfooter.TabIndex = 1;
            linkLabelfooter.TabStop = true;
            linkLabelfooter.Text = "© 2026 Gestion des employer HR Copyright tous droits reserve";
            // 
            // Dashboard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1616, 878);
            Controls.Add(PanelMain);
            Controls.Add(panelNT);
            Controls.Add(paneldashboard);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            IsMdiContainer = true;
            Name = "Dashboard";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Dashboard";
            FormClosing += Dashboard_FormClosing;
            Load += Dashboard_Load;
            paneldashboard.ResumeLayout(false);
            paneldashboard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxProfile).EndInit();
            panelNT.ResumeLayout(false);
            panelNT.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)BTNAssistant).EndInit();
            ((System.ComponentModel.ISupportInitialize)PictureNNotifica).EndInit();
            ((System.ComponentModel.ISupportInitialize)PictureNOTUserconnecter).EndInit();
            PanelMain.ResumeLayout(false);
            panelfooter.ResumeLayout(false);
            panelfooter.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel paneldashboard;
        private Label LabelRole;
        private PictureBox pictureBoxProfile;
        private Button ButnDashboard;
        private Label AdminName;
        private Button BTNEmployer;
        private Button BTNUtils;
        private Panel panelNT;
        private Label countNotification;
        private PictureBox PictureNNotifica;
        private PictureBox PictureNOTUserconnecter;
        private Button BTNpresence;
        private Label labeltitre;
        private Label Labelwelcom;
        private Button BTNDeconnexion;
        private Label titreApp;
        private Panel PanelMain;
        private Label labelCountNotificationIa;
        private PictureBox BTNAssistant;
        private Panel panelfooter;
        private LinkLabel linkLabelfooter;
    }
}
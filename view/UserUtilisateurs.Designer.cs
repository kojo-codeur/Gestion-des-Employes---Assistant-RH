namespace Gestion_des_Employés.view
{
    partial class UserUtilisateurs
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
            titreUtilisateur = new Label();
            panelUserUtilisateurControle = new Panel();
            SuspendLayout();
            // 
            // titreUtilisateur
            // 
            titreUtilisateur.AutoSize = true;
            titreUtilisateur.Font = new Font("Segoe UI", 18F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            titreUtilisateur.Location = new Point(523, 21);
            titreUtilisateur.Name = "titreUtilisateur";
            titreUtilisateur.Size = new Size(277, 32);
            titreUtilisateur.TabIndex = 0;
            titreUtilisateur.Text = "Gestion des utilisateur ";
            // 
            // panelUserUtilisateurControle
            // 
            panelUserUtilisateurControle.Location = new Point(23, 66);
            panelUserUtilisateurControle.Name = "panelUserUtilisateurControle";
            panelUserUtilisateurControle.Size = new Size(1339, 692);
            panelUserUtilisateurControle.TabIndex = 1;
            // 
            // UserUtilisateurs
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panelUserUtilisateurControle);
            Controls.Add(titreUtilisateur);
            Name = "UserUtilisateurs";
            Size = new Size(1378, 778);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label titreUtilisateur;
        private Panel panelUserUtilisateurControle;
    }
}

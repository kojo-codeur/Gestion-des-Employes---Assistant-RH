namespace Gestion_des_Employés.view
{
    partial class UserEmployers
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
            PanelAfficheEmployerClick = new Panel();
            titreemployer = new Label();
            SuspendLayout();
            // 
            // PanelAfficheEmployerClick
            // 
            PanelAfficheEmployerClick.Location = new Point(24, 82);
            PanelAfficheEmployerClick.Name = "PanelAfficheEmployerClick";
            PanelAfficheEmployerClick.Size = new Size(1330, 668);
            PanelAfficheEmployerClick.TabIndex = 19;
            // 
            // titreemployer
            // 
            titreemployer.AutoSize = true;
            titreemployer.Font = new Font("Segoe UI", 18F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            titreemployer.Location = new Point(496, 28);
            titreemployer.Name = "titreemployer";
            titreemployer.Size = new Size(238, 32);
            titreemployer.TabIndex = 12;
            titreemployer.Text = "Liste des Employers";
            // 
            // UserEmployers
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(PanelAfficheEmployerClick);
            Controls.Add(titreemployer);
            Name = "UserEmployers";
            Size = new Size(1378, 778);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel PanelAfficheEmployerClick;
        private Label titreemployer;
    }
}

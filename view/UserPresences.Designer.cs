namespace Gestion_des_Employés.view
{
    partial class UserPresences
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
            PanelAffichePresenceBTNClick = new Panel();
            TitrePresence = new Label();
            SuspendLayout();
            // 
            // PanelAffichePresenceBTNClick
            // 
            PanelAffichePresenceBTNClick.Location = new Point(34, 102);
            PanelAffichePresenceBTNClick.Name = "PanelAffichePresenceBTNClick";
            PanelAffichePresenceBTNClick.Size = new Size(1320, 641);
            PanelAffichePresenceBTNClick.TabIndex = 19;
            // 
            // TitrePresence
            // 
            TitrePresence.AutoSize = true;
            TitrePresence.Font = new Font("Segoe UI", 18F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            TitrePresence.Location = new Point(476, 26);
            TitrePresence.Name = "TitrePresence";
            TitrePresence.Size = new Size(300, 32);
            TitrePresence.TabIndex = 12;
            TitrePresence.Text = "Enregistre une presences";
            // 
            // UserPresences
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(PanelAffichePresenceBTNClick);
            Controls.Add(TitrePresence);
            Name = "UserPresences";
            Size = new Size(1378, 778);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel PanelAffichePresenceBTNClick;
        private Label TitrePresence;
    }
}

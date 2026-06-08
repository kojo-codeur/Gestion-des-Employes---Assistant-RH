using Gestion_des_Employés.Controler;
using Gestion_des_Employés.models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;


namespace Gestion_des_Employés.view
{
    public partial class UserConnexion : UserControl
    {
        private DatabaseHelper db = new DatabaseHelper();

        public UserConnexion()
        {
            InitializeComponent();
            this.Load += UserConnexion_Load;

        }


        private void UserConnexion_Load(object sender, EventArgs e)
        {
            ApplyDesign();

        }

        private void ApplyDesign()
        {
            desinemodel.MakeCirclePicture(pictureboxuserlogin);
            desinemodel.StyleTextBox(TBEmail);
            desinemodel.StyleButton(butlogin);
            desinemodel.StyleLabel(labelOublier);
        }

        

        private void butlogin_Click(object sender, EventArgs e)
        {
            string email = TBEmail.Text.Trim();
            string password = TBPassword.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Veuillez saisir votre email et mot de passe.", "Champs requis", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Utilisateur user = db.Authentifier(email, password);
            if (user == null)
            {
                MessageBox.Show("Email ou mot de passe incorrect.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (user.Statut == "en_attente")
            {
                MessageBox.Show("Votre compte est en attente de validation par l'administrateur.", "Compte non validé", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (user.Statut == "inactif")
            {
                MessageBox.Show("Votre compte a été désactivé. Contactez l'administrateur.", "Compte inactif", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Sauvegarde des identifiants si "Se souvenir de moi"
            //SaveCredentials(email, password, checkBox1.Checked);

            // Sauvegarde de l'avatar dans les paramètres
            byte[] avatarBytes = db.GetUserAvatar(user.Id);
            //SaveUserAvatar(avatarBytes);

            // Initialisation de la session
            SessionManager.SetUser(user);

            // Ouvrir le tableau de bord
            Dashboard dashboard = new Dashboard();
            dashboard.Show();

            // Fermer la fenêtre de connexion (le parent)
            Form parent = this.FindForm();
            parent?.Hide();
        }
    }
}

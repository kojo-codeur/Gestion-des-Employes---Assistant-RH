using Gestion_des_Employés.Controler;
using Gestion_des_Employés.models;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Gestion_des_Employés.view
{
    public partial class UserProfile : UserControl
    {
        private DatabaseHelper db = new DatabaseHelper();
        private byte[] selectedAvatarBytes = null;

        public UserProfile()
        {
            InitializeComponent();
            Load += UserProfile_Load;
            pictureBox1.Click += PictureBox1_Click;
            BTNVALiderMODIFICATION.Click += BTNVALiderMODIFICATION_Click;
        }

        private void UserProfile_Load(object sender, EventArgs e)
        {
            ApplyDesign();
            LoadUserData();
        }

        private void ApplyDesign()
        {
            // Avatar
            desinemodel.MakeCirclePicture(pictureBox1);
            // Labels d'information
            foreach (Label lbl in new[] { LabelNomUtilisateur, labelPrenom, labeladresseUtilsateur, labelTelUtilisateur, labeldepartementUtilisateur, labelEmailUtilisateur })
                desinemodel.StyleLabel(lbl, false);
            // Titre "Modifier vos informations"
            desinemodel.StyleSectionHeader(labeltitremodifierinfo);
            // Champs de texte
            foreach (TextBox tb in new[] { textBox1, textBox2, textBox3, textBox4, textBox5, textBox6, textBox7, textBox8 })
                desinemodel.StyleTextBox(tb);
            // Groupe sexe
            desinemodel.Rounded(groupBoxSexe, 15);
            groupBoxSexe.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            // Bouton de validation
            desinemodel.StyleActionButton(BTNVALiderMODIFICATION, Color.LimeGreen);
        }

        private void LoadUserData()
        {
            if (!SessionManager.IsLoggedIn)
            {
                MessageBox.Show("Aucun utilisateur connecté.");
                return;
            }

            int userId = SessionManager.CurrentUserId;
            Utilisateur user = db.GetUtilisateurById(userId);
            if (user == null) return;

            // Affichage des informations actuelles
            LabelNomUtilisateur.Text = user.Nom;
            labelPrenom.Text = user.Prenom;
            labeladresseUtilsateur.Text = user.Adresse ?? "Non renseigné";
            labelTelUtilisateur.Text = user.Telephone ?? "Non renseigné";
            labeldepartementUtilisateur.Text = user.Departement ?? "Non renseigné";
            labelEmailUtilisateur.Text = user.Email;

            // Remplir les champs de modification
            textBox1.Text = user.Nom;
            textBox2.Text = user.Prenom;
            textBox8.Text = user.Adresse ?? "";
            textBox7.Text = user.Telephone ?? "";
            textBox3.Text = user.Departement ?? "";
            textBox6.Text = user.Email;

            // Sexe
            if (user.Sexe == "Homme")
                radioButtonHomme.Checked = true;
            else if (user.Sexe == "Femme")
                radioButtonFEMME.Checked = true;

            // Avatar
            byte[] avatarBytes = db.GetUserAvatar(userId);
            if (avatarBytes != null && avatarBytes.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream(avatarBytes))
                {
                    pictureBox1.Image = Image.FromStream(ms);
                }
            }
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Images|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Image img = Image.FromFile(ofd.FileName);
                        // Redimensionner pour éviter des fichiers trop lourds
                        Image resized = new Bitmap(img, new Size(150, 150));
                        pictureBox1.Image = resized;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            resized.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            selectedAvatarBytes = ms.ToArray();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erreur lors du chargement de l'image : " + ex.Message);
                    }
                }
            }
        }

        private void BTNVALiderMODIFICATION_Click(object sender, EventArgs e)
        {
            if (!SessionManager.IsLoggedIn)
            {
                MessageBox.Show("Vous n'êtes pas connecté.");
                return;
            }

            string nom = textBox1.Text.Trim();
            string prenom = textBox2.Text.Trim();
            string adresse = textBox8.Text.Trim();
            string telephone = textBox7.Text.Trim();
            string departement = textBox3.Text.Trim();
            string email = textBox6.Text.Trim();
            string sexe = radioButtonHomme.Checked ? "Homme" : (radioButtonFEMME.Checked ? "Femme" : "");

            if (string.IsNullOrEmpty(nom) || string.IsNullOrEmpty(prenom) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Le nom, prénom et email sont obligatoires.");
                return;
            }

            int userId = SessionManager.CurrentUserId;
            bool updated = db.UpdateUtilisateur(userId, nom, prenom, email, telephone, adresse, departement, sexe);
            if (updated)
            {
                // Mise à jour de l'avatar si une nouvelle image a été choisie
                if (selectedAvatarBytes != null)
                {
                    db.UpdateUserAvatar(userId, selectedAvatarBytes);
                }
                // Mettre à jour la session
                SessionManager.CurrentUserNom = nom;
                SessionManager.CurrentUserPrenom = prenom;
                SessionManager.CurrentUserEmail = email;
                // Recharger les labels
                LoadUserData();
                MessageBox.Show("Profil mis à jour avec succès.");
            }
            else
            {
                MessageBox.Show("Erreur lors de la mise à jour. Vérifiez que l'email n'est pas déjà utilisé.");
            }
        }
    }
}
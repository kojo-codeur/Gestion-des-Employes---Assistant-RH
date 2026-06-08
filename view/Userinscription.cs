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
    public partial class Userinscription : UserControl
    {

        private DatabaseHelper db = new DatabaseHelper();
        private byte[] selectedAvatarBytes = null;

        public Userinscription()
        {
            InitializeComponent();

            Load += Userinscription_Load;
            butInscription.Click += butInscription_Click;
            pictureboxuser.Click += pictureboxuser_Click;

        }



        private void ApplyDesign()
        {
            desinemodel.MakeCirclePicture(pictureboxuser);
            desinemodel.StyleTextBox(TBName);
            desinemodel.StyleTextBox(TBEmail);
            desinemodel.StyleTextBox(TBPassword);
            desinemodel.StyleTextBox(TBConfirmationPass);
            desinemodel.StyleButton(butInscription);
            desinemodel.StylePasswordTextBox(TBPassword);
            desinemodel.StylePasswordTextBox(TBConfirmationPass);
        }

        private void Userinscription_Load(object sender, EventArgs e)
        {
            ApplyDesign();
        }

        private void butInscription_Click(object sender, EventArgs e)
        {
            string nomComplet = TBName.Text.Trim();
            string email = TBEmail.Text.Trim();
            string password = TBPassword.Text.Trim();
            string confirm = TBConfirmationPass.Text.Trim();

            if (string.IsNullOrEmpty(nomComplet) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Tous les champs sont obligatoires.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != confirm)
            {
                MessageBox.Show("Les mots de passe ne correspondent pas.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!checkTerme.Checked)
            {
                MessageBox.Show("Vous devez accepter les conditions d'utilisation.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Séparer nom et prénom (premier mot = nom, reste = prénom)
            string[] parts = nomComplet.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string nom = parts.Length > 0 ? parts[0] : "";
            string prenom = parts.Length > 1 ? string.Join(" ", parts, 1, parts.Length - 1) : "";

            bool success = db.AddUtilisateur(nom, prenom, email, password, selectedAvatarBytes);
            if (success)
            {
                MessageBox.Show("Inscription réussie ! En attente de validation par l'administrateur.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TBName.Clear();
                TBEmail.Clear();
                TBPassword.Clear();
                TBConfirmationPass.Clear();
                checkTerme.Checked = false;
                pictureboxuser.Image = null;
                selectedAvatarBytes = null;
            }
            else
            {
                MessageBox.Show("Cet email est déjà utilisé ou une erreur est survenue.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureboxuser_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Images|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Image img = Image.FromFile(ofd.FileName);
                        // Redimensionner pour éviter les fichiers trop lourds
                        Image resized = new Bitmap(img, new Size(150, 150));
                        pictureboxuser.Image = resized;
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
    }
}





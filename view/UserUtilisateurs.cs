using Gestion_des_Employés.Controler;
using Gestion_des_Employés.models;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Gestion_des_Employés.view
{
    public partial class UserUtilisateurs : UserControl
    {
        private DatabaseHelper db = new DatabaseHelper();
        private DataGridView dgvUtilisateurs;
        private Label lblBadgeEnAttente, lblBadgeActif;
        private Button btnTous, btnEnAttente, btnActifs;
        private Button btnAjouter, btnModifier, btnSupprimer, btnValider, btnRejeter;
        private string currentFilter = "all";

        public UserUtilisateurs()
        {
            InitializeComponent();
            Load += (s, e) => BuildInterface();
        }

        private void BuildInterface()
        {
            panelUserUtilisateurControle.Controls.Clear();
            panelUserUtilisateurControle.Dock = DockStyle.Fill;
            panelUserUtilisateurControle.AutoScroll = true;

            // TableLayoutPrincipal : 3 lignes (badges, filtres, reste)
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(0)
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45)); // badges
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45)); // filtres
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // DataGridView + boutons action
            panelUserUtilisateurControle.Controls.Add(mainLayout);

            // ----- Ligne 0 : Badges -----
            FlowLayoutPanel flowBadges = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10, 5, 10, 0),
                BackColor = Color.Transparent
            };
            Label lblAttente = new Label { Text = "En attente :", Font = new Font("Segoe UI", 10, FontStyle.Bold), AutoSize = true };
            lblBadgeEnAttente = new Label { Text = "0", AutoSize = false, Size = new Size(30, 30), TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.Orange, ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            desinemodel.Rounded(lblBadgeEnAttente, 15);
            Label lblActif = new Label { Text = "Actifs :", Font = new Font("Segoe UI", 10, FontStyle.Bold), AutoSize = true };
            lblBadgeActif = new Label { Text = "0", AutoSize = false, Size = new Size(30, 30), TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.Green, ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            desinemodel.Rounded(lblBadgeActif, 15);
            flowBadges.Controls.AddRange(new Control[] { lblAttente, lblBadgeEnAttente, lblActif, lblBadgeActif });
            mainLayout.Controls.Add(flowBadges, 0, 0);

            // ----- Ligne 1 : Boutons de filtre -----
            FlowLayoutPanel flowFiltres = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10, 5, 10, 0),
                BackColor = Color.Transparent
            };
            btnTous = new Button { Text = "Tous", Width = 120, Height = 32 };
            btnEnAttente = new Button { Text = "En attente", Width = 120, Height = 32 };
            btnActifs = new Button { Text = "Actifs", Width = 120, Height = 32 };
            foreach (var btn in new[] { btnTous, btnEnAttente, btnActifs })
            {
                desinemodel.StyleActionButton(btn);
                btn.Click += (s, e) =>
                {
                    currentFilter = btn == btnTous ? "all" : (btn == btnEnAttente ? "en_attente" : "actif");
                    LoadUtilisateurs();
                };
            }
            flowFiltres.Controls.AddRange(new Control[] { btnTous, btnEnAttente, btnActifs });
            mainLayout.Controls.Add(flowFiltres, 0, 1);

            // ----- Ligne 2 : DataGridView + boutons d'action -----
            Panel bottomContainer = new Panel { Dock = DockStyle.Fill };
            mainLayout.Controls.Add(bottomContainer, 0, 2);

            // DataGridView (remplit l'espace restant)
            dgvUtilisateurs = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            desinemodel.StyleDataGridView(dgvUtilisateurs);
            dgvUtilisateurs.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) ModifierUtilisateur(); };
            bottomContainer.Controls.Add(dgvUtilisateurs);

            // Boutons d'action (en bas)
            FlowLayoutPanel flowActions = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 55,
                Padding = new Padding(10, 5, 10, 5),
                BackColor = Color.Transparent,
                FlowDirection = FlowDirection.LeftToRight
            };
            btnAjouter = new Button { Text = "Ajouter", Width = 120, Height = 35 };
            btnModifier = new Button { Text = "Modifier", Width = 120, Height = 35 };
            btnSupprimer = new Button { Text = "Supprimer", Width = 120, Height = 35 };
            btnValider = new Button { Text = "Valider", Width = 120, Height = 35 };
            btnRejeter = new Button { Text = "Rejeter", Width = 120, Height = 35 };
            desinemodel.StyleActionButton(btnAjouter, Color.DodgerBlue);
            desinemodel.StyleActionButton(btnModifier, Color.Goldenrod);
            desinemodel.StyleActionButton(btnSupprimer, Color.OrangeRed);
            desinemodel.StyleActionButton(btnValider, Color.Green);
            desinemodel.StyleActionButton(btnRejeter, Color.DarkRed);
            btnAjouter.Click += (s, e) => AjouterUtilisateur();
            btnModifier.Click += (s, e) => ModifierUtilisateur();
            btnSupprimer.Click += (s, e) => SupprimerUtilisateur();
            btnValider.Click += (s, e) => ValiderUtilisateur();
            btnRejeter.Click += (s, e) => RejeterUtilisateur();
            flowActions.Controls.AddRange(new Control[] { btnAjouter, btnModifier, btnSupprimer, btnValider, btnRejeter });
            bottomContainer.Controls.Add(flowActions);

            // Charger les données
            UpdateBadges();
            LoadUtilisateurs();
        }

        private void UpdateBadges()
        {
            int nbEnAttente = db.GetUtilisateursByStatut("en_attente").Count;
            int nbActifs = db.GetUtilisateursByStatut("actif").Count;
            lblBadgeEnAttente.Text = nbEnAttente.ToString();
            lblBadgeActif.Text = nbActifs.ToString();
        }

        private void LoadUtilisateurs()
        {
            var utilisateurs = currentFilter switch
            {
                "en_attente" => db.GetUtilisateursByStatut("en_attente"),
                "actif" => db.GetUtilisateursByStatut("actif"),
                _ => db.GetAllUtilisateurs()
            };

            if (utilisateurs == null || utilisateurs.Count == 0)
            {
                dgvUtilisateurs.DataSource = null;
                return;
            }

            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Nom", typeof(string));
            table.Columns.Add("Prénom", typeof(string));
            table.Columns.Add("Email", typeof(string));
            table.Columns.Add("Rôle", typeof(string));
            table.Columns.Add("Statut", typeof(string));
            table.Columns.Add("Absences", typeof(int));

            foreach (var u in utilisateurs)
            {
                table.Rows.Add(u.Id, u.Nom, u.Prenom, u.Email, u.Role, u.Statut, u.NbAbsences);
            }

            dgvUtilisateurs.DataSource = table;
            if (dgvUtilisateurs.Columns["Id"] != null)
                dgvUtilisateurs.Columns["Id"].Visible = false;
        }

        // ---------- CRUD : Ajouter ----------
        private void AjouterUtilisateur()
        {
            using (var form = new Form())
            {
                form.Text = "Ajouter un utilisateur";
                form.Size = new Size(400, 380);
                form.StartPosition = FormStartPosition.CenterParent;
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.MaximizeBox = false;
                form.MinimizeBox = false;

                var lblNom = new Label { Text = "Nom :", Location = new Point(20, 20), AutoSize = true };
                var txtNom = new TextBox { Location = new Point(120, 17), Width = 200 };
                var lblPrenom = new Label { Text = "Prénom :", Location = new Point(20, 60), AutoSize = true };
                var txtPrenom = new TextBox { Location = new Point(120, 57), Width = 200 };
                var lblEmail = new Label { Text = "Email :", Location = new Point(20, 100), AutoSize = true };
                var txtEmail = new TextBox { Location = new Point(120, 97), Width = 200 };
                var lblMdp = new Label { Text = "Mot de passe :", Location = new Point(20, 140), AutoSize = true };
                var txtMdp = new TextBox { Location = new Point(120, 137), Width = 200, PasswordChar = '*' };
                var lblRole = new Label { Text = "Rôle :", Location = new Point(20, 180), AutoSize = true };
                var cmbRole = new ComboBox { Location = new Point(120, 177), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
                cmbRole.Items.AddRange(new[] { "User", "Admin" });
                cmbRole.SelectedIndex = 0;

                var btnOk = new Button { Text = "Ajouter", Location = new Point(120, 230), Width = 100 };
                var btnAnnuler = new Button { Text = "Annuler", Location = new Point(230, 230), Width = 100 };

                btnOk.Click += (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(txtNom.Text) || string.IsNullOrWhiteSpace(txtPrenom.Text) ||
                        string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtMdp.Text))
                    {
                        MessageBox.Show("Tous les champs sont obligatoires.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    bool ok = db.AddUtilisateur(txtNom.Text.Trim(), txtPrenom.Text.Trim(), txtEmail.Text.Trim(), txtMdp.Text.Trim(), null, cmbRole.SelectedItem.ToString());
                    if (ok)
                    {
                        MessageBox.Show("Utilisateur ajouté. En attente de validation.");
                        form.DialogResult = DialogResult.OK;
                        form.Close();
                    }
                    else
                    {
                        MessageBox.Show("Erreur (email peut-être déjà utilisé).", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
                btnAnnuler.Click += (s, e) => form.Close();

                form.Controls.AddRange(new Control[] { lblNom, txtNom, lblPrenom, txtPrenom, lblEmail, txtEmail, lblMdp, txtMdp, lblRole, cmbRole, btnOk, btnAnnuler });
                if (form.ShowDialog() == DialogResult.OK)
                {
                    UpdateBadges();
                    LoadUtilisateurs();
                }
            }
        }

        // ---------- CRUD : Modifier ----------
        private void ModifierUtilisateur()
        {
            if (dgvUtilisateurs.CurrentRow == null) return;
            int id = Convert.ToInt32(dgvUtilisateurs.CurrentRow.Cells["Id"].Value);
            var user = db.GetUtilisateurById(id);
            if (user == null) return;

            using (var form = new Form())
            {
                form.Text = "Modifier l'utilisateur";
                form.Size = new Size(400, 420);
                form.StartPosition = FormStartPosition.CenterParent;
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.MaximizeBox = false;
                form.MinimizeBox = false;

                var lblNom = new Label { Text = "Nom :", Location = new Point(20, 20), AutoSize = true };
                var txtNom = new TextBox { Text = user.Nom, Location = new Point(120, 17), Width = 200 };
                var lblPrenom = new Label { Text = "Prénom :", Location = new Point(20, 60), AutoSize = true };
                var txtPrenom = new TextBox { Text = user.Prenom, Location = new Point(120, 57), Width = 200 };
                var lblEmail = new Label { Text = "Email :", Location = new Point(20, 100), AutoSize = true };
                var txtEmail = new TextBox { Text = user.Email, Location = new Point(120, 97), Width = 200 };
                var lblRole = new Label { Text = "Rôle :", Location = new Point(20, 140), AutoSize = true };
                var cmbRole = new ComboBox { Location = new Point(120, 137), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
                cmbRole.Items.AddRange(new[] { "User", "Admin" });
                cmbRole.SelectedItem = user.Role;

                var lblStatut = new Label { Text = "Statut :", Location = new Point(20, 180), AutoSize = true };
                var cmbStatut = new ComboBox { Location = new Point(120, 177), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
                cmbStatut.Items.AddRange(new[] { "actif", "inactif", "en_attente" });
                cmbStatut.SelectedItem = user.Statut;

                var btnOk = new Button { Text = "Modifier", Location = new Point(120, 230), Width = 100 };
                var btnAnnuler = new Button { Text = "Annuler", Location = new Point(230, 230), Width = 100 };

                btnOk.Click += (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(txtNom.Text) || string.IsNullOrWhiteSpace(txtPrenom.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
                    {
                        MessageBox.Show("Nom, prénom et email sont obligatoires.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    // Mettre à jour les infos de base
                    bool ok = db.UpdateUtilisateur(id, txtNom.Text.Trim(), txtPrenom.Text.Trim(), txtEmail.Text.Trim(),
                        user.Telephone, user.Adresse, user.Departement, user.Sexe);
                    // Mettre à jour le statut (besoin d'une méthode spécifique, sinon on le fait ici)
                    if (ok)
                    {
                        string sql = "UPDATE Utilisateur SET Statut = @statut WHERE Id = @id";
                        using (var conn = new MySql.Data.MySqlClient.MySqlConnection(db.GetConnectionString()))
                        {
                            conn.Open();
                            using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn))
                            {
                                cmd.Parameters.AddWithValue("@statut", cmbStatut.SelectedItem.ToString());
                                cmd.Parameters.AddWithValue("@id", id);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        MessageBox.Show("Utilisateur modifié.");
                        form.DialogResult = DialogResult.OK;
                        form.Close();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la modification.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
                btnAnnuler.Click += (s, e) => form.Close();

                form.Controls.AddRange(new Control[] { lblNom, txtNom, lblPrenom, txtPrenom, lblEmail, txtEmail, lblRole, cmbRole, lblStatut, cmbStatut, btnOk, btnAnnuler });
                if (form.ShowDialog() == DialogResult.OK)
                {
                    UpdateBadges();
                    LoadUtilisateurs();
                }
            }
        }

        // ---------- CRUD : Supprimer ----------
        private void SupprimerUtilisateur()
        {
            if (dgvUtilisateurs.CurrentRow == null) return;
            int id = Convert.ToInt32(dgvUtilisateurs.CurrentRow.Cells["Id"].Value);
            string nom = dgvUtilisateurs.CurrentRow.Cells["Nom"].Value.ToString();
            if (MessageBox.Show($"Supprimer définitivement l'utilisateur {nom} ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (db.DeleteUtilisateur(id))
                {
                    MessageBox.Show("Utilisateur supprimé.");
                    UpdateBadges();
                    LoadUtilisateurs();
                }
                else
                    MessageBox.Show("Erreur lors de la suppression.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ---------- Valider une inscription ----------
        private void ValiderUtilisateur()
        {
            if (dgvUtilisateurs.CurrentRow == null) return;
            int id = Convert.ToInt32(dgvUtilisateurs.CurrentRow.Cells["Id"].Value);
            string statut = dgvUtilisateurs.CurrentRow.Cells["Statut"].Value.ToString();
            if (statut != "en_attente")
            {
                MessageBox.Show("Cet utilisateur n'est pas en attente.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Valider cette inscription ?", "Validation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (db.ValiderUtilisateur(id))
                {
                    MessageBox.Show("Utilisateur validé.");
                    UpdateBadges();
                    LoadUtilisateurs();
                }
                else
                    MessageBox.Show("Erreur lors de la validation.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ---------- Rejeter une inscription ----------
        private void RejeterUtilisateur()
        {
            if (dgvUtilisateurs.CurrentRow == null) return;
            int id = Convert.ToInt32(dgvUtilisateurs.CurrentRow.Cells["Id"].Value);
            string statut = dgvUtilisateurs.CurrentRow.Cells["Statut"].Value.ToString();
            if (statut != "en_attente")
            {
                MessageBox.Show("Seules les inscriptions en attente peuvent être rejetées.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Rejeter cette inscription (supprimer l'utilisateur) ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (db.DeleteUtilisateur(id))
                {
                    MessageBox.Show("Inscription rejetée et utilisateur supprimé.");
                    UpdateBadges();
                    LoadUtilisateurs();
                }
                else
                    MessageBox.Show("Erreur lors du rejet.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
using Gestion_des_Employés.Controler;
using Gestion_des_Employés.models;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Gestion_des_Employés.view
{
    public partial class UserEmployers : UserControl
    {
        private DatabaseHelper db = new DatabaseHelper();
        private DataGridView dgvEmployes;
        private Label lblBadgeEnOrdre, lblBadgeNonOrdre;
        private Button btnTous, btnEnOrdre, btnNonOrdre;
        private Button btnAjouter, btnModifier, btnSupprimer, btnValider, btnRejeter;
        private string currentFilter = "all";

        public UserEmployers()
        {
            InitializeComponent();
            Load += (s, e) => BuildInterface();
        }

        private void BuildInterface()
        {
            PanelAfficheEmployerClick.Controls.Clear();
            PanelAfficheEmployerClick.Dock = DockStyle.Fill;
            PanelAfficheEmployerClick.BackColor = Color.Transparent;

            // TableLayoutPanel principal pour organiser l'espace
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(0)
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45)); // badges
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45)); // filtres
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // DataGridView + boutons
            PanelAfficheEmployerClick.Controls.Add(mainLayout);

            // --- Ligne 0 : Badges ---
            FlowLayoutPanel flowBadges = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10, 5, 10, 0),
                BackColor = Color.Transparent
            };
            Label lblOrdre = new Label { Text = "En ordre (abs. < 3) :", Font = new Font("Segoe UI", 10, FontStyle.Bold), AutoSize = true };
            lblBadgeEnOrdre = new Label { Text = "0", AutoSize = false, Size = new Size(32, 32), TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.Green, ForeColor = Color.White, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            desinemodel.Rounded(lblBadgeEnOrdre, 16);
            Label lblNonOrdre = new Label { Text = "Non en ordre (abs. ≥ 3) :", Font = new Font("Segoe UI", 10, FontStyle.Bold), AutoSize = true };
            lblBadgeNonOrdre = new Label { Text = "0", AutoSize = false, Size = new Size(32, 32), TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.Red, ForeColor = Color.White, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            desinemodel.Rounded(lblBadgeNonOrdre, 16);
            flowBadges.Controls.AddRange(new Control[] { lblOrdre, lblBadgeEnOrdre, lblNonOrdre, lblBadgeNonOrdre });
            mainLayout.Controls.Add(flowBadges, 0, 0);

            // --- Ligne 1 : Boutons de filtre ---
            FlowLayoutPanel flowFiltres = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10, 5, 10, 0),
                BackColor = Color.Transparent
            };
            btnTous = new Button { Text = "Tous", Width = 110, Height = 32 };
            btnEnOrdre = new Button { Text = "En ordre", Width = 160, Height = 32 };
            btnNonOrdre = new Button { Text = "Non en ordre", Width = 180, Height = 32 };
            foreach (var btn in new[] { btnTous, btnEnOrdre, btnNonOrdre })
            {
                desinemodel.StyleActionButton(btn);
                btn.Click += (s, e) =>
                {
                    currentFilter = btn == btnTous ? "all" : (btn == btnEnOrdre ? "ordre" : "nonordre");
                    LoadEmployes();
                };
            }
            flowFiltres.Controls.AddRange(new Control[] { btnTous, btnEnOrdre, btnNonOrdre });
            mainLayout.Controls.Add(flowFiltres, 0, 1);

            // --- Ligne 2 : DataGridView + boutons d'action (en bas) ---
            // Panel intermédiaire pour empiler DataGridView et boutons
            Panel bottomPanel = new Panel { Dock = DockStyle.Fill };
            mainLayout.Controls.Add(bottomPanel, 0, 2);

            dgvEmployes = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            desinemodel.StyleDataGridView(dgvEmployes);
            dgvEmployes.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) ModifierEmploye(); };
            bottomPanel.Controls.Add(dgvEmployes);

            // Boutons d'action en bas
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
            btnAjouter.Click += (s, e) => AjouterEmploye();
            btnModifier.Click += (s, e) => ModifierEmploye();
            btnSupprimer.Click += (s, e) => SupprimerEmploye();
            btnValider.Click += (s, e) => ValiderEmploye();
            btnRejeter.Click += (s, e) => RejeterEmploye();
            flowActions.Controls.AddRange(new Control[] { btnAjouter, btnModifier, btnSupprimer, btnValider, btnRejeter });
            bottomPanel.Controls.Add(flowActions);

            UpdateBadges();
            LoadEmployes();
        }

        private void UpdateBadges()
        {
            var allEmployes = db.GetAllUtilisateurs().Where(u => u.Role == "User").ToList();
            lblBadgeEnOrdre.Text = allEmployes.Count(e => e.NbAbsences < 3).ToString();
            lblBadgeNonOrdre.Text = allEmployes.Count(e => e.NbAbsences >= 3).ToString();
        }

        private void LoadEmployes()
        {
            var allEmployes = db.GetAllUtilisateurs().Where(u => u.Role == "User").ToList();
            if (allEmployes.Count == 0)
            {
                dgvEmployes.DataSource = null;
                return;
            }

            var filtered = currentFilter switch
            {
                "ordre" => allEmployes.Where(e => e.NbAbsences < 3).ToList(),
                "nonordre" => allEmployes.Where(e => e.NbAbsences >= 3).ToList(),
                _ => allEmployes
            };

            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Nom", typeof(string));
            table.Columns.Add("Prénom", typeof(string));
            table.Columns.Add("Email", typeof(string));
            table.Columns.Add("Département", typeof(string));
            table.Columns.Add("Absences", typeof(int));
            table.Columns.Add("Statut", typeof(string));

            foreach (var e in filtered)
                table.Rows.Add(e.Id, e.Nom, e.Prenom, e.Email, e.Departement ?? "Non défini", e.NbAbsences, e.Statut);

            dgvEmployes.DataSource = table;
            if (dgvEmployes.Columns["Id"] != null)
                dgvEmployes.Columns["Id"].Visible = false;
        }

        // --- CRUD : Ajouter ---
        private void AjouterEmploye()
        {
            using (var form = new Form())
            {
                form.Text = "Ajouter un employé";
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
                var lblDept = new Label { Text = "Département :", Location = new Point(20, 180), AutoSize = true };
                var txtDept = new TextBox { Location = new Point(120, 177), Width = 200 };

                var btnOk = new Button { Text = "Ajouter", Location = new Point(120, 230), Width = 100 };
                var btnAnnuler = new Button { Text = "Annuler", Location = new Point(230, 230), Width = 100 };

                btnOk.Click += (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(txtNom.Text) || string.IsNullOrWhiteSpace(txtPrenom.Text) ||
                        string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtMdp.Text))
                    {
                        MessageBox.Show("Tous les champs (sauf département) sont obligatoires.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    bool ok = db.AddUtilisateur(txtNom.Text.Trim(), txtPrenom.Text.Trim(), txtEmail.Text.Trim(), txtMdp.Text.Trim(), null, "User");
                    if (ok)
                    {
                        var user = db.Authentifier(txtEmail.Text.Trim(), txtMdp.Text.Trim());
                        if (user != null && !string.IsNullOrEmpty(txtDept.Text))
                            db.UpdateUtilisateur(user.Id, user.Nom, user.Prenom, user.Email, null, null, txtDept.Text.Trim(), null);
                        MessageBox.Show("Employé ajouté. En attente de validation.");
                        form.DialogResult = DialogResult.OK;
                        form.Close();
                    }
                    else
                        MessageBox.Show("Erreur (email peut-être déjà utilisé).", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                };
                btnAnnuler.Click += (s, e) => form.Close();

                form.Controls.AddRange(new Control[] { lblNom, txtNom, lblPrenom, txtPrenom, lblEmail, txtEmail, lblMdp, txtMdp, lblDept, txtDept, btnOk, btnAnnuler });
                if (form.ShowDialog() == DialogResult.OK)
                {
                    UpdateBadges();
                    LoadEmployes();
                }
            }
        }

        // --- CRUD : Modifier ---
        private void ModifierEmploye()
        {
            if (dgvEmployes.CurrentRow == null) return;
            int id = Convert.ToInt32(dgvEmployes.CurrentRow.Cells["Id"].Value);
            var user = db.GetUtilisateurById(id);
            if (user == null) return;

            using (var form = new Form())
            {
                form.Text = "Modifier l'employé";
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
                var lblDept = new Label { Text = "Département :", Location = new Point(20, 140), AutoSize = true };
                var txtDept = new TextBox { Text = user.Departement, Location = new Point(120, 137), Width = 200 };
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
                    bool ok = db.UpdateUtilisateur(id, txtNom.Text.Trim(), txtPrenom.Text.Trim(), txtEmail.Text.Trim(),
                        user.Telephone, user.Adresse, txtDept.Text.Trim(), user.Sexe);
                    if (ok)
                    {
                        if (cmbStatut.SelectedItem.ToString() != user.Statut)
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
                        }
                        MessageBox.Show("Employé modifié.");
                        form.DialogResult = DialogResult.OK;
                        form.Close();
                    }
                    else
                        MessageBox.Show("Erreur lors de la modification.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                };
                btnAnnuler.Click += (s, e) => form.Close();

                form.Controls.AddRange(new Control[] { lblNom, txtNom, lblPrenom, txtPrenom, lblEmail, txtEmail, lblDept, txtDept, lblStatut, cmbStatut, btnOk, btnAnnuler });
                if (form.ShowDialog() == DialogResult.OK)
                {
                    UpdateBadges();
                    LoadEmployes();
                }
            }
        }

        // --- CRUD : Supprimer ---
        private void SupprimerEmploye()
        {
            if (dgvEmployes.CurrentRow == null) return;
            int id = Convert.ToInt32(dgvEmployes.CurrentRow.Cells["Id"].Value);
            string nom = dgvEmployes.CurrentRow.Cells["Nom"].Value.ToString();
            if (MessageBox.Show($"Supprimer définitivement l'employé {nom} ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (db.DeleteUtilisateur(id))
                {
                    MessageBox.Show("Employé supprimé.");
                    UpdateBadges();
                    LoadEmployes();
                }
                else
                    MessageBox.Show("Erreur lors de la suppression.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- Valider un employé (en_attente -> actif) ---
        private void ValiderEmploye()
        {
            if (dgvEmployes.CurrentRow == null) return;
            int id = Convert.ToInt32(dgvEmployes.CurrentRow.Cells["Id"].Value);
            string statut = dgvEmployes.CurrentRow.Cells["Statut"].Value.ToString();
            if (statut != "en_attente")
            {
                MessageBox.Show("Cet employé n'est pas en attente.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Valider cet employé ?", "Validation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (db.ValiderUtilisateur(id))
                {
                    MessageBox.Show("Employé validé.");
                    UpdateBadges();
                    LoadEmployes();
                }
                else
                    MessageBox.Show("Erreur lors de la validation.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- Rejeter un employé (supprimer le compte) ---
        private void RejeterEmploye()
        {
            if (dgvEmployes.CurrentRow == null) return;
            int id = Convert.ToInt32(dgvEmployes.CurrentRow.Cells["Id"].Value);
            string statut = dgvEmployes.CurrentRow.Cells["Statut"].Value.ToString();
            if (statut != "en_attente")
            {
                MessageBox.Show("Seuls les employés en attente peuvent être rejetés.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Rejeter cet employé (supprimer son compte) ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (db.DeleteUtilisateur(id))
                {
                    MessageBox.Show("Employé rejeté et supprimé.");
                    UpdateBadges();
                    LoadEmployes();
                }
                else
                    MessageBox.Show("Erreur lors du rejet.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
using Gestion_des_Employés.Controler;
using Gestion_des_Employés.models;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Gestion_des_Employés.view
{
    public partial class UserPresences : UserControl
    {
        private DatabaseHelper db = new DatabaseHelper();
        private TabControl tabControl;
        private bool isAdmin;

        public UserPresences()
        {
            InitializeComponent();
            Load += (s, e) => ApplyDesign();
        }

        private void ApplyDesign()
        {
            // Styliser les contrôles du designer
            desinemodel.StyleSectionHeader(TitrePresence);
            desinemodel.StyleContentPanel(PanelAffichePresenceBTNClick);

            isAdmin = SessionManager.CurrentUserRole == "Admin";

            // Créer le TabControl
            tabControl = new TabControl { Dock = DockStyle.Fill };
            PanelAffichePresenceBTNClick.Controls.Clear();
            PanelAffichePresenceBTNClick.Controls.Add(tabControl);

            // Onglet 1 : Pointage (entrée/sortie)
            TabPage pagePointage = new TabPage("Pointage");
            tabControl.TabPages.Add(pagePointage);
            CreerPointagePage(pagePointage);

            // Onglet 2 : Mes présences / absences
            TabPage pageHistorique = new TabPage("Mes présences");
            tabControl.TabPages.Add(pageHistorique);
            CreerHistoriquePage(pageHistorique);

            // Onglet 3 : Demandes de congé (employé) + gestion admin
            TabPage pageConges = new TabPage("Congés");
            tabControl.TabPages.Add(pageConges);
            CreerCongesPage(pageConges);

            // Onglet 4 : Jours fériés (admin seulement)
            if (isAdmin)
            {
                TabPage pageFeries = new TabPage("Jours fériés");
                tabControl.TabPages.Add(pageFeries);
                CreerFeriesPage(pageFeries);
            }

        }

        // ------------------------------------------------------------
        // 1. Page Pointage (entrée / sortie)
        // ------------------------------------------------------------
        private void CreerPointagePage(TabPage page)
        {
            FlowLayoutPanel flow = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, FlowDirection = FlowDirection.LeftToRight, Padding = new Padding(10) };
            page.Controls.Add(flow);

            // Panneau entrée
            Panel panelEntree = new Panel { Width = 360, Height = 180, Margin = new Padding(5), BorderStyle = BorderStyle.FixedSingle };
            desinemodel.Rounded(panelEntree, 10);
            panelEntree.BackColor = Color.WhiteSmoke;
            Label lblTitreEntree = new Label { Text = "POINTAGE ENTRÉE (avant 8h30)", Location = new Point(10, 10), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            DateTimePicker dtpDateEntree = new DateTimePicker { Format = DateTimePickerFormat.Short, Location = new Point(80, 47), Width = 100 };
            NumericUpDown nudHeureEntree = new NumericUpDown { Minimum = 0, Maximum = 23, Value = DateTime.Now.Hour, Location = new Point(80, 77), Width = 45 };
            NumericUpDown nudMinuteEntree = new NumericUpDown { Minimum = 0, Maximum = 59, Value = DateTime.Now.Minute, Location = new Point(130, 77), Width = 45 };
            Button btnEntree = new Button { Text = "Pointer mon entrée", Location = new Point(190, 70), Size = new Size(140, 35) };
            desinemodel.StyleButton(btnEntree);
            Label lblStatutEntree = new Label { Text = "", Location = new Point(10, 130), AutoSize = true, ForeColor = Color.Blue };
            panelEntree.Controls.AddRange(new Control[] { lblTitreEntree, new Label { Text = "Date :", Location = new Point(10, 50), AutoSize = true }, dtpDateEntree,
                new Label { Text = "Heure :", Location = new Point(10, 80), AutoSize = true }, nudHeureEntree, nudMinuteEntree, btnEntree, lblStatutEntree });

            // Panneau sortie
            Panel panelSortie = new Panel { Width = 360, Height = 180, Margin = new Padding(5), BorderStyle = BorderStyle.FixedSingle };
            desinemodel.Rounded(panelSortie, 10);
            panelSortie.BackColor = Color.WhiteSmoke;
            Label lblTitreSortie = new Label { Text = "POINTAGE SORTIE (après 17h30)", Location = new Point(10, 10), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            DateTimePicker dtpDateSortie = new DateTimePicker { Format = DateTimePickerFormat.Short, Location = new Point(80, 47), Width = 100 };
            NumericUpDown nudHeureSortie = new NumericUpDown { Minimum = 0, Maximum = 23, Value = DateTime.Now.Hour, Location = new Point(80, 77), Width = 45 };
            NumericUpDown nudMinuteSortie = new NumericUpDown { Minimum = 0, Maximum = 59, Value = DateTime.Now.Minute, Location = new Point(130, 77), Width = 45 };
            Button btnSortie = new Button { Text = "Pointer ma sortie", Location = new Point(190, 70), Size = new Size(140, 35) };
            desinemodel.StyleButton(btnSortie);
            Label lblStatutSortie = new Label { Text = "", Location = new Point(10, 130), AutoSize = true, ForeColor = Color.Blue };
            panelSortie.Controls.AddRange(new Control[] { lblTitreSortie, new Label { Text = "Date :", Location = new Point(10, 50), AutoSize = true }, dtpDateSortie,
                new Label { Text = "Heure :", Location = new Point(10, 80), AutoSize = true }, nudHeureSortie, nudMinuteSortie, btnSortie, lblStatutSortie });

            flow.Controls.Add(panelEntree);
            flow.Controls.Add(panelSortie);

            // Événements
            btnEntree.Click += (s, e) => PointerEntree((int)nudHeureEntree.Value, (int)nudMinuteEntree.Value, dtpDateEntree.Value, lblStatutEntree);
            btnSortie.Click += (s, e) => PointerSortie((int)nudHeureSortie.Value, (int)nudMinuteSortie.Value, dtpDateSortie.Value, lblStatutSortie);
            RafraichirStatutPointage(lblStatutEntree, lblStatutSortie);
        }

        private void PointerEntree(int h, int m, DateTime date, Label lbl)
        {
            int userId = SessionManager.CurrentUserId;
            TimeSpan heure = new TimeSpan(h, m, 0);
            if (db.EstFerie(date))
            { MessageBox.Show("Jour férié, pas de pointage."); return; }
            if (db.ADejaPointéEntree(userId, date))
            { MessageBox.Show("Entrée déjà pointée."); return; }
            if (db.EnregistrerEntree(userId, date, heure))
            {
                MessageBox.Show($"Entrée à {heure:hh\\:mm} enregistrée.");
                RafraichirStatutPointage(lbl, null);
            }
            else MessageBox.Show("Erreur.");
        }

        private void PointerSortie(int h, int m, DateTime date, Label lbl)
        {
            int userId = SessionManager.CurrentUserId;
            TimeSpan heure = new TimeSpan(h, m, 0);
            if (db.EstFerie(date))
            { MessageBox.Show("Jour férié, pas de pointage."); return; }
            if (!db.ADejaPointéEntree(userId, date))
            { MessageBox.Show("Entrée non pointée."); return; }
            if (db.ADejaPointéSortie(userId, date))
            { MessageBox.Show("Sortie déjà pointée."); return; }
            if (db.EnregistrerSortie(userId, date, heure))
            {
                MessageBox.Show($"Sortie à {heure:hh\\:mm} enregistrée.");
                RafraichirStatutPointage(null, lbl);
            }
            else MessageBox.Show("Erreur.");
        }

        private void RafraichirStatutPointage(Label lblEntree, Label lblSortie)
        {
            int userId = SessionManager.CurrentUserId;
            DateTime today = DateTime.Today;
            DataRow row = db.GetPresenceDuJour(userId, today);
            if (row != null)
            {
                if (lblEntree != null) lblEntree.Text = $"Statut entrée : {row["Statut"]}";
                if (lblSortie != null) lblSortie.Text = $"Statut sortie : {(row["HeureSortie"] == DBNull.Value ? "Non pointée" : (row["StatutDepart"]?.ToString() == "Present" ? "Présent" : "Absent"))}";
            }
            else
            {
                if (lblEntree != null) lblEntree.Text = "Aucun pointage";
                if (lblSortie != null) lblSortie.Text = "";
            }
        }

        // ------------------------------------------------------------
        // 2. Page Historique des présences
        // ------------------------------------------------------------
        private void CreerHistoriquePage(TabPage page)
        {
            DataGridView dgv = new DataGridView { Dock = DockStyle.Fill, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true };
            desinemodel.StyleDataGridView(dgv);
            page.Controls.Add(dgv);
            ChargerHistorique(dgv);
        }

        private void ChargerHistorique(DataGridView dgv)
        {
            int userId = SessionManager.CurrentUserId;
            DataTable dt = db.GetPresencesByUser(userId);
            if (dt.Rows.Count == 0) return;
            dgv.DataSource = dt;
            if (dgv.Columns["Id"] != null) dgv.Columns["Id"].Visible = false;
            if (dgv.Columns["UtilisateurId"] != null) dgv.Columns["UtilisateurId"].Visible = false;
        }

        // ------------------------------------------------------------
        // 3. Page Congés (demande + liste)
        // ------------------------------------------------------------
        private void CreerCongesPage(TabPage page)
        {
            // Pour l'admin : pas de formulaire de demande, seulement la liste
            // Pour l'utilisateur : formulaire + liste de ses demandes
            if (isAdmin)
            {
                // Admin : seulement une DataGridView avec toutes les demandes en attente
                DataGridView dgvDemandes = new DataGridView
                {
                    Dock = DockStyle.Fill,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    ReadOnly = true,
                    AllowUserToAddRows = false,
                    RowHeadersVisible = false,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect
                };
                desinemodel.StyleDataGridView(dgvDemandes);
                page.Controls.Add(dgvDemandes);

                // Ajout des boutons Valider/Refuser en bas
                FlowLayoutPanel flowActions = new FlowLayoutPanel
                {
                    Dock = DockStyle.Bottom,
                    Height = 45,
                    Padding = new Padding(10, 5, 10, 5),
                    BackColor = Color.Transparent
                };
                Button btnValider = new Button { Text = "✓ Valider la demande sélectionnée", Width = 200, Height = 35 };
                Button btnRefuser = new Button { Text = "✗ Refuser la demande sélectionnée", Width = 200, Height = 35 };
                desinemodel.StyleActionButton(btnValider, Color.Green);
                desinemodel.StyleActionButton(btnRefuser, Color.DarkRed);
                btnValider.Click += (s, e) => RepondreDemandeAdmin(dgvDemandes, "accepte");
                btnRefuser.Click += (s, e) => RepondreDemandeAdmin(dgvDemandes, "refuse");
                flowActions.Controls.Add(btnValider);
                flowActions.Controls.Add(btnRefuser);
                page.Controls.Add(flowActions);

                ChargerDemandesAdmin(dgvDemandes);
            }
            else
            {
                // Utilisateur normal : formulaire + liste de ses demandes
                SplitContainer split = new SplitContainer { Dock = DockStyle.Fill, Orientation = Orientation.Horizontal };
                split.SplitterDistance = 220;
                page.Controls.Add(split);

                // Partie haute : formulaire de demande
                Panel formPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };
                Label lblTitre = new Label { Text = "Nouvelle demande de congé", Font = new Font("Segoe UI", 12, FontStyle.Bold), Dock = DockStyle.Top, Height = 30 };
                DateTimePicker dtpDebut = new DateTimePicker { Format = DateTimePickerFormat.Short, Location = new Point(120, 50), Width = 120 };
                DateTimePicker dtpFin = new DateTimePicker { Format = DateTimePickerFormat.Short, Location = new Point(120, 90), Width = 120 };
                TextBox txtRaison = new TextBox { Location = new Point(120, 130), Width = 250, PlaceholderText = "Raison du congé" };
                Button btnDemander = new Button { Text = "Envoyer la demande", Location = new Point(120, 170), Size = new Size(150, 35) };
                desinemodel.StyleButton(btnDemander);
                formPanel.Controls.AddRange(new Control[] { lblTitre,
                    new Label { Text = "Date début :", Location = new Point(20, 55), AutoSize = true }, dtpDebut,
                    new Label { Text = "Date fin :", Location = new Point(20, 95), AutoSize = true }, dtpFin,
                    new Label { Text = "Raison :", Location = new Point(20, 135), AutoSize = true }, txtRaison,
                    btnDemander });
                split.Panel1.Controls.Add(formPanel);

                // Partie basse : liste de ses demandes
                DataGridView dgvDemandes = new DataGridView
                {
                    Dock = DockStyle.Fill,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    ReadOnly = true,
                    AllowUserToAddRows = false,
                    RowHeadersVisible = false,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect
                };
                desinemodel.StyleDataGridView(dgvDemandes);
                split.Panel2.Controls.Add(dgvDemandes);

                btnDemander.Click += (s, e) =>
                {
                    if (dtpDebut.Value > dtpFin.Value) { MessageBox.Show("Date début > date fin"); return; }
                    if (db.AjouterDemandeConge(SessionManager.CurrentUserId, dtpDebut.Value, dtpFin.Value, txtRaison.Text))
                    {
                        MessageBox.Show("Demande envoyée.");
                        ChargerMesDemandes(dgvDemandes);
                        txtRaison.Clear();
                    }
                    else MessageBox.Show("Erreur.");
                };

                ChargerMesDemandes(dgvDemandes);
            }
        }

        // Admin : charger toutes les demandes en attente
        private void ChargerDemandesAdmin(DataGridView dgv)
        {
            DataTable dt = db.GetDemandesConge(0, "en_attente");
            dgv.DataSource = dt;
            if (dgv.Columns["Id"] != null) dgv.Columns["Id"].Visible = true;
            if (dgv.Columns["UtilisateurId"] != null) dgv.Columns["UtilisateurId"].Visible = false;
            if (dgv.Columns["Statut"] != null) dgv.Columns["Statut"].HeaderText = "Statut";
            dgv.AutoResizeColumns();
        }

        // Admin : répondre à une demande
        private void RepondreDemandeAdmin(DataGridView dgv, string statut)
        {
            if (dgv.CurrentRow == null)
            {
                MessageBox.Show("Veuillez sélectionner une demande.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            int id = Convert.ToInt32(dgv.CurrentRow.Cells["Id"].Value);
            if (db.RepondreDemandeConge(id, statut))
            {
                MessageBox.Show($"Demande {statut}.");
                ChargerDemandesAdmin(dgv);
            }
            else
                MessageBox.Show("Erreur lors du traitement.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Utilisateur : charger ses propres demandes
        private void ChargerMesDemandes(DataGridView dgv)
        {
            DataTable dt = db.GetDemandesConge(SessionManager.CurrentUserId);
            dgv.DataSource = dt;
            if (dgv.Columns["Id"] != null) dgv.Columns["Id"].Visible = false;
            if (dgv.Columns["UtilisateurId"] != null) dgv.Columns["UtilisateurId"].Visible = false;
            if (dgv.Columns["Statut"] != null) dgv.Columns["Statut"].HeaderText = "Statut";
            dgv.AutoResizeColumns();
        }


        private void ChargerDemandes(DataGridView dgv)
        {
            DataTable dt;
            if (isAdmin)
                dt = db.GetDemandesConge(0, "en_attente");
            else
                dt = db.GetDemandesConge(SessionManager.CurrentUserId);
            dgv.DataSource = dt;
            if (dgv.Columns["Id"] != null) dgv.Columns["Id"].Visible = false;
            if (dgv.Columns["UtilisateurId"] != null) dgv.Columns["UtilisateurId"].Visible = false;
        }

        

        // ------------------------------------------------------------
        // 4. Page Jours fériés (admin)
        // ------------------------------------------------------------
        private void CreerFeriesPage(TabPage page)
        {
            SplitContainer split = new SplitContainer { Dock = DockStyle.Fill, Orientation = Orientation.Horizontal };
            split.SplitterDistance = 100;
            page.Controls.Add(split);

            Panel top = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };
            DateTimePicker dtpFerie = new DateTimePicker { Format = DateTimePickerFormat.Short, Location = new Point(120, 20), Width = 120 };
            Button btnAjouter = new Button { Text = "Ajouter jour férié", Location = new Point(250, 20), Size = new Size(150, 30) };
            desinemodel.StyleButton(btnAjouter);
            top.Controls.AddRange(new Control[] { new Label { Text = "Date :", Location = new Point(20, 25), AutoSize = true }, dtpFerie, btnAjouter });
            split.Panel1.Controls.Add(top);

            DataGridView dgvFeries = new DataGridView { Dock = DockStyle.Fill, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true };
            desinemodel.StyleDataGridView(dgvFeries);
            split.Panel2.Controls.Add(dgvFeries);
            ChargerFeries(dgvFeries);

            btnAjouter.Click += (s, e) =>
            {
                if (db.AjouterFerie(dtpFerie.Value, "Jour férié"))
                {
                    MessageBox.Show("Jour férié ajouté.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ChargerFeries(dgvFeries);
                }
                else
                {
                    MessageBox.Show("Cette date est déjà un jour férié.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };
        }

        private void ChargerFeries(DataGridView dgv)
        {
            dgv.DataSource = db.GetJoursFeries();
            if (dgv.Columns["Id"] != null) dgv.Columns["Id"].Visible = false;
        }
    }
}
using Gestion_des_Employés.Controler;
using Gestion_des_Employés.models;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Gestion_des_Employés.view
{
    public partial class UserAlerte : UserControl
    {
        private DatabaseHelper db = new DatabaseHelper();
        private string currentFilter = "all";

        public UserAlerte()
        {
            InitializeComponent();
            Load += (s, e) => ApplyDesign();

            // Ces événements ne seront utiles que pour l'admin (les boutons sont masqués pour l'utilisateur)
            BTNNewUtilisateur.Click += (s, e) => { currentFilter = "inscription"; LoadAlertes(); };
            BTNPresenceNotification.Click += (s, e) => { currentFilter = "presence"; LoadAlertes(); };
            BTNNotificationEmployer.Click += (s, e) => { currentFilter = "absence"; LoadAlertes(); };
            if (BTNtousalertes != null)
                BTNtousalertes.Click += (s, e) => { currentFilter = "all"; LoadAlertes(); };
        }

        private void ApplyDesign()
        {
            desinemodel.StyleSectionHeader(TitreAlerteNotification);
            desinemodel.StyleContentPanel(PanelAfficheNotificationClick);

            bool isAdmin = SessionManager.CurrentUserRole == "Admin";

            if (isAdmin)
            {
                // Admin : afficher les boutons de filtre
                desinemodel.StyleActionButton(BTNNewUtilisateur);
                desinemodel.StyleActionButton(BTNPresenceNotification);
                desinemodel.StyleActionButton(BTNNotificationEmployer);
                if (BTNtousalertes != null)
                    desinemodel.StyleActionButton(BTNtousalertes);
                // Rendre les labels badges visibles
                CountNewNotificationUtilisateur.Visible = true;
                CountNotificationPresence.Visible = true;
                CountNotificationEmployer.Visible = true;
                labelcounttousalerte.Visible = true;
            }
            else
            {
                // Utilisateur normal : masquer les filtres et les badges superflus
                BTNNewUtilisateur.Visible = false;
                BTNPresenceNotification.Visible = false;
                BTNNotificationEmployer.Visible = false;
                if (BTNtousalertes != null) BTNtousalertes.Visible = false;
                CountNewNotificationUtilisateur.Visible = false;
                CountNotificationPresence.Visible = false;
                CountNotificationEmployer.Visible = false;
                // Afficher un seul badge total pour les alertes non lues
                labelcounttousalerte.Visible = true;
                labelcounttousalerte.Text = "0";
                desinemodel.StyleBadge(labelcounttousalerte, 0);
                // Optionnel : ajouter un label "Mes alertes"
            }

            UpdateBadges();
            LoadAlertes();
        }

        private void UpdateBadges()
        {
            DataTable dt = GetAlertesData();
            if (dt == null) return;

            if (SessionManager.CurrentUserRole == "Admin")
            {
                int insc = CompterAlertesParType(dt, "inscription");
                int pres = CompterAlertesParType(dt, "presence");
                int abs = CompterAlertesParType(dt, "absence");
                CountNewNotificationUtilisateur.Text = insc.ToString();
                CountNotificationPresence.Text = pres.ToString();
                CountNotificationEmployer.Text = abs.ToString();
                labelcounttousalerte.Text = dt.Rows.Count.ToString();
                desinemodel.StyleBadge(CountNewNotificationUtilisateur, insc);
                desinemodel.StyleBadge(CountNotificationPresence, pres);
                desinemodel.StyleBadge(CountNotificationEmployer, abs);
                desinemodel.StyleBadge(labelcounttousalerte, dt.Rows.Count);
            }
            else
            {
                int total = dt.Rows.Count;
                labelcounttousalerte.Text = total.ToString();
                desinemodel.StyleBadge(labelcounttousalerte, total);
            }
        }

        private DataTable GetAlertesData()
        {
            if (SessionManager.CurrentUserRole == "Admin")
                return db.GetAlertesNonLues(); // toutes les alertes non lues
            else
                return db.GetAlertesByUserId(SessionManager.CurrentUserId); // alertes concernant l'utilisateur
        }

        private int CompterAlertesParType(DataTable dt, string type)
        {
            if (dt == null || dt.Rows.Count == 0) return 0;
            int count = 0;
            foreach (DataRow row in dt.Rows)
            {
                string msg = row["Message"].ToString().ToLower();
                if (type == "inscription" && msg.Contains("inscription")) count++;
                else if (type == "presence" && msg.Contains("présence")) count++;
                else if (type == "absence" && msg.Contains("absence")) count++;
            }
            return count;
        }

        private void LoadAlertes()
        {
            DataTable dt = GetAlertesData();
            PanelAfficheNotificationClick.Controls.Clear();

            if (dt == null || dt.Rows.Count == 0)
            {
                AfficherMessageVide("Aucune alerte pour le moment.");
                return;
            }

            FlowLayoutPanel flow = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, FlowDirection = FlowDirection.TopDown, WrapContents = false };

            foreach (DataRow row in dt.Rows)
            {
                string msg = row["Message"].ToString();
                int alerteId = Convert.ToInt32(row["Id"]);
                string niveau = row["Niveau"].ToString();
                DateTime dateAlerte = Convert.ToDateTime(row["DateAlerte"]);
                string nomUser = row.Table.Columns.Contains("Nom") ? row["Nom"].ToString() : "";

                bool correspond = true;
                if (SessionManager.CurrentUserRole == "Admin")
                {
                    correspond = (currentFilter == "all") ||
                        (currentFilter == "inscription" && msg.ToLower().Contains("inscription")) ||
                        (currentFilter == "presence" && msg.ToLower().Contains("présence")) ||
                        (currentFilter == "absence" && msg.ToLower().Contains("absence"));
                }

                if (correspond)
                {
                    Panel card = CreerCarteAlerte(alerteId, msg, niveau, dateAlerte, nomUser);
                    flow.Controls.Add(card);
                }
            }

            PanelAfficheNotificationClick.Controls.Add(flow);
        }

        private Panel CreerCarteAlerte(int id, string message, string niveau, DateTime date, string nomUser)
        {
            Panel card = new Panel();
            card.Width = PanelAfficheNotificationClick.Width - 30;
            card.Height = 80;
            card.Margin = new Padding(5);
            card.Padding = new Padding(5);
            desinemodel.StylePanel(card);

            Color borderColor = niveau == "danger" ? Color.Red : (niveau == "warning" ? Color.Orange : Color.Gray);
            card.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(borderColor, 2))
                {
                    Rectangle rect = new Rectangle(1, 1, card.Width - 3, card.Height - 3);
                    e.Graphics.DrawRectangle(pen, rect);
                }
            };

            TableLayoutPanel tlp = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 2 };
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

            Label lblNiveau = new Label
            {
                Text = niveau.ToUpper(),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = borderColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                Dock = DockStyle.Fill
            };
            tlp.Controls.Add(lblNiveau, 0, 0);
            tlp.SetRowSpan(lblNiveau, 2);

            Label lblMsg = new Label
            {
                Text = message,
                AutoSize = false,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Black
            };
            tlp.Controls.Add(lblMsg, 1, 0);

            Label lblDate = new Label
            {
                Text = date.ToString("dd/MM/yyyy HH:mm"),
                AutoSize = false,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleLeft
            };
            tlp.Controls.Add(lblDate, 1, 1);

            Button btnMarquer = new Button
            {
                Text = "✓ Lu",
                FlatStyle = FlatStyle.Flat,
                BackColor = desinemodel.SuccessColor,
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                Tag = id,
                Dock = DockStyle.Fill
            };
            btnMarquer.FlatAppearance.BorderSize = 0;
            btnMarquer.Click += (s, e) =>
            {
                db.MarquerAlerteLue(id);
                LoadAlertes();
                UpdateBadges();
            };
            tlp.Controls.Add(btnMarquer, 2, 0);
            tlp.SetRowSpan(btnMarquer, 2);

            card.Controls.Add(tlp);
            return card;
        }

        private void AfficherMessageVide(string texte)
        {
            Label lbl = new Label
            {
                Text = texte,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 12, FontStyle.Italic),
                ForeColor = Color.Gray
            };
            PanelAfficheNotificationClick.Controls.Add(lbl);
        }
    }
}
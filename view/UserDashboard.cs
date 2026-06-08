using Gestion_des_Employés.Controler;
using Gestion_des_Employés.models;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Gestion_des_Employés.view
{
    public partial class UserDashboard : UserControl
    {
        private DatabaseHelper db = new DatabaseHelper();

        public UserDashboard()
        {
            InitializeComponent();
            Load += (s, e) => BuildDashboard();
        }

        private void BuildDashboard()
        {
            panelDashboard.Controls.Clear();
            panelDashboard.Dock = DockStyle.Fill;
            panelDashboard.AutoScroll = true;
            panelDashboard.BackColor = Color.Transparent;

            // --- Cartes (FlowLayoutPanel horizontal) ---
            FlowLayoutPanel flowCards = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 160,
                Padding = new Padding(10),
                BackColor = Color.Transparent,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };

            string[] titres = { "Utilisateurs", "Employés", "Alertes", "Présences" };
            Color[] couleurs = { Color.DeepSkyBlue, Color.NavajoWhite, Color.Red, Color.CornflowerBlue };
            string[] sousTitres = { "Nombre total", "Effectif", "Non lues", "Aujourd'hui" };
            Label[] valeurs = new Label[4];

            for (int i = 0; i < 4; i++)
            {
                Panel card = new Panel { Width = 300, Height = 160, Margin = new Padding(5) };
                card.BackColor = couleurs[i];
                desinemodel.Rounded(card, 15);
                flowCards.Controls.Add(card);

                Label lblTitle = new Label
                {
                    Text = titres[i],
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    ForeColor = Color.White,
                    Location = new Point(10, 10),
                    AutoSize = true
                };
                card.Controls.Add(lblTitle);

                Label lblValue = new Label
                {
                    Text = "0",
                    Font = new Font("Segoe UI", 22, FontStyle.Bold),
                    ForeColor = Color.White,
                    Location = new Point(10, 50),
                    AutoSize = true
                };
                card.Controls.Add(lblValue);
                valeurs[i] = lblValue;

                Label lblSub = new Label
                {
                    Text = sousTitres[i],
                    Font = new Font("Segoe UI", 9),
                    ForeColor = Color.White,
                    Location = new Point(10, 110),
                    AutoSize = true
                };
                card.Controls.Add(lblSub);
            }

            panelDashboard.Controls.Add(flowCards);

            // Charger les valeurs des cartes
            valeurs[0].Text = db.CountUtilisateurs().ToString();
            valeurs[1].Text = db.CountEmployes().ToString();
            valeurs[2].Text = db.CountAllAlertesNonLues().ToString();
            valeurs[3].Text = db.CountPresencesAujourdhui().ToString();
        }

       
    }
}
using Gestion_des_Employés.Controler;
using Gestion_des_Employés.models;
using Gestion_des_Employés.Properties;
using Gestion_des_Employés.view;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Timers;

namespace Gestion_des_Employés
{
    public partial class Dashboard : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int left, int top, int right, int bottom, int width, int height);

        private bool isAssistantVisible = false;
        private int assistantWidth = 320;
        private DatabaseHelper db = new DatabaseHelper();
        private System.Timers.Timer notificationTimer;
        private UserControleIA iaControl;
        private UserDashboard home;

        public Dashboard()
        {
            InitializeComponent();
            Load += Dashboard_Load;
            this.FormClosing += Dashboard_FormClosing;

            
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            ApplyDesign();
            AttachEvents();
            UpdateNotificationBadges();
            StartNotificationTimer();
        }

        private void ApplyDesign()
        {
            // Arrondi de la fenêtre
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 30, 30));

            // Stylisation des contrôles
            desinemodel.StylePanel(paneldashboard);
            desinemodel.MakeCirclePicture(pictureBoxProfile);
            desinemodel.StyleLabel(LabelRole, true);
            desinemodel.StyleSectionTitle(AdminName);
            desinemodel.StyleMenuButton(ButnDashboard);
            desinemodel.StyleMenuButton(BTNUtils);
            desinemodel.StyleMenuButton(BTNEmployer);
            desinemodel.StyleMenuButton(BTNpresence);
            desinemodel.StylePanel(panelNT);
            desinemodel.MakeCirclePicture(PictureNNotifica);
            desinemodel.StyleLabel(Labelwelcom, true);
            desinemodel.StyleSectionTitle(labeltitre);
            desinemodel.StyleLogoutButton(BTNDeconnexion, Resources.logout_20dp_E3E3E3_FILL0_wght400_GRAD0_opsz20);
            desinemodel.StyleLabel(titreApp, true);
            desinemodel.MakeCirclePicture(BTNAssistant);
            desinemodel.StylePanel(PanelMain);
            desinemodel.StylePanel(panelfooter);
            desinemodel.StyleLinkLabel(linkLabelfooter);
            desinemodel.MakeCirclePicture(PictureNOTUserconnecter);

            // Informations de session
            if (SessionManager.IsLoggedIn)
            {
                AdminName.Text = $"{SessionManager.CurrentUserNom} {SessionManager.CurrentUserPrenom}";
                LabelRole.Text = SessionManager.CurrentUserRole;
                if (SessionManager.CurrentUserRole != "Admin")
                {
                    BTNUtils.Visible = false;
                    BTNEmployer.Visible = false;
                }

                // Avatar
                byte[] avatarBytes = db.GetUserAvatar(SessionManager.CurrentUserId);
                if (avatarBytes != null && avatarBytes.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(avatarBytes))
                    {
                        pictureBoxProfile.Image = Image.FromStream(ms);
                        PictureNOTUserconnecter.Image = Image.FromStream(ms);
                    }
                }
                else
                {
                    pictureBoxProfile.Image = Resources.assistant_incon;
                    PictureNOTUserconnecter.Image = Resources.assistant_incon;
                }
            }

            // Ajout unique du contrôle IA dans le panneau assistant
            if (PanelMain.Controls.Count == 0)
            {
                iaControl = new UserControleIA { Dock = DockStyle.Fill };
                PanelMain.Controls.Add(iaControl);
            }

        }

        private void AttachEvents()
        {
            // Navigation principale
            ButnDashboard.Click += (s, e) => LoadView(new UserDashboard());
            BTNUtils.Click += (s, e) => LoadView(new UserUtilisateurs());
            BTNEmployer.Click += (s, e) => LoadView(new UserEmployers());
            BTNpresence.Click += (s, e) => LoadView(new UserPresences());
            BTNAssistant.Click += (s, e) => LoadView(new UserControleIA());
            LoadView(new UserDashboard());

            // Profil et notifications
            PictureNNotifica.Click += (s, e) => LoadView(new UserAlerte());
            PictureNOTUserconnecter.Click += (s, e) => LoadView(new UserProfile());
            pictureBoxProfile.Click += (s, e) => LoadView(new UserProfile());

            // Déconnexion
            BTNDeconnexion.Click += (s, e) =>
            {
                var result = MessageBox.Show("Êtes-vous sûr de vouloir vous déconnecter ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    SessionManager.Logout();
                    this.Close();
                }
            };
        }

        public void LoadView(UserControl view)
        {
            PanelMain.Controls.Clear();
            view.Dock = DockStyle.Fill;
            PanelMain.Controls.Add(view);
        }

        private void UpdateNotificationBadges()
        {
            int nbAlertes;
            if (SessionManager.CurrentUserRole == "Admin")
                nbAlertes = db.CountAllAlertesNonLues();
            else
                nbAlertes = db.CountAlertesNonLues(SessionManager.CurrentUserId);

            countNotification.Text = nbAlertes > 0 ? nbAlertes.ToString() : "0";
            desinemodel.StyleBadge(countNotification, nbAlertes);

            labelCountNotificationIa.Text = "0";
            desinemodel.StyleBadge(labelCountNotificationIa, 0);
        }


        private void StartNotificationTimer()
        {
            notificationTimer = new System.Timers.Timer(30000);
            notificationTimer.Elapsed += (s, e) =>
            {
                if (this.IsDisposed) return;
                if (this.InvokeRequired)
                    this.Invoke(new Action(UpdateNotificationBadges));
                else
                    UpdateNotificationBadges();
            };
            notificationTimer.Start();
        }



        private void AdjustLayout()
        {
            int assistantDelta = isAssistantVisible ? assistantWidth : 0;
            PanelMain.Width = this.ClientSize.Width - paneldashboard.Width - assistantDelta - 20;
            panelNT.Width = this.ClientSize.Width - paneldashboard.Width - 20;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            AdjustLayout();
        }

        private void Dashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (notificationTimer != null)
            {
                notificationTimer.Stop();
                notificationTimer.Dispose();
                notificationTimer = null;
            }
        }
    }
}
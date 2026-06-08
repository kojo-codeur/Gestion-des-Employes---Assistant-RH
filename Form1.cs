using Gestion_des_Employés.models;
using Gestion_des_Employés.view;
using System.Runtime.InteropServices;

namespace Gestion_des_Employés
{
    public partial class Form1 : Form
    {

        // Importation de la fonction CreateRoundRectRgn pour créer des régions arrondies ou de border raduis pour tous nos éléments graphiques
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")] 
        private static extern IntPtr CreateRoundRectRgn(int left, int top, int right, int bottom, int width, int height);

        public Form1()
        {
            InitializeComponent();
            labelconn_Click(null, EventArgs.Empty);

            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 30, 30));

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void labelconn_Click(object sender, EventArgs e)
        {
            UserConnexion connexion = new UserConnexion();

            connexion.Dock = DockStyle.Fill;

            Pconnexioninscription.Controls.Clear();
            Pconnexioninscription.Controls.Add(connexion);
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Userinscription userinscription = new Userinscription();

            userinscription.Dock = DockStyle.Fill;

            Pconnexioninscription.Controls.Clear();
            Pconnexioninscription.Controls.Add(userinscription);
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            
        }
    }
}



using Gestion_des_Employés.models;

namespace Gestion_des_Employés.Controler
{
    public static class SessionManager
    {
        public static int CurrentUserId { get; set; } = -1;
        public static string CurrentUserNom { get; set; } = "";
        public static string CurrentUserPrenom { get; set; } = "";
        public static string CurrentUserEmail { get; set; } = "";
        public static string CurrentUserRole { get; set; } = "";
        public static string CurrentUserStatut { get; set; } = "";
        public static bool IsLoggedIn => CurrentUserId > 0;

        public static void SetUser(Utilisateur user)
        {
            CurrentUserId = user.Id;
            CurrentUserNom = user.Nom;
            CurrentUserPrenom = user.Prenom;
            CurrentUserEmail = user.Email;
            CurrentUserRole = user.Role;
            CurrentUserStatut = user.Statut;
        }

        public static void Logout()
        {
            CurrentUserId = -1;
            CurrentUserNom = "";
            CurrentUserPrenom = "";
            CurrentUserEmail = "";
            CurrentUserRole = "";
            CurrentUserStatut = "";
        }

        public static Utilisateur GetCurrentUser()
        {
            return new Utilisateur
            {
                Id = CurrentUserId,
                Nom = CurrentUserNom,
                Prenom = CurrentUserPrenom,
                Email = CurrentUserEmail,
                Role = CurrentUserRole,
                Statut = CurrentUserStatut
            };
        }
    }
}
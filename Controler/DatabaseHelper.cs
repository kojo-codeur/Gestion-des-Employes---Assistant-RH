using Gestion_des_Employés.models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace Gestion_des_Employés.Controler
{
    public class DatabaseHelper
    {
        private string connectionString;

        #region Classes auxiliaires pour les graphiques
        public class MonthData { public string Month { get; set; } public int Count { get; set; } }
        public class DeptData { public string Departement { get; set; } public int Count { get; set; } }
        public class WeekDayData { public string DayName { get; set; } public int Count { get; set; } }
        public class AlertTypeData { public string Type { get; set; } public int Count { get; set; } }
        #endregion

        #region Constructeur et propriétés
        public string GetConnectionString() => connectionString;

        public DatabaseHelper(string server = "localhost", string database = "rh_management", string uid = "root", string password = "")
        {
            connectionString = $"Server={server};Database={database};Uid={uid};Pwd={password};";
        }
        #endregion

        #region Méthodes privées (hachage, conversion, exécution)
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }

        private Utilisateur DataRowToUtilisateur(DataRow row)
        {
            return new Utilisateur
            {
                Id = Convert.ToInt32(row["Id"]),
                Nom = row["Nom"].ToString(),
                Prenom = row["Prenom"].ToString(),
                Email = row["Email"].ToString(),
                Role = row["Role"].ToString(),
                Sexe = row["Sexe"]?.ToString(),
                Telephone = row["Telephone"]?.ToString(),
                Adresse = row["Adresse"]?.ToString(),
                Departement = row["Departement"]?.ToString(),
                DateEmbauche = row["DateEmbauche"] != DBNull.Value ? Convert.ToDateTime(row["DateEmbauche"]) : (DateTime?)null,
                Salaire = row["Salaire"] != DBNull.Value ? Convert.ToDecimal(row["Salaire"]) : (decimal?)null,
                Statut = row["Statut"].ToString(),
                NbAbsences = Convert.ToInt32(row["NbAbsences"]),
                DateCreation = Convert.ToDateTime(row["DateCreation"]),
                Avatar = row["Avatar"] != DBNull.Value ? (byte[])row["Avatar"] : null
            };
        }

        private DataTable ExecuteQuery(string sql, params MySqlParameter[] parameters)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    return dt;
                }
            }
        }

        private int ExecuteNonQuery(string sql, params MySqlParameter[] parameters)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        private object ExecuteScalar(string sql, params MySqlParameter[] parameters)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteScalar();
                }
            }
        }
        #endregion

        #region Authentification et Avatar
        public Utilisateur Authentifier(string email, string password)
        {
            string hashed = HashPassword(password);
            string sql = "SELECT * FROM Utilisateur WHERE Email = @email AND Password = @pwd";
            var dt = ExecuteQuery(sql,
                new MySqlParameter("@email", email),
                new MySqlParameter("@pwd", hashed));
            if (dt.Rows.Count == 1)
                return DataRowToUtilisateur(dt.Rows[0]);
            return null;
        }

        public bool UpdateUserAvatar(int userId, byte[] avatar)
        {
            string sql = "UPDATE Utilisateur SET Avatar = @avatar WHERE Id = @id";
            return ExecuteNonQuery(sql,
                new MySqlParameter("@avatar", avatar ?? (object)DBNull.Value),
                new MySqlParameter("@id", userId)) > 0;
        }

        public byte[] GetUserAvatar(int userId)
        {
            string sql = "SELECT Avatar FROM Utilisateur WHERE Id = @id";
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", userId);
                    var result = cmd.ExecuteScalar();
                    return result != DBNull.Value ? (byte[])result : null;
                }
            }
        }
        #endregion

        #region Gestion des jours fériés
        public bool EstFerie(DateTime date)
        {
            string sql = "SELECT COUNT(*) FROM JoursFeries WHERE DateFerie = @date";
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        public bool AjouterFerie(DateTime date, string description)
        {
            string checkSql = "SELECT COUNT(*) FROM JoursFeries WHERE DateFerie = @date";
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(checkSql, conn))
                {
                    cmd.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0)
                        return false;
                }
            }

            string sql = "INSERT INTO JoursFeries (DateFerie, Description) VALUES (@date, @desc)";
            return ExecuteNonQuery(sql,
                new MySqlParameter("@date", date.ToString("yyyy-MM-dd")),
                new MySqlParameter("@desc", description)) > 0;
        }

        public bool SupprimerFerie(DateTime date)
        {
            string sql = "DELETE FROM JoursFeries WHERE DateFerie = @date";
            return ExecuteNonQuery(sql, new MySqlParameter("@date", date.ToString("yyyy-MM-dd"))) > 0;
        }

        public DataTable GetJoursFeries()
        {
            return ExecuteQuery("SELECT * FROM JoursFeries ORDER BY DateFerie DESC");
        }
        #endregion

        #region Gestion des utilisateurs (CRUD)
        public Utilisateur GetUtilisateurById(int id)
        {
            string sql = "SELECT * FROM Utilisateur WHERE Id = @id";
            var dt = ExecuteQuery(sql, new MySqlParameter("@id", id));
            return dt.Rows.Count == 1 ? DataRowToUtilisateur(dt.Rows[0]) : null;
        }

        public List<Utilisateur> GetAllUtilisateurs()
        {
            var list = new List<Utilisateur>();
            string sql = "SELECT * FROM Utilisateur ORDER BY Id DESC";
            var dt = ExecuteQuery(sql);
            foreach (DataRow row in dt.Rows)
                list.Add(DataRowToUtilisateur(row));
            return list;
        }

        public List<Utilisateur> GetUtilisateursByStatut(string statut)
        {
            var list = new List<Utilisateur>();
            string sql = "SELECT * FROM Utilisateur WHERE Statut = @statut";
            var dt = ExecuteQuery(sql, new MySqlParameter("@statut", statut));
            foreach (DataRow row in dt.Rows)
                list.Add(DataRowToUtilisateur(row));
            return list;
        }

        public List<Utilisateur> GetUtilisateursEnAttente()
        {
            return GetUtilisateursByStatut("en_attente");
        }

        public List<Utilisateur> GetUtilisateursWithManyAbsences(int seuil = 2)
        {
            var list = new List<Utilisateur>();
            string sql = "SELECT * FROM Utilisateur WHERE NbAbsences > @seuil ORDER BY NbAbsences DESC";
            var dt = ExecuteQuery(sql, new MySqlParameter("@seuil", seuil));
            foreach (DataRow row in dt.Rows)
                list.Add(DataRowToUtilisateur(row));
            return list;
        }

        public bool AddUtilisateur(string nom, string prenom, string email, string password, byte[] avatar = null, string role = "User")
        {
            string sql = "INSERT INTO Utilisateur (Nom, Prenom, Email, Password, Role, Statut, Avatar) VALUES (@nom, @prenom, @email, @pwd, @role, 'en_attente', @avatar)";
            bool inserted = ExecuteNonQuery(sql,
                new MySqlParameter("@nom", nom),
                new MySqlParameter("@prenom", prenom),
                new MySqlParameter("@email", email),
                new MySqlParameter("@pwd", HashPassword(password)),
                new MySqlParameter("@role", role),
                new MySqlParameter("@avatar", avatar ?? (object)DBNull.Value)) > 0;
            if (inserted)
                AddAlerteInscription(email, nom, prenom);
            return inserted;
        }

        public bool ValiderUtilisateur(int userId)
        {
            string sql = "UPDATE Utilisateur SET Statut = 'actif' WHERE Id = @id";
            return ExecuteNonQuery(sql, new MySqlParameter("@id", userId)) > 0;
        }

        public bool UpdateUtilisateur(int id, string nom, string prenom, string email, string telephone, string adresse, string departement, string sexe)
        {
            string sql = "UPDATE Utilisateur SET Nom=@nom, Prenom=@prenom, Email=@email, Telephone=@tel, Adresse=@adresse, Departement=@dept, Sexe=@sexe WHERE Id=@id";
            return ExecuteNonQuery(sql,
                new MySqlParameter("@id", id),
                new MySqlParameter("@nom", nom),
                new MySqlParameter("@prenom", prenom),
                new MySqlParameter("@email", email),
                new MySqlParameter("@tel", telephone ?? (object)DBNull.Value),
                new MySqlParameter("@adresse", adresse ?? (object)DBNull.Value),
                new MySqlParameter("@dept", departement ?? (object)DBNull.Value),
                new MySqlParameter("@sexe", sexe ?? (object)DBNull.Value)) > 0;
        }

        public bool DeleteUtilisateur(int userId)
        {
            string sql = "DELETE FROM Utilisateur WHERE Id = @id";
            return ExecuteNonQuery(sql, new MySqlParameter("@id", userId)) > 0;
        }
        #endregion

        #region Gestion des présences (pointage, absences)
        public bool EnregistrerEntree(int userId, DateTime date, TimeSpan heure)
        {
            TimeSpan heureLimite = new TimeSpan(8, 30, 0);
            string statut = heure <= heureLimite ? "Present" : "Retard";

            string sql = @"INSERT INTO Presence (UtilisateurId, DatePresence, Statut, HeureEntree) 
                           VALUES (@uid, @date, @statut, @heure)
                           ON DUPLICATE KEY UPDATE HeureEntree = @heure, Statut = @statut";
            bool success = ExecuteNonQuery(sql,
                new MySqlParameter("@uid", userId),
                new MySqlParameter("@date", date.ToString("yyyy-MM-dd")),
                new MySqlParameter("@statut", statut),
                new MySqlParameter("@heure", heure)) > 0;

            if (success && statut == "Retard")
                AddAlerte(userId, $"Retard à l'entrée le {date:dd/MM/yyyy} à {heure}", "warning");

            return success;
        }

        public bool EnregistrerSortie(int userId, DateTime date, TimeSpan heure)
        {
            TimeSpan heureNormale = new TimeSpan(17, 30, 0);
            string statutDepart = heure >= heureNormale ? "Present" : "Absent";

            string sql = @"UPDATE Presence 
                           SET HeureSortie = @heure, StatutDepart = @statutDepart
                           WHERE UtilisateurId = @uid AND DatePresence = @date";
            int rows = ExecuteNonQuery(sql,
                new MySqlParameter("@uid", userId),
                new MySqlParameter("@date", date.ToString("yyyy-MM-dd")),
                new MySqlParameter("@heure", heure),
                new MySqlParameter("@statutDepart", statutDepart));

            if (rows == 0) return false;

            if (statutDepart == "Absent")
            {
                bool absence = AddPresence(userId, date, "Absent", "Sortie anticipée");
                if (absence)
                    AddAlerte(userId, $"Départ anticipé le {date:dd/MM/yyyy} à {heure} (avant 17h30)", "warning");
                return absence;
            }
            return true;
        }

        public bool ADejaPointéEntree(int userId, DateTime date)
        {
            string sql = "SELECT COUNT(*) FROM Presence WHERE UtilisateurId = @uid AND DatePresence = @date AND HeureEntree IS NOT NULL";
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@uid", userId);
                    cmd.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        public bool ADejaPointéSortie(int userId, DateTime date)
        {
            string sql = "SELECT COUNT(*) FROM Presence WHERE UtilisateurId = @uid AND DatePresence = @date AND HeureSortie IS NOT NULL";
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@uid", userId);
                    cmd.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        public DataRow GetPresenceDuJour(int userId, DateTime date)
        {
            string sql = "SELECT * FROM Presence WHERE UtilisateurId = @uid AND DatePresence = @date";
            DataTable dt = ExecuteQuery(sql,
                new MySqlParameter("@uid", userId),
                new MySqlParameter("@date", date.ToString("yyyy-MM-dd")));
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        public bool AddPresence(int userId, DateTime date, string statut, string justification = "")
        {
            string check = "SELECT COUNT(*) FROM Presence WHERE UtilisateurId = @uid AND DatePresence = @date";
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(check, conn))
                {
                    cmd.Parameters.AddWithValue("@uid", userId);
                    cmd.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
                    int exists = Convert.ToInt32(cmd.ExecuteScalar());
                    if (exists > 0 && statut == "Absent")
                        return false;
                }
            }

            string sql = "INSERT INTO Presence (UtilisateurId, DatePresence, Statut, Justification) VALUES (@uid, @date, @statut, @justif)";
            bool success = ExecuteNonQuery(sql,
                new MySqlParameter("@uid", userId),
                new MySqlParameter("@date", date.ToString("yyyy-MM-dd")),
                new MySqlParameter("@statut", statut),
                new MySqlParameter("@justif", justification)) > 0;
            if (success && statut == "Absent")
            {
                IncrementAbsences(userId);
                int nb = GetNbAbsences(userId);
                if (nb >= 7)
                    AddAlerteAbsence(userId, nb, "danger");
                else if (nb >= 3)
                    AddAlerteAbsence(userId, nb, "warning");
            }
            return success;
        }

        public DataTable GetPresencesByUser(int userId)
        {
            string sql = "SELECT Id, DatePresence, Statut, HeureEntree, HeureSortie, StatutDepart, Justification FROM Presence WHERE UtilisateurId = @uid ORDER BY DatePresence DESC";
            return ExecuteQuery(sql, new MySqlParameter("@uid", userId));
        }

        public DataTable GetAllPresences()
        {
            string sql = "SELECT p.Id, u.Nom, u.Prenom, p.DatePresence, p.Statut, p.HeureEntree, p.HeureSortie, p.StatutDepart FROM Presence p JOIN Utilisateur u ON p.UtilisateurId = u.Id ORDER BY p.DatePresence DESC";
            return ExecuteQuery(sql);
        }

        private void IncrementAbsences(int userId)
        {
            string sql = "UPDATE Utilisateur SET NbAbsences = NbAbsences + 1 WHERE Id = @id";
            ExecuteNonQuery(sql, new MySqlParameter("@id", userId));
        }

        public int GetNbAbsences(int userId)
        {
            string sql = "SELECT NbAbsences FROM Utilisateur WHERE Id = @id";
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", userId);
                    var result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }

        public bool JustifierAbsence(int userId, DateTime date, string justification)
        {
            string sql = @"INSERT INTO Presence (UtilisateurId, DatePresence, Statut, Justification) 
                           VALUES (@uid, @date, 'Absent', @justif)
                           ON DUPLICATE KEY UPDATE Justification = @justif";
            return ExecuteNonQuery(sql,
                new MySqlParameter("@uid", userId),
                new MySqlParameter("@date", date.ToString("yyyy-MM-dd")),
                new MySqlParameter("@justif", justification)) > 0;
        }

        public bool DeletePresence(int presenceId)
        {
            string sql = "DELETE FROM Presence WHERE Id = @id";
            return ExecuteNonQuery(sql, new MySqlParameter("@id", presenceId)) > 0;
        }

        public int GetPresenceId(int userId, DateTime date)
        {
            string sql = "SELECT Id FROM Presence WHERE UtilisateurId = @uid AND DatePresence = @date";
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@uid", userId);
                    cmd.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
                    var result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }
        #endregion

        #region Gestion des alertes
        public bool AddAlerte(int userId, string message, string niveau)
        {
            string validNiveau = (niveau == "info" || niveau == "warning" || niveau == "danger") ? niveau : "info";
            string sql = "INSERT INTO Alerte (UtilisateurId, Message, Niveau) VALUES (@uid, @msg, @niveau)";
            return ExecuteNonQuery(sql,
                new MySqlParameter("@uid", userId),
                new MySqlParameter("@msg", message),
                new MySqlParameter("@niveau", validNiveau)) > 0;
        }

        public void AddAlerteInscription(string email, string nom, string prenom)
        {
            string message = $"Nouvelle inscription : {nom} {prenom} ({email}) en attente de validation.";
            AddAlerte(1, message, "info");
        }

        public void AddAlerteAbsence(int userId, int nbAbsences, string niveau)
        {
            string message = $"L'utilisateur {userId} a {nbAbsences} absence(s). Seuil {niveau}.";
            AddAlerte(userId, message, niveau);
        }

        public int EnvoyerAlerteAbsencesMassive(int seuil = 2, string niveau = "warning")
        {
            var users = GetUtilisateursWithManyAbsences(seuil);
            int count = 0;
            foreach (var u in users)
            {
                string message = $"⚠️ Vous avez {u.NbAbsences} absence(s). Veuillez justifier votre situation auprès des RH.";
                if (AddAlerte(u.Id, message, niveau))
                    count++;
            }
            return count;
        }

        public DataTable GetAlertesNonLues()
        {
            string sql = "SELECT a.Id, u.Nom, u.Prenom, a.Message, a.Niveau, a.DateAlerte FROM Alerte a JOIN Utilisateur u ON a.UtilisateurId = u.Id WHERE a.EstLu = 0 ORDER BY a.DateAlerte DESC";
            return ExecuteQuery(sql);
        }

        public DataTable GetAlertesByUserId(int userId)
        {
            string sql = "SELECT Id, Message, Niveau, DateAlerte, EstLu FROM Alerte WHERE UtilisateurId = @uid ORDER BY DateAlerte DESC";
            return ExecuteQuery(sql, new MySqlParameter("@uid", userId));
        }

        public void MarquerAlerteLue(int alerteId)
        {
            string sql = "UPDATE Alerte SET EstLu = 1 WHERE Id = @id";
            ExecuteNonQuery(sql, new MySqlParameter("@id", alerteId));
        }

        public bool DeleteAlerte(int alerteId)
        {
            string sql = "DELETE FROM Alerte WHERE Id = @id";
            return ExecuteNonQuery(sql, new MySqlParameter("@id", alerteId)) > 0;
        }
        #endregion

        #region Messages IA
        public bool SaveMessageIA(int userId, string userMessage, string iaMessage)
        {
            string sql = "INSERT INTO MessageIA (UtilisateurId, MessageUser, MessageIA) VALUES (@uid, @user, @ia)";
            return ExecuteNonQuery(sql,
                new MySqlParameter("@uid", userId),
                new MySqlParameter("@user", userMessage),
                new MySqlParameter("@ia", iaMessage)) > 0;
        }

        public DataTable GetMessagesIA(int userId)
        {
            string sql = "SELECT MessageUser, MessageIA, DateMessage FROM MessageIA WHERE UtilisateurId = @uid ORDER BY DateMessage ASC";
            return ExecuteQuery(sql, new MySqlParameter("@uid", userId));
        }
        #endregion

        #region Gestion des congés
        public bool AjouterDemandeConge(int userId, DateTime debut, DateTime fin, string raison)
        {
            string sql = "INSERT INTO DemandeConge (UtilisateurId, DateDebut, DateFin, Raison) VALUES (@uid, @debut, @fin, @raison)";
            return ExecuteNonQuery(sql,
                new MySqlParameter("@uid", userId),
                new MySqlParameter("@debut", debut.ToString("yyyy-MM-dd")),
                new MySqlParameter("@fin", fin.ToString("yyyy-MM-dd")),
                new MySqlParameter("@raison", raison)) > 0;
        }

        public DataTable GetDemandesConge(int userId = 0, string statut = null)
        {
            string sql = "SELECT d.*, u.Nom, u.Prenom FROM DemandeConge d JOIN Utilisateur u ON d.UtilisateurId = u.Id";
            List<string> conditions = new List<string>();
            if (userId > 0) conditions.Add("d.UtilisateurId = @uid");
            if (!string.IsNullOrEmpty(statut)) conditions.Add("d.Statut = @statut");
            if (conditions.Count > 0) sql += " WHERE " + string.Join(" AND ", conditions);
            sql += " ORDER BY d.DateDemande DESC";
            var parameters = new List<MySqlParameter>();
            if (userId > 0) parameters.Add(new MySqlParameter("@uid", userId));
            if (!string.IsNullOrEmpty(statut)) parameters.Add(new MySqlParameter("@statut", statut));
            return ExecuteQuery(sql, parameters.ToArray());
        }

        public bool RepondreDemandeConge(int demandeId, string nouveauStatut)
        {
            string sql = "UPDATE DemandeConge SET Statut = @statut WHERE Id = @id";
            bool ok = ExecuteNonQuery(sql,
                new MySqlParameter("@statut", nouveauStatut),
                new MySqlParameter("@id", demandeId)) > 0;
            if (ok)
            {
                DataTable dt = ExecuteQuery("SELECT UtilisateurId FROM DemandeConge WHERE Id = @id", new MySqlParameter("@id", demandeId));
                if (dt.Rows.Count > 0)
                {
                    int userId = Convert.ToInt32(dt.Rows[0]["UtilisateurId"]);
                    string message = $"Votre demande de congé a été {nouveauStatut}.";
                    AddAlerte(userId, message, nouveauStatut == "accepte" ? "success" : "warning");
                }
            }
            return ok;
        }
        #endregion

        #region Statistiques
        public int CountUtilisateurs()
        {
            string sql = "SELECT COUNT(*) FROM Utilisateur";
            return Convert.ToInt32(ExecuteScalar(sql));
        }

        public int CountEmployes()
        {
            string sql = "SELECT COUNT(*) FROM Utilisateur WHERE Role = 'User'";
            return Convert.ToInt32(ExecuteScalar(sql));
        }

        public int CountAlertesNonLues(int userId)
        {
            string sql = "SELECT COUNT(*) FROM Alerte WHERE EstLu = 0 AND UtilisateurId = @uid";
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@uid", userId);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public int CountAllAlertesNonLues()
        {
            string sql = "SELECT COUNT(*) FROM Alerte WHERE EstLu = 0";
            return Convert.ToInt32(ExecuteScalar(sql));
        }

        public int CountPresencesAujourdhui()
        {
            string sql = "SELECT COUNT(*) FROM Presence WHERE DatePresence = CURDATE() AND Statut = 'Present'";
            return Convert.ToInt32(ExecuteScalar(sql));
        }
        #endregion

        #region Graphiques
        public List<MonthData> GetUserRegistrationsByMonth(int months)
        {
            var list = new List<MonthData>();
            string sql = @"
                SELECT DATE_FORMAT(DateCreation, '%b %Y') as Month, COUNT(*) as Count
                FROM Utilisateur
                WHERE DateCreation >= DATE_SUB(NOW(), INTERVAL @months MONTH)
                GROUP BY YEAR(DateCreation), MONTH(DateCreation)
                ORDER BY YEAR(DateCreation), MONTH(DateCreation)";
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@months", months);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            list.Add(new MonthData { Month = reader["Month"].ToString(), Count = Convert.ToInt32(reader["Count"]) });
                    }
                }
            }
            return list;
        }

        public List<DeptData> GetEmployesByDepartement()
        {
            var list = new List<DeptData>();
            string sql = "SELECT Departement, COUNT(*) as Count FROM Utilisateur WHERE Role = 'User' AND Departement IS NOT NULL GROUP BY Departement";
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        list.Add(new DeptData { Departement = reader["Departement"].ToString(), Count = Convert.ToInt32(reader["Count"]) });
                }
            }
            if (list.Count == 0) list.Add(new DeptData { Departement = "Aucun", Count = 1 });
            return list;
        }

        public List<WeekDayData> GetPresencesByWeekDay()
        {
            var list = new List<WeekDayData>();
            string sql = @"
                SELECT DAYNAME(DatePresence) as DayName, COUNT(*) as Count
                FROM Presence
                WHERE Statut = 'Present'
                GROUP BY DAYOFWEEK(DatePresence), DAYNAME(DatePresence)
                ORDER BY DAYOFWEEK(DatePresence)";
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        list.Add(new WeekDayData { DayName = reader["DayName"].ToString(), Count = Convert.ToInt32(reader["Count"]) });
                }
            }
            if (list.Count == 0)
            {
                var defaults = new[] { "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi" };
                foreach (var d in defaults) list.Add(new WeekDayData { DayName = d, Count = 0 });
            }
            return list;
        }

        public List<AlertTypeData> GetAlertesByType()
        {
            var list = new List<AlertTypeData>();
            string sql = @"
                SELECT 
                    CASE 
                        WHEN Message LIKE '%absence%' THEN 'Absence'
                        WHEN Message LIKE '%retard%' THEN 'Retard'
                        ELSE 'Autre'
                    END as Type,
                    COUNT(*) as Count
                FROM Alerte
                GROUP BY Type";
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        list.Add(new AlertTypeData { Type = reader["Type"].ToString(), Count = Convert.ToInt32(reader["Count"]) });
                }
            }
            if (list.Count == 0) list.Add(new AlertTypeData { Type = "Aucune alerte", Count = 1 });
            return list;
        }
        #endregion
    }
}
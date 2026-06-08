using Gestion_des_Employés.Controler;
using Gestion_des_Employés.models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gestion_des_Employés.view
{
    public partial class UserControleIA : UserControl
    {
        #region Composants UI
        private FlowLayoutPanel flowMessages;
        private RichTextBox txtInput;
        private Button btnSend;
        private Panel inputPanel;
        private Panel headerPanel;
        private Label lblTitle;
        private Label lblStatus;
        private bool isTyping = false;
        #endregion

        #region Constantes API
        private const string GROQ_API_KEY = "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";
        private const string GROQ_API_URL = "https://api.groq.com/openai/v1/chat/completions";
        private readonly HttpClient httpClient = new HttpClient();
        #endregion

        #region Base de données et historique conversation
        private DatabaseHelper db = new DatabaseHelper();
        private List<Dictionary<string, string>> conversationHistory = new List<Dictionary<string, string>>();
        private const int MAX_HISTORY = 10; // nombre de messages à conserver
        #endregion

        #region Constructeur et initialisation
        public UserControleIA()
        {
            InitializeComponent();
            SetupUI();
            SetupEvents();
            LoadConversationHistory();
        }

        private void LoadConversationHistory()
        {
            if (SessionManager.CurrentUserId <= 0) return;
            var dt = db.GetMessagesIA(SessionManager.CurrentUserId);
            foreach (DataRow row in dt.Rows)
            {
                string userMsg = row["MessageUser"].ToString();
                string iaMsg = row["MessageIA"].ToString();
                AddMessage(userMsg, true);
                AddMessage(iaMsg, false);
                // Ajouter à l'historique mémoire
                conversationHistory.Add(new Dictionary<string, string> { { "role", "user" }, { "content", userMsg } });
                conversationHistory.Add(new Dictionary<string, string> { { "role", "assistant" }, { "content", iaMsg } });
            }
            // Garder seulement les derniers MAX_HISTORY messages
            if (conversationHistory.Count > MAX_HISTORY * 2)
                conversationHistory = conversationHistory.Skip(conversationHistory.Count - MAX_HISTORY * 2).ToList();
        }
        #endregion

        #region Configuration de l'interface
        private void SetupUI()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Padding = new Padding(0);

            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.White,
                Padding = new Padding(10)
            };
            headerPanel.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(200, 200, 200), 1))
                    e.Graphics.DrawLine(pen, 0, headerPanel.Height - 1, headerPanel.Width, headerPanel.Height - 1);
            };

            lblTitle = new Label
            {
                Text = "Assistant RH",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(15, 15),
                AutoSize = true
            };
            lblStatus = new Label
            {
                Text = "En ligne",
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.Green,
                Location = new Point(15, 38),
                AutoSize = true
            };
            headerPanel.Controls.Add(lblTitle);
            headerPanel.Controls.Add(lblStatus);

            flowMessages = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(10),
                BackColor = Color.White
            };

            inputPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 70,
                Padding = new Padding(8),
                BackColor = Color.White
            };
            txtInput = new RichTextBox
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 9),
                Multiline = true,
                Height = 50
            };
            btnSend = new Button
            {
                Text = "Envoyer",
                Dock = DockStyle.Right,
                Width = 70,
                Height = 50,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnSend.FlatAppearance.BorderSize = 0;
            inputPanel.Controls.Add(txtInput);
            inputPanel.Controls.Add(btnSend);

            this.Controls.Add(flowMessages);
            this.Controls.Add(inputPanel);
            this.Controls.Add(headerPanel);
        }

        private void SetupEvents()
        {
            btnSend.Click += async (s, e) => await SendMessageAsync();
            txtInput.KeyDown += async (s, e) =>
            {
                if (e.Control && e.KeyCode == Keys.Enter)
                {
                    await SendMessageAsync();
                    e.SuppressKeyPress = true;
                }
            };
        }
        #endregion

        #region Envoi et affichage des messages
        private async Task SendMessageAsync()
        {
            string userMessage = txtInput.Text.Trim();
            if (string.IsNullOrEmpty(userMessage)) return;

            AddMessage(userMessage, true);
            txtInput.Clear();
            EnableInput(false);
            ShowTypingIndicator(true);

            // Exécuter l'action locale (lecture/écriture base)
            string actionResult = await ExecuteLocalAction(userMessage);

            // Obtenir la réponse de l'IA avec contexte
            string iaResponse = await GetAIResponse(userMessage, actionResult);

            // Sauvegarde dans la base
            if (SessionManager.CurrentUserId > 0)
                db.SaveMessageIA(SessionManager.CurrentUserId, userMessage, iaResponse);

            // Mise à jour de l'historique mémoire
            conversationHistory.Add(new Dictionary<string, string> { { "role", "user" }, { "content", userMessage } });
            conversationHistory.Add(new Dictionary<string, string> { { "role", "assistant" }, { "content", iaResponse } });
            if (conversationHistory.Count > MAX_HISTORY * 2)
                conversationHistory = conversationHistory.Skip(conversationHistory.Count - MAX_HISTORY * 2).ToList();

            ShowTypingIndicator(false);
            AddMessage(iaResponse, false);
            EnableInput(true);
            txtInput.Focus();
        }

        private void EnableInput(bool enabled)
        {
            txtInput.Enabled = enabled;
            btnSend.Enabled = enabled;
        }

        private void AddMessage(string text, bool isUser)
        {
            Label msgLabel = new Label
            {
                Text = text,
                AutoSize = false,
                MaximumSize = new Size(flowMessages.Width - 40, int.MaxValue),
                Padding = new Padding(8),
                Margin = new Padding(5),
                Font = new Font("Segoe UI", 12),
                BackColor = isUser ? Color.FromArgb(0, 120, 215) : Color.FromArgb(240, 240, 240),
                ForeColor = isUser ? Color.White : Color.Black,
                TextAlign = isUser ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft
            };
            msgLabel.Paint += (s, e) =>
            {
                var rect = new Rectangle(0, 0, msgLabel.Width - 1, msgLabel.Height - 1);
                using (var path = GetRoundedRect(rect, 12))
                    msgLabel.Region = new Region(path);
            };
            int textHeight = TextRenderer.MeasureText(text, msgLabel.Font, new Size(msgLabel.MaximumSize.Width, int.MaxValue), TextFormatFlags.WordBreak).Height;
            msgLabel.Height = textHeight + 16;
            msgLabel.Width = Math.Min(msgLabel.MaximumSize.Width, TextRenderer.MeasureText(text, msgLabel.Font).Width + 40);
            flowMessages.Controls.Add(msgLabel);
            flowMessages.ScrollControlIntoView(msgLabel);
        }

        private GraphicsPath GetRoundedRect(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void ShowTypingIndicator(bool show)
        {
            if (show && !isTyping)
            {
                isTyping = true;
                Label typingLabel = new Label
                {
                    Text = "L'assistant écrit...",
                    ForeColor = Color.Gray,
                    Font = new Font("Segoe UI", 12, FontStyle.Italic),
                    AutoSize = true,
                    Name = "typingIndicator"
                };
                flowMessages.Controls.Add(typingLabel);
                flowMessages.ScrollControlIntoView(typingLabel);
            }
            else if (!show && isTyping)
            {
                isTyping = false;
                var typing = flowMessages.Controls.Find("typingIndicator", false);
                if (typing.Length > 0) flowMessages.Controls.Remove(typing[0]);
            }
        }
        #endregion

        #region Actions locales (lecture base et CRUD alertes) - Sécurisé par rôle
        private async Task<string> ExecuteLocalAction(string message)
        {
            string lower = message.ToLower();
            int currentUserId = SessionManager.CurrentUserId;
            if (currentUserId <= 0) return "Vous n'êtes pas connecté.";

            bool isAdmin = SessionManager.CurrentUserRole == "Admin";

            // --- Actions personnelles (tous utilisateurs) ---
            if (lower.Contains("présent") || lower.Contains("present") || (lower.Contains("signaler") && lower.Contains("présence")))
            {
                bool success = db.AddPresence(currentUserId, DateTime.Today, "Present");
                return success ? "✅ Votre présence a été enregistrée pour aujourd'hui." : "❌ Erreur (peut-être déjà enregistré).";
            }
            if (lower.Contains("absent") || (lower.Contains("signaler") && lower.Contains("absence")))
            {
                string justification = "";
                if (lower.Contains("maladie")) justification = "Maladie";
                else if (lower.Contains("congé")) justification = "Congé";
                else justification = "Non justifié";
                bool success = db.AddPresence(currentUserId, DateTime.Today, "Absent", justification);
                return success ? $"⚠️ Votre absence a été signalée (justification: {justification})." : "❌ Impossible d'enregistrer l'absence.";
            }

            if (lower.Contains("mes présences") || lower.Contains("mon historique présence"))
            {
                var dt = db.GetPresencesByUser(currentUserId);
                if (dt.Rows.Count == 0) return "Aucune présence enregistrée.";
                string result = "📋 Historique des présences :\n";
                foreach (DataRow row in dt.Rows)
                {
                    result += $"- {Convert.ToDateTime(row["DatePresence"]):dd/MM/yyyy} : {row["Statut"]}";
                    if (!string.IsNullOrEmpty(row["Justification"]?.ToString()))
                        result += $" ({row["Justification"]})";
                    result += "\n";
                }
                return result;
            }
            if (lower.Contains("nombre d'absences") || lower.Contains("nb absences"))
            {
                int nb = db.GetNbAbsences(currentUserId);
                return $"📊 Vous avez {nb} absence(s) enregistrée(s).";
            }

            // --- Recherche d'un employé (par email ou nom) - accessible à tous (lecture seule) ---
            if (lower.Contains("recherche") || lower.Contains("trouver") || lower.Contains("chercher"))
            {
                string query = ExtractEmailFromMessage(message);
                if (string.IsNullOrEmpty(query))
                {
                    // essayer d'extraire un nom (deux mots)
                    var words = message.Split(' ');
                    if (words.Length >= 2)
                        query = words[words.Length - 2] + " " + words[words.Length - 1];
                    else if (words.Length == 1)
                        query = words[0];
                }
                if (string.IsNullOrEmpty(query))
                    return "Veuillez donner un email ou un nom à rechercher.";

                var all = db.GetAllUtilisateurs();
                var found = all.Find(u => u.Email.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                                          u.Nom.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                                          u.Prenom.Contains(query, StringComparison.OrdinalIgnoreCase));
                if (found == null)
                    return $"Aucun utilisateur trouvé avec '{query}'.";
                return $"👤 Employé trouvé : {found.NomComplet} ({found.Email}) - Département : {found.Departement ?? "Non défini"} - Absences : {found.NbAbsences}";
            }

            // --- Lecture de données globales (réservée admin) ---
            if (lower.Contains("liste des utilisateurs") || lower.Contains("tous les utilisateurs"))
            {
                if (!isAdmin) return "⛔ Accès refusé. Seul l'administrateur peut voir la liste de tous les utilisateurs.";
                var users = db.GetAllUtilisateurs();
                if (users.Count == 0) return "Aucun utilisateur.";
                string result = "👥 Liste des utilisateurs :\n";
                foreach (var u in users)
                    result += $"- {u.NomComplet} ({u.Email}) - {u.Role} - {u.Statut}\n";
                return result;
            }

            if (lower.Contains("liste des employés") || lower.Contains("tous les employés"))
            {
                if (!isAdmin) return "⛔ Accès refusé. Seul l'administrateur peut voir la liste de tous les employés.";
                var employes = db.GetAllUtilisateurs().Where(u => u.Role == "User").ToList();
                if (employes.Count == 0) return "Aucun employé.";
                string result = "🧑‍💼 Liste des employés :\n";
                foreach (var e in employes)
                    result += $"- {e.NomComplet} ({e.Email}) - {e.NbAbsences} absence(s)\n";
                return result;
            }

            if (lower.Contains("liste des alertes") || lower.Contains("alertes non lues"))
            {
                DataTable dt;
                if (isAdmin)
                    dt = db.GetAlertesNonLues();
                else
                    dt = db.GetAlertesByUserId(currentUserId);
                if (dt.Rows.Count == 0) return "Aucune alerte non lue.";
                string result = "🔔 Alertes non lues :\n";
                foreach (DataRow row in dt.Rows)
                    result += $"- {row["DateAlerte"]:dd/MM/yyyy HH:mm} : {row["Message"]} (Niveau {row["Niveau"]})\n";
                return result;
            }

            if (lower.Contains("statistiques") || lower.Contains("résumé") || lower.Contains("combien"))
            {
                if (!isAdmin) return "⛔ Seul l'administrateur peut consulter les statistiques globales.";
                int nbUsers = db.CountUtilisateurs();
                int nbEmployes = db.CountEmployes();
                int nbAlertes = db.CountAllAlertesNonLues();
                int nbPresences = db.CountPresencesAujourdhui();
                return $"📊 Statistiques générales :\n- Utilisateurs : {nbUsers}\n- Employés : {nbEmployes}\n- Alertes non lues : {nbAlertes}\n- Présences aujourd'hui : {nbPresences}";
            }

            // --- CRUD sur Alertes (créer, marquer lue) - autorisé à tous pour eux-mêmes ou admin pour tous ---
            if (lower.Contains("ajouter une alerte") || (lower.Contains("créer") && lower.Contains("alerte")))
            {
                string targetEmail = ExtractEmailFromMessage(message);
                if (string.IsNullOrEmpty(targetEmail))
                    return "❌ Je n'ai pas compris pour qui créer l'alerte. Précisez l'email.";
                string alerteMessage = message.Replace("ajouter une alerte", "").Replace("créer alerte", "").Trim();
                if (string.IsNullOrEmpty(alerteMessage))
                    alerteMessage = "Alerte générée par l'assistant.";
                var allUsers = db.GetAllUtilisateurs();
                var cible = allUsers.Find(u => u.Email.Equals(targetEmail, StringComparison.OrdinalIgnoreCase));
                if (cible == null) return $"Utilisateur avec l'email {targetEmail} introuvable.";
                // Seul l'admin peut créer une alerte pour un autre ; un user peut créer pour lui-même
                if (!isAdmin && cible.Id != currentUserId)
                    return "⛔ Vous ne pouvez créer une alerte que pour vous-même.";
                db.AddAlerte(cible.Id, alerteMessage, "info");
                return $"✅ Alerte ajoutée pour {cible.NomComplet}.";
            }

            if (lower.Contains("marquer alerte lue") || lower.Contains("alerte lue"))
            {
                int id = ExtractIdFromMessage(message);
                if (id <= 0) return "Veuillez préciser le numéro de l'alerte (ex: 'marquer alerte 5 lue').";
                // Vérifier que l'alerte appartient bien à l'utilisateur ou admin
                var dt = db.GetAlertesByUserId(currentUserId);
                bool found = false;
                foreach (DataRow row in dt.Rows)
                    if (Convert.ToInt32(row["Id"]) == id) { found = true; break; }
                if (!found && !isAdmin)
                    return "⛔ Vous ne pouvez marquer que vos propres alertes.";
                db.MarquerAlerteLue(id);
                return $"✅ Alerte {id} marquée comme lue.";
            }

            // --- Actions ADMIN seulement (écriture / suppression) ---
            if (!isAdmin) return null; // l'IA répondra sans action spécifique

            // Inscriptions en attente
            if (lower.Contains("inscriptions en attente") || lower.Contains("nouveaux employés") || lower.Contains("en attente de validation"))
            {
                var enAttente = db.GetUtilisateursEnAttente();
                if (enAttente.Count == 0)
                    return "Aucune inscription en attente de validation.";
                string result = "📝 **Inscriptions en attente** :\n";
                foreach (var u in enAttente)
                    result += $"- {u.Nom} {u.Prenom} ({u.Email}) - Date : {u.DateCreation:dd/MM/yyyy}\n";
                result += "\nPour valider, dites : 'valider l'utilisateur [email]'";
                return result;
            }

            // Valider un utilisateur
            if (lower.Contains("valider l'utilisateur") || lower.Contains("valider l'inscription") || lower.Contains("valider employé"))
            {
                string email = ExtractEmailFromMessage(message);
                if (string.IsNullOrEmpty(email))
                    return "❌ Veuillez préciser l'email de l'utilisateur à valider.";
                var users = db.GetAllUtilisateurs();
                var target = users.Find(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
                if (target == null) return $"Utilisateur {email} introuvable.";
                if (target.Statut == "actif") return $"L'utilisateur {target.NomComplet} est déjà actif.";
                bool ok = db.ValiderUtilisateur(target.Id);
                if (ok)
                {
                    db.AddAlerte(target.Id, "✅ Votre inscription a été validée par l'administrateur. Bienvenue !", "success");
                    return $"✅ L'utilisateur {target.NomComplet} a été validé et est maintenant actif.";
                }
                return "❌ Erreur lors de la validation.";
            }

            // Supprimer un utilisateur
            if (lower.Contains("supprimer l'utilisateur") || lower.Contains("effacer l'utilisateur"))
            {
                string email = ExtractEmailFromMessage(message);
                if (string.IsNullOrEmpty(email))
                    return "❌ Veuillez préciser l'email de l'utilisateur à supprimer.";
                var users = db.GetAllUtilisateurs();
                var target = users.Find(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
                if (target == null) return $"Utilisateur {email} introuvable.";
                if (target.Role == "Admin") return "❌ Impossible de supprimer un administrateur via l'assistant.";
                bool ok = db.DeleteUtilisateur(target.Id);
                if (ok)
                {
                    db.AddAlerte(1, $"L'utilisateur {target.NomComplet} ({email}) a été supprimé par l'assistant.", "warning");
                    return $"🗑️ Utilisateur {target.NomComplet} supprimé définitivement.";
                }
                return "❌ Erreur lors de la suppression.";
            }

            // Supprimer une alerte
            if (lower.Contains("supprimer l'alerte") || lower.Contains("effacer l'alerte"))
            {
                int id = ExtractIdFromMessage(message);
                if (id <= 0) return "❌ Veuillez donner l'ID de l'alerte.";
                bool ok = db.DeleteAlerte(id);
                return ok ? $"🗑️ Alerte {id} supprimée." : $"❌ Alerte {id} introuvable.";
            }

            // Supprimer une présence
            if (lower.Contains("supprimer la présence") || lower.Contains("effacer la présence"))
            {
                string email = ExtractEmailFromMessage(message);
                if (string.IsNullOrEmpty(email)) return "❌ Veuillez préciser l'email de l'utilisateur.";
                DateTime date = ExtractDateFromMessage(message);
                if (date == DateTime.MinValue) return "❌ Veuillez préciser la date (AAAA-MM-JJ).";
                var users = db.GetAllUtilisateurs();
                var target = users.Find(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
                if (target == null) return $"Utilisateur {email} introuvable.";
                int presenceId = db.GetPresenceId(target.Id, date);
                if (presenceId == 0) return $"Aucune présence trouvée pour {target.NomComplet} le {date:dd/MM/yyyy}.";
                bool ok = db.DeletePresence(presenceId);
                return ok ? $"🗑️ Présence du {date:dd/MM/yyyy} supprimée." : "❌ Erreur lors de la suppression.";
            }

            // Notification massive d'absences
            if (lower.Contains("notifier") && lower.Contains("absence"))
            {
                int seuil = ExtractSeuilFromMessage(message);
                int count = db.EnvoyerAlerteAbsencesMassive(seuil, "warning");
                return $"📢 {count} alerte(s) envoyée(s) aux utilisateurs avec plus de {seuil} absences.";
            }

            return null;
        }
        #endregion

        #region Utilitaires d'extraction
        private string ExtractEmailFromMessage(string msg)
        {
            var words = msg.Split(' ');
            foreach (var w in words)
            {
                string cleaned = w.Trim().Trim(',', '.', ';', '!', '?');
                if (cleaned.Contains("@") && cleaned.Contains("."))
                    return cleaned;
            }
            return null;
        }

        private int ExtractIdFromMessage(string msg)
        {
            var words = msg.Split(' ');
            foreach (var w in words)
                if (int.TryParse(w, out int id))
                    return id;
            return 0;
        }

        private int ExtractSeuilFromMessage(string msg)
        {
            var words = msg.Split(' ');
            foreach (var w in words)
                if (int.TryParse(w, out int seuil))
                    return seuil;
            return 2;
        }

        private DateTime ExtractDateFromMessage(string msg)
        {
            var words = msg.Split(' ');
            foreach (var w in words)
            {
                if (DateTime.TryParseExact(w, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime d))
                    return d;
                if (DateTime.TryParseExact(w, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out d))
                    return d;
            }
            return DateTime.MinValue;
        }
        #endregion

        #region Appel à l'API Groq avec contexte et mémoire
        private async Task<string> GetAIResponse(string userMessage, string actionResult)
        {
            string systemPrompt = @"Tu es un assistant RH professionnel, empathique et compétent. 
            Tu connais le droit du travail, les bonnes pratiques en ressources humaines, la gestion des talents, les entretiens, les conflits, la motivation, etc.
            Tu peux raconter des histoires courtes ou des exemples pour illustrer tes conseils.
            Réponds de manière naturelle, chaleureuse et utile.

            Règles importantes :
            - Tu as accès aux données de l'entreprise via des actions locales. Le résultat de ces actions t'est fourni entre crochets [Résultat de l'action...].
            - Si l'utilisateur demande des données (liste des employés, statistiques, etc.), base-toi STRICTEMENT sur le résultat donné.
            - Si aucune action n'a été déclenchée (pas de résultat), tu peux donner des conseils généraux, répondre à des questions RH, ou demander à l'utilisateur de reformuler.
            - Ne jamais inventer de données chiffrées ou de listes d'employés.
            - Pour les utilisateurs non administrateurs, certaines actions sont refusées (tu peux l'expliquer poliment).
            - Encourage les bonnes pratiques RH et reste professionnel.";

            // Construire la liste des messages à envoyer (contexte + dernier message)
            var messagesForApi = new List<object>();
            messagesForApi.Add(new { role = "system", content = systemPrompt });

            // Ajouter l'historique récent (conversationHistory)
            foreach (var msg in conversationHistory)
            {
                messagesForApi.Add(new { role = msg["role"], content = msg["content"] });
            }

            // Ajouter le message utilisateur actuel avec le résultat d'action si applicable
            string userContent = userMessage;
            if (!string.IsNullOrEmpty(actionResult))
                userContent += $"\n\n[Résultat de l'action exécutée : {actionResult}]";
            messagesForApi.Add(new { role = "user", content = userContent });

            var requestBody = new
            {
                model = "llama-3.1-8b-instant",
                messages = messagesForApi,
                temperature = 0.7,
                max_tokens = 1000
            };
            string json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {GROQ_API_KEY}");

            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(GROQ_API_URL, content);
                string responseBody = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    using JsonDocument doc = JsonDocument.Parse(responseBody);
                    JsonElement root = doc.RootElement;
                    if (root.TryGetProperty("choices", out JsonElement choices) && choices.GetArrayLength() > 0)
                        return choices[0].GetProperty("message").GetProperty("content").GetString() ?? "Je n'ai pas de réponse à vous fournir.";
                    return "Format de réponse inattendu.";
                }
                else
                {
                    return $"Erreur API ({response.StatusCode}) : {responseBody}";
                }
            }
            catch (Exception ex)
            {
                return $"Erreur réseau : {ex.Message}";
            }
        }
        #endregion
    }
}
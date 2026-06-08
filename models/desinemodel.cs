using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Gestion_des_Employés.models
{
    internal static class desinemodel
    {

        



        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int left, int top, int right, int bottom, int width, int height);

        public static void Rounded(Control ctrl, int radius)
        {
            if (ctrl.Width == 0 || ctrl.Height == 0) return;
            ctrl.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, ctrl.Width, ctrl.Height, radius, radius));
        }

        public static void MakeCirclePicture(PictureBox pb)
        {
            pb.SizeMode = PictureBoxSizeMode.Zoom;
            pb.Paint += (s, e) =>
            {
                GraphicsPath path = new GraphicsPath();
                path.AddEllipse(0, 0, pb.Width, pb.Height);
                pb.Region = new Region(path);
            };
            pb.Resize += (s, e) =>
            {
                GraphicsPath path = new GraphicsPath();
                path.AddEllipse(0, 0, pb.Width, pb.Height);
                pb.Region = new Region(path);
            };
        }

        // Couleurs modifiables avant utilisation
        public static Color PrimaryColor = Color.FromArgb(0, 120, 215);
        public static Color SecondaryColor = Color.FromArgb(32, 32, 32);
        public static Color BackgroundColor = Color.FromArgb(245, 247, 250);
        public static Color DangerColor = Color.FromArgb(220, 53, 69);
        public static Color WarningColor = Color.FromArgb(255, 193, 7);
        public static Color SuccessColor = Color.FromArgb(40, 167, 69);

        public static void StyleForm(Form form)
        {
            form.FormBorderStyle = FormBorderStyle.None;
            form.BackColor = BackgroundColor;
            form.DoubleBuffered(true);
        }

        public static void DoubleBuffered(this Form form, bool enable)
        {
            var prop = typeof(Control).GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            prop?.SetValue(form, enable, null);
        }

        public static void StylePanel(Panel panel)
        {
            panel.BackColor = Color.White;
            Rounded(panel, 15);
            panel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(Color.FromArgb(220, 220, 220), 1))
                {
                    Rectangle rect = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
                    e.Graphics.DrawRectangle(pen, rect);
                }
            };
        }

        public static void StyleButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = PrimaryColor;
            btn.ForeColor = Color.White;
            btn.Cursor = Cursors.Hand;
            Rounded(btn, 20);
            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = Color.FromArgb(0, 140, 240);
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = Color.White;
            };
            btn.MouseLeave += (s, e) =>
            {
                btn.BackColor = PrimaryColor;
                btn.FlatAppearance.BorderSize = 0;
            };
            btn.MouseDown += (s, e) => btn.BackColor = Color.FromArgb(0, 90, 180);
        }

        public static void StyleMenuButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = PrimaryColor;
            btn.ForeColor = Color.White;
            btn.Padding = new Padding(20, 0, 0, 0);
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.MouseLeave += (s, e) => btn.BackColor = PrimaryColor;
        }

        public static void StyleTextBox(TextBox tb)
        {
            tb.BorderStyle = BorderStyle.None;
            tb.BackColor = Color.White;
            tb.ForeColor = Color.Black;
            Rounded(tb, 12);
            tb.MouseEnter += (s, e) => tb.BackColor = Color.FromArgb(250, 250, 250);
            tb.MouseLeave += (s, e) => tb.BackColor = Color.White;
        }

        public static void StyleComboBox(ComboBox cb)
        {
            cb.FlatStyle = FlatStyle.Flat;
            cb.BackColor = Color.White;
            cb.ForeColor = Color.Black;
            cb.Font = new Font("Segoe UI", 9);
            Rounded(cb, 12);
        }

        public static void StyleDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Color.FromArgb(230, 230, 230);
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = true;

            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = PrimaryColor;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersHeight = 40;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgv.DefaultCellStyle.ForeColor = Color.Black;
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 230, 255);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.RowTemplate.Height = 35;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);
        }

        public static void StyleListBox(ListBox lb)
        {
            lb.BorderStyle = BorderStyle.None;
            lb.BackColor = Color.White;
            lb.Font = new Font("Segoe UI", 9);
            lb.ForeColor = Color.Black;
            lb.DrawMode = DrawMode.OwnerDrawVariable;
            lb.MeasureItem += (s, e) => e.ItemHeight = 35;
            lb.DrawItem += (s, e) =>
            {
                if (e.Index < 0) return;
                e.DrawBackground();
                Brush brush = (e.State & DrawItemState.Selected) != 0 ? new SolidBrush(Color.FromArgb(240, 248, 255)) : new SolidBrush(Color.White);
                e.Graphics.FillRectangle(brush, e.Bounds);
                string text = lb.Items[e.Index].ToString();
                e.Graphics.DrawString(text, lb.Font, Brushes.Black, e.Bounds.X + 5, e.Bounds.Y + 8);
                e.DrawFocusRectangle();
            };
        }

        public static Panel CreateStatCard(string title, string value, Image icon, Color accentColor)
        {
            Panel card = new Panel();
            card.Size = new Size(220, 110);
            card.BackColor = Color.White;
            Rounded(card, 15);
            card.Paint += (s, e) =>
            {
                using (var path = GetRoundedRectPath(card.ClientRectangle, 15))
                using (var brush = new SolidBrush(Color.FromArgb(20, 0, 0, 0)))
                {
                    e.Graphics.FillPath(brush, path);
                }
            };

            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                Location = new Point(15, 15),
                AutoSize = true
            };
            Label lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = accentColor,
                Location = new Point(15, 45),
                AutoSize = true
            };
            PictureBox pic = new PictureBox
            {
                Image = icon,
                SizeMode = PictureBoxSizeMode.Zoom,
                Location = new Point(card.Width - 55, 20),
                Size = new Size(40, 40),
                BackColor = Color.Transparent
            };
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValue);
            card.Controls.Add(pic);
            return card;
        }

        private static GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }

        public static Panel CreateSeparator(int width = 1)
        {
            Panel sep = new Panel();
            sep.Height = width;
            sep.BackColor = Color.FromArgb(220, 220, 220);
            sep.Dock = DockStyle.Top;
            return sep;
        }

        public static void ApplyGradient(Panel panel, Color startColor, Color endColor, float angle = 90f)
        {
            panel.Paint += (s, e) =>
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(panel.ClientRectangle, startColor, endColor, angle))
                {
                    e.Graphics.FillRectangle(brush, panel.ClientRectangle);
                }
            };
        }

        public static void StyleLabel(Label lbl, bool isTitle = false)
        {
            if (isTitle)
            {
                lbl.Font = new Font("Segoe UI", 14, FontStyle.Bold);
                lbl.ForeColor = PrimaryColor;
            }
            else
            {
                lbl.Font = new Font("Segoe UI", 9, FontStyle.Regular);
                lbl.ForeColor = Color.FromArgb(80, 80, 80);
            }
            lbl.AutoSize = true;
            lbl.BackColor = Color.Transparent;
        }

        public static void StyleBadge(Label lbl, int count)
        {
            lbl.Text = count.ToString();
            lbl.AutoSize = false;
            lbl.Size = new Size(24, 24);
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lbl.ForeColor = Color.White;
            lbl.BackColor = DangerColor;
            Rounded(lbl, 12);
            lbl.Padding = new Padding(0);
        }

        public static void StyleLogoutButton(Button btn, Image icon = null)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.TextAlign = ContentAlignment.MiddleRight;
            btn.ImageAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(5);
            if (icon != null)
            {
                btn.Image = icon;
                btn.ImageAlign = ContentAlignment.MiddleLeft;
            }
            btn.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
        }

        public static void StyleIconButton(Button btn, Image icon)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Color.White;
            btn.ForeColor = PrimaryColor;
            btn.Image = icon;
            btn.ImageAlign = ContentAlignment.MiddleCenter;
            btn.Text = "";
            btn.Size = new Size(40, 40);
            Rounded(btn, 20);
            btn.Cursor = Cursors.Hand;
            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(240, 248, 255);
            btn.MouseLeave += (s, e) => btn.BackColor = Color.White;
        }

        public static void StyleAvatar(PictureBox pb, string initials = null, Color? backColor = null)
        {
            MakeCirclePicture(pb);
            pb.SizeMode = PictureBoxSizeMode.Zoom;
            pb.BackColor = backColor ?? PrimaryColor;
            if (!string.IsNullOrEmpty(initials) && pb.Image == null)
            {
                Bitmap bmp = new Bitmap(pb.Width, pb.Height);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(pb.BackColor);
                    using (Font font = new Font("Segoe UI", pb.Width / 3, FontStyle.Bold))
                    using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    {
                        g.DrawString(initials, font, Brushes.White, new Rectangle(0, 0, pb.Width, pb.Height), sf);
                    }
                }
                pb.Image = bmp;
            }
        }

        public static void StyleSectionTitle(Label lbl)
        {
            lbl.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lbl.ForeColor = SecondaryColor;
            lbl.AutoSize = true;
        }

        public static void StyleLinkLabel(LinkLabel ll)
        {
            ll.LinkColor = PrimaryColor;
            ll.VisitedLinkColor = ControlPaint.Dark(PrimaryColor, 0.2f);
            ll.ActiveLinkColor = ControlPaint.Light(PrimaryColor, 0.2f);
            ll.Font = new Font("Segoe UI", 9);
            ll.LinkBehavior = LinkBehavior.HoverUnderline;
        }

        // ---------- STYLES POUR LE CHAT IA ----------

        // Style d'une bulle de message (IA ou utilisateur)
        
        public static void StyleIAMessage(Label lbl, bool isUser)
        {
            lbl.Font = new Font("Segoe UI", 9);
            lbl.Padding = new Padding(10);
            lbl.AutoSize = false;
            lbl.MaximumSize = new Size(250, 0);
            Rounded(lbl, 15);
            if (isUser)
            {
                lbl.BackColor = PrimaryColor;
                lbl.ForeColor = Color.White;
                lbl.TextAlign = ContentAlignment.MiddleRight;
            }
            else
            {
                lbl.BackColor = Color.FromArgb(240, 240, 240);
                lbl.ForeColor = Color.Black;
                lbl.TextAlign = ContentAlignment.MiddleLeft;
            }
        }

        // Style de la zone de saisie (RichTextBox)
        public static void StyleChatInput(RichTextBox rtb)
        {
            rtb.BorderStyle = BorderStyle.None;
            rtb.BackColor = Color.White;
            rtb.Font = new Font("Segoe UI", 9);
            rtb.ForeColor = Color.Black;
            Rounded(rtb, 15);
            // Ajout d'une bordure légère au focus
            rtb.Enter += (s, e) => rtb.BackColor = Color.FromArgb(255, 255, 240);
            rtb.Leave += (s, e) => rtb.BackColor = Color.White;
        }

        // Style du bouton d'envoi (PictureBox utilisé comme bouton)
        public static void StyleSendButton(PictureBox pb)
        {
            MakeCirclePicture(pb); // rend l'icône circulaire
            pb.SizeMode = PictureBoxSizeMode.Zoom;
            pb.BackColor = PrimaryColor;
            pb.Cursor = Cursors.Hand;
            // Effet au survol
            pb.MouseEnter += (s, e) => pb.BackColor = ControlPaint.Light(PrimaryColor, 0.3f);
            pb.MouseLeave += (s, e) => pb.BackColor = PrimaryColor;
        }

        // Style de l'en-tête du panneau chat
        public static void StyleChatHeader(Panel header)
        {
            header.BackColor = Color.White;
            Rounded(header, 15);
            // Ombre légère sous l'en-tête
            header.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(200, 200, 200), 1))
                {
                    e.Graphics.DrawLine(pen, 0, header.Height - 1, header.Width, header.Height - 1);
                }
            };
        }

        // Style pour une carte (panel) : arrondi, ombre, fond personnalisable
        public static void StyleCard(Panel card, Color backColor)
        {
            card.BackColor = backColor;
            Rounded(card, 15);
            // Ombre portée (facultative)
            card.Paint += (s, e) =>
            {
                using (var path = GetRoundedRectPath(card.ClientRectangle, 15))
                using (var brush = new SolidBrush(Color.FromArgb(30, 0, 0, 0)))
                {
                    e.Graphics.FillPath(brush, path);
                }
            };
        }

        // Style pour le titre de la carte (ex: "Utilisateurs")
        public static void StyleCardTitle(Label lbl)
        {
            lbl.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lbl.ForeColor = Color.White;
            lbl.BackColor = Color.Transparent;
        }

        // Style pour la valeur chiffrée (ex: "30")
        public static void StyleCardValue(Label lbl)
        {
            lbl.Font = new Font("Segoe UI", 36, FontStyle.Bold);
            lbl.ForeColor = Color.White;
            lbl.BackColor = Color.Transparent;
        }

        // Style pour le sous-titre (ex: "nombre d'utilisateur")
        public static void StyleCardSubtitle(Label lbl)
        {
            lbl.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lbl.ForeColor = Color.White;
            lbl.BackColor = Color.Transparent;
        }

        public static void StyleChatRichTextBox(RichTextBox rtb)
        {
            rtb.BorderStyle = BorderStyle.None;
            rtb.BackColor = Color.FromArgb(250, 250, 250);
            rtb.Font = new Font("Segoe UI", 9);
            rtb.ReadOnly = true;
            rtb.ScrollBars = RichTextBoxScrollBars.Vertical;
        }

        // Style pour un bouton "action" (ex: Nouvel utilisateur, Voir tous, etc.)
        public static void StyleActionButton(Button btn, Color? backColor = null)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = backColor ?? PrimaryColor;
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            Rounded(btn, 20);
            btn.MouseEnter += (s, e) => btn.BackColor = ControlPaint.Light(btn.BackColor, 0.2f);
            btn.MouseLeave += (s, e) => btn.BackColor = backColor ?? PrimaryColor;
        }

        // Style pour un label de titre de section (ex: "Notification Alertes")
        public static void StyleSectionHeader(Label lbl)
        {
            lbl.Font = new Font("Segoe UI", 18, FontStyle.Bold | FontStyle.Underline);
            lbl.ForeColor = SecondaryColor;
            lbl.AutoSize = true;
        }

        // Style pour un panel de contenu (ex: PanelAfficheNotificationClick)
        public static void StyleContentPanel(Panel panel)
        {
            panel.BackColor = Color.White;
            Rounded(panel, 15);
            panel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(Color.FromArgb(220, 220, 220), 1))
                {
                    Rectangle rect = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
                    e.Graphics.DrawRectangle(pen, rect);
                }
            };
        }

        // Style pour un formulaire de connexion (UserConnexion)
        public static void StyleLoginForm(UserControl control)
        {
            control.BackColor = BackgroundColor;
            foreach (Control c in control.Controls)
            {
                if (c is TextBox tb) StyleTextBox(tb);
                else if (c is Button btn) StyleButton(btn);
                else if (c is Label lbl) StyleLabel(lbl);
                else if (c is CheckBox chk) chk.Font = new Font("Segoe UI", 9);
                else if (c is PictureBox pb) MakeCirclePicture(pb);
            }
        }


        public static void StylePasswordTextBox(TextBox tb)
        {
            StyleTextBox(tb);          
            tb.PasswordChar = '*';
            tb.UseSystemPasswordChar = true;
        }


    }
}
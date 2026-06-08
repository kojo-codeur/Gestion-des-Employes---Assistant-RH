using System;
using System.Drawing;
using System.Windows.Forms;

namespace Gestion_des_Employés.models
{
    public static class ResponsiveHelper
    {
        // Version par défaut (hauteur carte 130, marges 5)
        public static void SetupDashboardLayout(UserControl userControl, Panel[] cards, Panel[] charts)
        {
            SetupDashboardLayout(userControl, cards, charts, 130, 5);
        }

        // Version paramétrable
        public static void SetupDashboardLayout(UserControl userControl, Panel[] cards, Panel[] charts, int cardHeight, int chartMargin)
        {
            if (cards.Length != 4 || charts.Length != 4)
                throw new ArgumentException("Il faut exactement 4 cartes et 4 graphiques.");

            userControl.SuspendLayout();

            // 1. TableLayout pour les 4 cartes
            var cardTable = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = cardHeight,
                ColumnCount = 4,
                RowCount = 1,
                Padding = new Padding(8),
                BackColor = Color.Transparent
            };
            for (int i = 0; i < 4; i++)
                cardTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));

            for (int i = 0; i < 4; i++)
            {
                cards[i].Dock = DockStyle.Fill;
                cards[i].Margin = new Padding(chartMargin);
                cardTable.Controls.Add(cards[i], i, 0);
            }

            // 2. TableLayout pour les 4 graphiques
            var chartTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2,
                Padding = new Padding(8),
                BackColor = Color.Transparent
            };
            chartTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            chartTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            chartTable.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            chartTable.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

            int[,] positions = { { 0, 0 }, { 1, 0 }, { 0, 1 }, { 1, 1 } };
            for (int i = 0; i < 4; i++)
            {
                charts[i].Dock = DockStyle.Fill;
                charts[i].Margin = new Padding(chartMargin);
                chartTable.Controls.Add(charts[i], positions[i, 0], positions[i, 1]);
            }

            userControl.Controls.Clear();
            userControl.Controls.Add(chartTable);
            userControl.Controls.Add(cardTable);
            userControl.ResumeLayout();
        }
    }
}
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PsychrometricsSample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // Link the Paint event of the panelChart
            panelChart.Paint += new PaintEventHandler(PanelChart_Paint);
        }

        private void PanelChart_Paint(object sender, PaintEventArgs e)
        {
            DrawPsychrometricChart(e.Graphics);
        }

        private void DrawPsychrometricChart(Graphics g)
        {
            // Define chart boundaries
            int margin = 50;
            int chartWidth = panelChart.ClientSize.Width - 2 * margin;
            int chartHeight = panelChart.ClientSize.Height - 2 * margin;

            // Draw axes
            Pen axisPen = new Pen(Color.Black, 2);
            g.DrawLine(axisPen, margin, margin, margin, chartHeight + margin);
            g.DrawLine(axisPen, margin, chartHeight + margin, chartWidth + margin, chartHeight + margin);

            // Draw temperature lines (dry-bulb temperature)
            for (int t = 0; t <= 50; t += 5)
            {
                int x = margin + (int)(t * chartWidth / 50.0);
                g.DrawLine(Pens.Gray, x, margin, x, chartHeight + margin);
                g.DrawString(t.ToString(), this.Font, Brushes.Black, x - 10, chartHeight + margin + 5);
            }

            // Draw humidity ratio lines
            for (int w = 0; w <= 30; w += 5)
            {
                int y = chartHeight + margin - (int)(w * chartHeight / 30.0);
                g.DrawLine(Pens.Gray, margin, y, chartWidth + margin, y);
                g.DrawString(w.ToString(), this.Font, Brushes.Black, margin - 30, y - 5);
            }

            // Draw relative humidity curves (simplified for demonstration)
            for (int rh = 10; rh <= 100; rh += 10)
            {
                DrawRelativeHumidityCurve(g, rh, margin, chartWidth, chartHeight);
            }
        }

        private void DrawRelativeHumidityCurve(Graphics g, int rh, int margin, int chartWidth, int chartHeight)
        {
            Point[] points = new Point[51];
            for (int t = 0; t <= 50; t++)
            {
                double humidityRatio = CalculateHumidityRatio(t, rh);
                int x = margin + (int)(t * chartWidth / 50.0);
                int y = chartHeight + margin - (int)(humidityRatio * chartHeight / 30.0);
                points[t] = new Point(x, y);
            }
            g.DrawCurve(Pens.Blue, points);
        }

        private double CalculateHumidityRatio(int dryBulbTemperature, int relativeHumidity)
        {
            // Simplified calculation, in reality this involves more complex psychrometric formulas
            double saturationPressure = 0.61078 * Math.Exp((17.27 * dryBulbTemperature) / (dryBulbTemperature + 237.3));
            double actualVaporPressure = saturationPressure * relativeHumidity / 100.0;
            double humidityRatio = 0.622 * actualVaporPressure / (101.325 - actualVaporPressure);
            return humidityRatio * 1000; // Convert to grams per kilogram
        }
    }
}

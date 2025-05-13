// Controls/PointDetailControl.cs
using System;
using System.Drawing;
using System.Windows.Forms;
using DrivesViewer.Models;

namespace DrivesViewer.Controls
{
    public partial class PointDetailControl : UserControl
    {
        private DriveDataPoint _pt;

        public PointDetailControl()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        /// <summary>
        /// Nastaví bod, jehož detaily se budou vykreslovat.
        /// </summary>
        public void SetPoint(DriveDataPoint pt)
        {
            _pt = pt;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.Clear(Color.White);

            if (_pt == null)
            {
                // žádný bod není vybraný
                using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    g.DrawString("Vyberte bod", Font, Brushes.Gray, ClientRectangle, sf);
                }
                return;
            }

            const int textX = 10;
            int y = 10;
            int lineHeight = (int)(Font.GetHeight(e.Graphics) + 4);

            // Čas od startu
            g.DrawString($"Čas: {_pt.TimeFromStartSeconds}s", Font, Brushes.Black, textX, y);
            y += lineHeight;

            // Rychlost
            g.DrawString($"Rychlost: {_pt.SpeedRec:F1} km/h", Font, Brushes.Black, textX, y);
            y += lineHeight;

            // Náklon
            g.DrawString($"Roll: {_pt.Roll:F1}°", Font, Brushes.Black, textX, y);
            y += lineHeight;

            // Brzdí / zrychluje
            var accelText = _pt.SpeedRec - (_pt.Roll * 0) < 0 ? "Brzdí" : "Zrychluje";
            g.DrawString(accelText, Font, Brushes.Black, textX, y);

            // Jednoduchý grafický model motocyklu v dolní části
            var bikeWidth = 40;
            var bikeHeight = 10;
            var centerX = ClientSize.Width - bikeWidth - 10;
            var centerY = ClientSize.Height - bikeHeight - 10;

            g.TranslateTransform(centerX + bikeWidth / 2f, centerY + bikeHeight / 2f);
            g.RotateTransform((float)_pt.Roll);
            using (var brush = new SolidBrush(Color.Gray))
            {
                g.FillRectangle(brush, -bikeWidth / 2f, -bikeHeight / 2f, bikeWidth, bikeHeight);
            }
            g.ResetTransform();
        }
    }
}

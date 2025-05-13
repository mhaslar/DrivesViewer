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
        private double _accel;

        public PointDetailControl()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        /// <summary>
        /// Nastaví bod a akceleraci, vykreslí detail.
        /// </summary>
        public void SetPoint(DriveDataPoint pt, double accel)
        {
            _pt = pt;
            _accel = accel;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.Clear(Color.White);

            if (_pt == null)
            {
                using var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString("Vyberte bod", Font, Brushes.Gray, ClientRectangle, sf);
                return;
            }

            int x = 10, y = 10;
            int lh = (int)(Font.GetHeight(e.Graphics) + 4);

            // Čas
            g.DrawString($"Čas: {_pt.TimeFromStartSeconds}s", Font, Brushes.Black, x, y);
            y += lh;
            // Rychlost
            g.DrawString($"Rychlost: {_pt.SpeedRec:F1} km/h", Font, Brushes.Black, x, y);
            y += lh;
            // Náklon
            g.DrawString($"Roll: {_pt.Roll:F1}°", Font, Brushes.Black, x, y);
            y += lh;
            // Brzdí / zrychluje
            var txt = _accel < 0 ? "Brzdí" : "Zrychluje";
            g.DrawString(txt, Font, Brushes.Black, x, y);
            y += lh + 10;

            // Vykreslení jednoduché motorky s koly a náklonem
            int bodyW = 40, bodyH = 10;
            float cx = ClientSize.Width / 2f;
            float cy = ClientSize.Height - bodyH - 20;

            g.TranslateTransform(cx, cy);
            g.RotateTransform((float)_pt.Roll);

            // Tělo
            using (var bodyBrush = new SolidBrush(Color.Gray))
                g.FillRectangle(bodyBrush, -bodyW / 2f, -bodyH / 2f, bodyW, bodyH);

            // Kola
            float wheelR = bodyH;
            using (var wheelBrush = new SolidBrush(Color.Black))
            {
                g.FillEllipse(wheelBrush, -bodyW / 2f - wheelR / 2, -wheelR / 2, wheelR, wheelR);
                g.FillEllipse(wheelBrush, bodyW / 2f - wheelR / 2, -wheelR / 2, wheelR, wheelR);
            }

            g.ResetTransform();
        }
    }
}

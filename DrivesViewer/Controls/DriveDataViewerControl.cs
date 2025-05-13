// Controls/DriveDataViewerControl.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DrivesViewer.Models;
using DrivesViewer.Utils;

namespace DrivesViewer.Controls
{
    public enum RenderMode { Curve, Speed }

    public partial class DriveDataViewerControl : UserControl
    {
        private List<DriveDataPoint> _points = new List<DriveDataPoint>();

        /// <summary>
        /// Pokud je Curve, vykreslí spojnice + body v zatáčkách barevně podle směřování.
        /// Pokud je Speed, vykreslí spojnice barevným perem podle akcelerace/brzdění.
        /// </summary>
        public RenderMode Mode { get; set; } = RenderMode.Curve;

        public DriveDataViewerControl()
        {
            InitializeComponent();
            // abychom se vyhnuli blikání
            DoubleBuffered = true;
        }

        /// <summary>
        /// Načte nová data a (pokud je AutoRedraw zapnuto) hned překreslí.
        /// </summary>
        public void LoadData(List<DriveDataPoint> points)
        {
            _points = points ?? new List<DriveDataPoint>();
            if (SettingsManager.AutoRedraw)
                Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.Clear(Color.White);

            if (_points == null || _points.Count < 2)
                return;

            const int margin = 10;

            // jen body s platnými souřadnicemi
            var pts = _points
                .Where(p => p.LatRec.HasValue && p.LonRec.HasValue)
                .ToList();
            if (pts.Count < 2) return;

            // spočítáme min/max pro lat/lon
            float minLat = (float)pts.Min(p => p.LatRec.Value);
            float maxLat = (float)pts.Max(p => p.LatRec.Value);
            float minLon = (float)pts.Min(p => p.LonRec.Value);
            float maxLon = (float)pts.Max(p => p.LonRec.Value);

            // lambda pro převod na pixely
            Func<DriveDataPoint, PointF> map = p =>
            {
                float x = margin + (float)((p.LonRec.Value - minLon) / (maxLon - minLon) * (Width - 2 * margin));
                float y = margin + (float)((maxLat - p.LatRec.Value) / (maxLat - minLat) * (Height - 2 * margin));
                return new PointF(x, y);
            };

            if (Mode == RenderMode.Curve)
            {
                // vykreslíme spojnice i zvýraznění zatáček
                for (int i = 1; i < pts.Count; i++)
                {
                    var a = pts[i - 1];
                    var b = pts[i];
                    var p1 = map(a);
                    var p2 = map(b);

                    using (var pen = new Pen(Color.Black, 1))
                        g.DrawLine(pen, p1, p2);

                    if (b.InCurve)
                    {
                        // barva podle směru zatáčky
                        var brush = new SolidBrush(
                            b.CircuitCurveTurnDirection == 'L'
                                ? SettingsManager.ColorLeftCurve
                                : SettingsManager.ColorRightCurve
                        );
                        const float size = 6;
                        g.FillEllipse(brush, p2.X - size / 2, p2.Y - size / 2, size, size);
                        brush.Dispose();
                    }
                }
            }
            else // RenderMode.Speed
            {
                // vykreslíme spojnice barevně podle akcelerace/brzdění
                for (int i = 1; i < pts.Count; i++)
                {
                    var a = pts[i - 1];
                    var b = pts[i];
                    var p1 = map(a);
                    var p2 = map(b);

                    double accel = b.SpeedRec - a.SpeedRec;
                    using (var pen = new Pen(
                        accel < 0
                            ? SettingsManager.ColorBrake
                            : SettingsManager.ColorSpeed,
                        2))
                    {
                        g.DrawLine(pen, p1, p2);
                    }
                }
            }
        }
    }
}

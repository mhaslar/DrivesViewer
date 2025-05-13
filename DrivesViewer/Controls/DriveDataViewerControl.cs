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
        private List<DriveDataPoint> _points = new();
        private List<DriveDataPoint> _filteredPoints = new();
        private List<PointF> _pixelPoints = new();

        /// <summary>Režim vykreslení – zatáčky nebo rychlost.</summary>
        public RenderMode Mode { get; set; } = RenderMode.Curve;

        /// <summary>Kliknutím na bod vrátí bod + hodnotu akcelerace (km/h/s).</summary>
        public event Action<DriveDataPoint, double> PointSelected;

        // prahová hodnota pro „mírné zatáčky“ v stupních
        private const double CurveRollThreshold = 5.0;

        public DriveDataViewerControl()
        {
            InitializeComponent();
            DoubleBuffered = true;
            MouseClick += DriveDataViewerControl_MouseClick;
        }

        public void LoadData(List<DriveDataPoint> points)
        {
            _points = points ?? new List<DriveDataPoint>();
            if (SettingsManager.AutoRedraw) Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.Clear(Color.White);
            if (_points.Count < 2) return;

            const int margin = 10;
            _filteredPoints = _points
                .Where(p => p.LatRec.HasValue && p.LonRec.HasValue)
                .ToList();
            if (_filteredPoints.Count < 2) return;

            float minLat = (float)_filteredPoints.Min(p => p.LatRec.Value);
            float maxLat = (float)_filteredPoints.Max(p => p.LatRec.Value);
            float minLon = (float)_filteredPoints.Min(p => p.LonRec.Value);
            float maxLon = (float)_filteredPoints.Max(p => p.LonRec.Value);

            PointF Map(DriveDataPoint p) =>
                new PointF(
                    margin + (float)((p.LonRec.Value - minLon) / (maxLon - minLon) * (Width - 2 * margin)),
                    margin + (float)((maxLat - p.LatRec.Value) / (maxLat - minLat) * (Height - 2 * margin))
                );

            _pixelPoints = _filteredPoints.Select(Map).ToList();

            if (Mode == RenderMode.Curve)
            {
                for (int i = 1; i < _filteredPoints.Count; i++)
                {
                    var a = _filteredPoints[i - 1];
                    var b = _filteredPoints[i];
                    var p1 = _pixelPoints[i - 1];
                    var p2 = _pixelPoints[i];

                    using var pen = new Pen(Color.Black, 1);
                    g.DrawLine(pen, p1, p2);

                    // klasické i mírné zatáčky
                    bool isCurve = b.InCurve || Math.Abs(b.Roll) >= CurveRollThreshold;
                    if (isCurve)
                    {
                        // směr buď z DB, nebo dle znaménka roll
                        char dir = b.CircuitCurveTurnDirection.HasValue
                            ? b.CircuitCurveTurnDirection.Value
                            : (b.Roll > 0 ? 'R' : 'L');
                        var color = dir == 'L'
                            ? SettingsManager.ColorLeftCurve
                            : SettingsManager.ColorRightCurve;

                        using var brush = new SolidBrush(color);
                        const float size = 6;
                        g.FillEllipse(brush, p2.X - size / 2, p2.Y - size / 2, size, size);
                    }
                }
            }
            else // Speed mode
            {
                for (int i = 1; i < _filteredPoints.Count; i++)
                {
                    var a = _filteredPoints[i - 1];
                    var b = _filteredPoints[i];
                    var p1 = _pixelPoints[i - 1];
                    var p2 = _pixelPoints[i];
                    double accel = b.SpeedRec - a.SpeedRec;

                    using var pen = new Pen(accel < 0
                        ? SettingsManager.ColorBrake
                        : SettingsManager.ColorSpeed, 2);
                    g.DrawLine(pen, p1, p2);

                    // větší bod
                    using var brush = new SolidBrush(accel < 0
                        ? SettingsManager.ColorBrake
                        : SettingsManager.ColorSpeed);
                    const float pointSize = 8;
                    g.FillEllipse(brush, p2.X - pointSize / 2, p2.Y - pointSize / 2, pointSize, pointSize);
                }
            }
        }

        private void DriveDataViewerControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (_pixelPoints == null || _pixelPoints.Count == 0) return;

            float best = float.MaxValue;
            int idx = -1;
            for (int i = 0; i < _pixelPoints.Count; i++)
            {
                var dx = _pixelPoints[i].X - e.X;
                var dy = _pixelPoints[i].Y - e.Y;
                var dist2 = dx * dx + dy * dy;
                if (dist2 < best)
                {
                    best = dist2;
                    idx = i;
                }
            }

            // prah 10 px (100 = 10²)
            if (idx >= 0 && best <= 100)
            {
                double accel = 0;
                if (idx > 0)
                    accel = _filteredPoints[idx].SpeedRec - _filteredPoints[idx - 1].SpeedRec;

                PointSelected?.Invoke(_filteredPoints[idx], accel);
            }
        }
    }
}

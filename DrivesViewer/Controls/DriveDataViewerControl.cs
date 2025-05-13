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
        // Body, která skutečně kreslíme (bez NULL souřadnic)
        private List<DriveDataPoint> _filteredPoints = new();
        // Jejich pozice v pixelech
        private List<PointF> _pixelPoints = new();

        /// <summary>Režim vykreslení – zatáčky nebo rychlost.</summary>
        public RenderMode Mode { get; set; } = RenderMode.Curve;

        /// <summary>Kliknutím na bod se vyvolá s daty DriveDataPoint.</summary>
        public event Action<DriveDataPoint> PointSelected;

        public DriveDataViewerControl()
        {
            InitializeComponent();
            DoubleBuffered = true;
            // přihlásit klik
            this.MouseClick += DriveDataViewerControl_MouseClick;
        }

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
            if (_points.Count < 2) return;

            const int margin = 10;
            // vyfiltrovat jen bod s validními souřadnicemi
            _filteredPoints = _points
                .Where(p => p.LatRec.HasValue && p.LonRec.HasValue)
                .ToList();
            if (_filteredPoints.Count < 2) return;

            // spočítat rozsahy
            float minLat = (float)_filteredPoints.Min(p => p.LatRec.Value),
                  maxLat = (float)_filteredPoints.Max(p => p.LatRec.Value),
                  minLon = (float)_filteredPoints.Min(p => p.LonRec.Value),
                  maxLon = (float)_filteredPoints.Max(p => p.LonRec.Value);

            // lambda pro převod na pixely
            PointF Map(DriveDataPoint p)
            {
                float x = margin + (float)((p.LonRec.Value - minLon) / (maxLon - minLon) * (Width - 2 * margin));
                float y = margin + (float)((maxLat - p.LatRec.Value) / (maxLat - minLat) * (Height - 2 * margin));
                return new PointF(x, y);
            }

            // přepočítat pixelové body
            _pixelPoints = _filteredPoints.Select(Map).ToList();

            if (Mode == RenderMode.Curve)
            {
                for (int i = 1; i < _filteredPoints.Count; i++)
                {
                    var a = _filteredPoints[i - 1];
                    var b = _filteredPoints[i];
                    var p1 = _pixelPoints[i - 1];
                    var p2 = _pixelPoints[i];

                    // čára
                    using var pen = new Pen(Color.Black, 1);
                    g.DrawLine(pen, p1, p2);

                    // bod v zatáčce
                    if (b.InCurve && b.CircuitCurveTurnDirection.HasValue)
                    {
                        var color = b.CircuitCurveTurnDirection.Value == 'L'
                            ? SettingsManager.ColorLeftCurve
                            : SettingsManager.ColorRightCurve;
                        using var brush = new SolidBrush(color);
                        const float size = 6;
                        g.FillEllipse(brush, p2.X - size / 2, p2.Y - size / 2, size, size);
                    }
                }
            }
            else // Speed
            {
                for (int i = 1; i < _filteredPoints.Count; i++)
                {
                    var a = _filteredPoints[i - 1];
                    var b = _filteredPoints[i];
                    var p1 = _pixelPoints[i - 1];
                    var p2 = _pixelPoints[i];
                    double accel = b.SpeedRec - a.SpeedRec;

                    // barevná čára
                    using var pen = new Pen(accel < 0
                        ? SettingsManager.ColorBrake
                        : SettingsManager.ColorSpeed, 2);
                    g.DrawLine(pen, p1, p2);

                    // **větší bod**
                    var brush = new SolidBrush(accel < 0
                        ? SettingsManager.ColorBrake
                        : SettingsManager.ColorSpeed);
                    const float pointSize = 8;
                    g.FillEllipse(brush, p2.X - pointSize / 2, p2.Y - pointSize / 2, pointSize, pointSize);
                    brush.Dispose();
                }
            }
        }

        private void DriveDataViewerControl_MouseClick(object sender, MouseEventArgs e)
        {
            // najít nejbližší bod
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
            // pokud je v dosahu ~10px (100 = 10²)
            if (idx >= 0 && best <= 100)
            {
                PointSelected?.Invoke(_filteredPoints[idx]);
            }
        }
    }
}

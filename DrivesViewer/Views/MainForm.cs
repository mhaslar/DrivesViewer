// Views/MainForm.cs
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using Dapper;
using DrivesViewer.Controls;
using DrivesViewer.Models;

namespace DrivesViewer.Views
{
    public partial class MainForm : Form
    {
        private readonly string _connString;

        public MainForm()
        {
            InitializeComponent();

            // v konstruktoru po InitializeComponent()
            driveViewer.PointSelected += (pt, accel) =>
            {
                pointDetail.SetPoint(pt, accel);
            };



            _connString = ConfigurationManager
                .ConnectionStrings["DrivesDb"]
                .ConnectionString;

            LoadRecordings();

            recordingSelectorControl.RecordingChanged += OnRecordingChanged;

            rbCurve.CheckedChanged += (s, e) =>
            {
                if (rbCurve.Checked)
                {
                    driveViewer.Mode = RenderMode.Curve;
                    driveViewer.Invalidate();
                }
            };
            rbSpeed.CheckedChanged += (s, e) =>
            {
                if (rbSpeed.Checked)
                {
                    driveViewer.Mode = RenderMode.Speed;
                    driveViewer.Invalidate();
                }
            };

            settingsToolStripMenuItem.Click += (s, e) =>
            {
                using var dlg = new SettingsForm();
                if (dlg.ShowDialog() == DialogResult.OK)
                    driveViewer.Invalidate();
            };
        }

        private void LoadRecordings()
        {
            using var conn = new SqlConnection(_connString);
            var recs = conn
                .Query<Recording>(
                    @"SELECT Id, StartDateTime, SensorsDeviceName 
                      FROM Recordings 
                      ORDER BY StartDateTime")
                .ToList();
            recordingSelectorControl.SetRecordings(recs);
        }

        private void OnRecordingChanged(int recordingId)
        {
            using var conn = new SqlConnection(_connString);
            var pts = conn
                .Query<DriveDataPoint>(
                    @"SELECT RecordingId,
                             TimeFromStartSeconds,
                             LatRec,
                             LonRec,
                             SpeedRec,
                             InCurve,
                             Roll,
                             Ax,
                             CircuitCurveTurnDirection
                      FROM DriveData
                      WHERE RecordingId = @Id
                      ORDER BY TimeFromStartSeconds",
                    new { Id = recordingId })
                .ToList();

            driveViewer.LoadData(pts);

            if (pts.Any())
            {
                var first = pts.First();
                // Pro první bod spočteme akceleraci vůči sobě samému (0), 
                // nebo vůči druhému bodu, pokud existuje:
                double accel = pts.Count > 1
                    ? pts[1].SpeedRec - pts[0].SpeedRec
                    : 0.0;
                pointDetail.SetPoint(first, accel);
            }
            else
            {
                // Při nullu předáme i 0 jako accel
                pointDetail.SetPoint(null, 0.0);
            }
        }
    }
}

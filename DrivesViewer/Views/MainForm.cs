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

            driveViewer.PointSelected += pt =>
            {
                pointDetail.SetPoint(pt);
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
                      WHERE RecordingId = @Id                -- opraveno z WHERE Id
                      ORDER BY TimeFromStartSeconds",
                    new { Id = recordingId })
                .ToList();

            // načteme data do rendereru
            driveViewer.LoadData(pts);

            // pokud existují body, zobrazíme první
            if (pts.Any())
                pointDetail.SetPoint(pts.First());
            else
                pointDetail.SetPoint(null);
        }
    }
}

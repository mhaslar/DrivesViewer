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

            // načteme connection string
            _connString = ConfigurationManager
                .ConnectionStrings["DrivesDb"]
                .ConnectionString;

            LoadRecordings();

            // když uživatel zvolí jiný záznam
            recordingSelectorControl.RecordingChanged += OnRecordingChanged;

            // přepínání režimů vykreslení
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

            // otevření dialogu Nastavení
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
                      WHERE Id = @Id
                      ORDER BY TimeFromStartSeconds",
                    new { Id = recordingId })
                .ToList();
            driveViewer.LoadData(pts);
            pointDetail.SetPoint(null);
        }
    }
}

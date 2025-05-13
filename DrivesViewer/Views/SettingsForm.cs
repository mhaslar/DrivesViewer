using System;
using System.Windows.Forms;
using DrivesViewer.Utils;

namespace DrivesViewer.Views
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            chkAutoRedraw.Checked = SettingsManager.AutoRedraw;
            btnSpeed.BackColor = SettingsManager.ColorSpeed;
            btnBrake.BackColor = SettingsManager.ColorBrake;
            btnLeftCurve.BackColor = SettingsManager.ColorLeftCurve;
            btnRightCurve.BackColor = SettingsManager.ColorRightCurve;
            chkAutoRedraw.CheckedChanged += (s, e) => SettingsManager.AutoRedraw = chkAutoRedraw.Checked;
            btnSpeed.Click += (s, e) => PickColor(c => SettingsManager.ColorSpeed = c, btnSpeed);
            btnBrake.Click += (s, e) => PickColor(c => SettingsManager.ColorBrake = c, btnBrake);
            btnLeftCurve.Click += (s, e) => PickColor(c => SettingsManager.ColorLeftCurve = c, btnLeftCurve);
            btnRightCurve.Click += (s, e) => PickColor(c => SettingsManager.ColorRightCurve = c, btnRightCurve);
            btnOK.Click += (s, e) => { DialogResult = DialogResult.OK; Close(); };
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
        }
        private void PickColor(Action<System.Drawing.Color> setColor, Button btn)
        {
            using var dlg = new ColorDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                setColor(dlg.Color);
                btn.BackColor = dlg.Color;
            }
        }
    }
}
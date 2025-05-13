using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DrivesViewer.Views
{
    partial class SettingsForm
    {
        private IContainer components = null;
        private CheckBox chkAutoRedraw;
        private Button btnSpeed, btnBrake, btnLeftCurve, btnRightCurve, btnOK, btnCancel;
        private Label lblSpeed, lblBrake, lblLeftCurve, lblRightCurve;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new Container();
            chkAutoRedraw = new CheckBox();
            lblSpeed = new Label(); btnSpeed = new Button();
            lblBrake = new Label(); btnBrake = new Button();
            lblLeftCurve = new Label(); btnLeftCurve = new Button();
            lblRightCurve = new Label(); btnRightCurve = new Button();
            btnOK = new Button(); btnCancel = new Button();
            SuspendLayout();
            // chkAutoRedraw
            chkAutoRedraw.AutoSize = true;
            chkAutoRedraw.Location = new System.Drawing.Point(12, 12);
            chkAutoRedraw.Text = "Auto Redraw";
            // lblSpeed
            lblSpeed.AutoSize = true;
            lblSpeed.Location = new System.Drawing.Point(12, 40);
            lblSpeed.Text = "Speed Color:";
            // btnSpeed
            btnSpeed.Location = new System.Drawing.Point(120, 36);
            btnSpeed.Size = new System.Drawing.Size(50, 23);
            // lblBrake
            lblBrake.AutoSize = true;
            lblBrake.Location = new System.Drawing.Point(12, 70);
            lblBrake.Text = "Brake Color:";
            // btnBrake
            btnBrake.Location = new System.Drawing.Point(120, 66);
            btnBrake.Size = new System.Drawing.Size(50, 23);
            // lblLeftCurve
            lblLeftCurve.AutoSize = true;
            lblLeftCurve.Location = new System.Drawing.Point(12, 100);
            lblLeftCurve.Text = "Left Curve Color:";
            // btnLeftCurve
            btnLeftCurve.Location = new System.Drawing.Point(120, 96);
            btnLeftCurve.Size = new System.Drawing.Size(50, 23);
            // lblRightCurve
            lblRightCurve.AutoSize = true;
            lblRightCurve.Location = new System.Drawing.Point(12, 130);
            lblRightCurve.Text = "Right Curve Color:";
            // btnRightCurve
            btnRightCurve.Location = new System.Drawing.Point(120, 126);
            btnRightCurve.Size = new System.Drawing.Size(50, 23);
            // btnOK
            btnOK.Text = "OK";
            btnOK.Location = new System.Drawing.Point(30, 170);
            btnOK.DialogResult = DialogResult.OK;
            // btnCancel
            btnCancel.Text = "Cancel";
            btnCancel.Location = new System.Drawing.Point(120, 170);
            btnCancel.DialogResult = DialogResult.Cancel;
            // SettingsForm
            ClientSize = new System.Drawing.Size(200, 210);
            Controls.AddRange(new Control[] { chkAutoRedraw, lblSpeed, btnSpeed, lblBrake, btnBrake, lblLeftCurve, btnLeftCurve, lblRightCurve, btnRightCurve, btnOK, btnCancel });
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SettingsForm";
            Text = "Settings";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
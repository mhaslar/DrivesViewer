// Views/MainForm.Designer.cs
using System.Windows.Forms;
using DrivesViewer.Controls;

namespace DrivesViewer.Views
{
    partial class MainForm
    {
        private RecordingSelectorControl recordingSelectorControl;
        private DriveDataViewerControl driveViewer;
        private PointDetailControl pointDetail;
        private RadioButton rbCurve;
        private RadioButton rbSpeed;
        private Panel panelMiddle;
        private FlowLayoutPanel panelModes;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem settingsToolStripMenuItem;

        private void InitializeComponent()
        {
            this.menuStrip1 = new MenuStrip();
            this.settingsToolStripMenuItem = new ToolStripMenuItem();
            this.recordingSelectorControl = new RecordingSelectorControl();
            this.panelMiddle = new Panel();
            this.panelModes = new FlowLayoutPanel();
            this.rbCurve = new RadioButton();
            this.rbSpeed = new RadioButton();
            this.driveViewer = new DriveDataViewerControl();
            this.pointDetail = new PointDetailControl();

            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new ToolStripItem[] {
                this.settingsToolStripMenuItem
            });
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
            this.settingsToolStripMenuItem.Text = "Nastavení";
            // 
            // recordingSelectorControl
            // 
            this.recordingSelectorControl.Dock = DockStyle.Left;
            this.recordingSelectorControl.Width = 200;
            this.recordingSelectorControl.Location = new System.Drawing.Point(0, 24);
            this.recordingSelectorControl.TabIndex = 1;
            // 
            // panelMiddle
            // 
            this.panelMiddle.Dock = DockStyle.Fill;
            this.panelMiddle.Location = new System.Drawing.Point(200, 24);
            this.panelMiddle.Name = "panelMiddle";
            this.panelMiddle.TabIndex = 2;
            // 
            // panelModes
            // 
            this.panelModes.Dock = DockStyle.Top;
            this.panelModes.Height = 30;
            this.panelModes.FlowDirection = FlowDirection.LeftToRight;
            this.panelModes.Controls.Add(this.rbCurve);
            this.panelModes.Controls.Add(this.rbSpeed);
            this.panelModes.Location = new System.Drawing.Point(0, 0);
            this.panelModes.Name = "panelModes";
            this.panelModes.TabIndex = 0;
            // 
            // rbCurve
            // 
            this.rbCurve.AutoSize = true;
            this.rbCurve.Checked = true;
            this.rbCurve.Text = "Zatáčky";
            // 
            // rbSpeed
            // 
            this.rbSpeed.AutoSize = true;
            this.rbSpeed.Text = "Rychlost";
            // 
            // driveViewer
            // 
            this.driveViewer.Dock = DockStyle.Fill;
            this.driveViewer.Location = new System.Drawing.Point(0, 30);
            this.driveViewer.Name = "driveViewer";
            this.driveViewer.TabIndex = 1;
            // 
            // pointDetail
            // 
            this.pointDetail.Dock = DockStyle.Right;
            this.pointDetail.Width = 200;
            this.pointDetail.Location = new System.Drawing.Point(600, 24);
            this.pointDetail.TabIndex = 3;
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.pointDetail);
            this.Controls.Add(this.panelMiddle);
            this.Controls.Add(this.recordingSelectorControl);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Drives Viewer";
            this.WindowState = FormWindowState.Maximized;

            // přidáme do panelMiddle nejprve panelModes a poté driveViewer
            this.panelMiddle.Controls.Add(this.driveViewer);
            this.panelMiddle.Controls.Add(this.panelModes);

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}

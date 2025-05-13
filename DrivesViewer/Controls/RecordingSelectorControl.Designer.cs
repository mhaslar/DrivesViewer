using System.Windows.Forms;

namespace DrivesViewer.Controls
{
    partial class RecordingSelectorControl
    {
        private System.ComponentModel.IContainer components = null;
        private ComboBox comboBox1;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            comboBox1 = new ComboBox();
            SuspendLayout();
            // comboBox1
            comboBox1.Dock = DockStyle.Top;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.FormattingEnabled = true;
            comboBox1.Name = "comboBox1";
            // RecordingSelectorControl
            Controls.Add(comboBox1);
            Name = "RecordingSelectorControl";
            Size = new System.Drawing.Size(200, 30);
            ResumeLayout(false);
        }
    }
}
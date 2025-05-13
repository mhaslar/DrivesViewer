// Controls/PointDetailControl.Designer.cs
namespace DrivesViewer.Controls
{
    partial class PointDetailControl
    {
        /// <summary> 
        /// Požadovaná proměnná návrháře.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Uvolní všechny používané prostředky.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary> 
        /// Metoda vyžadovaná pro podporu návrháře - 
        /// neupravovat obsah této metody v editoru kódu.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // PointDetailControl
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Name = "PointDetailControl";
            this.ResumeLayout(false);
        }
    }
}

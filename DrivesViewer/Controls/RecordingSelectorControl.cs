using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DrivesViewer.Models;

namespace DrivesViewer.Controls
{
    public partial class RecordingSelectorControl : UserControl
    {
        public event Action<int> RecordingChanged;
        public RecordingSelectorControl()
        {
            InitializeComponent();
            comboBox1.SelectedIndexChanged += (s, e) => OnSelectionChanged();
        }
        public void SetRecordings(List<Recording> recs)
        {
            comboBox1.DisplayMember = nameof(Recording.StartDateTime);
            comboBox1.ValueMember = nameof(Recording.Id);
            comboBox1.DataSource = recs;
        }
        private void OnSelectionChanged()
        {
            if (comboBox1.SelectedItem is Recording rec)
                RecordingChanged?.Invoke(rec.Id);
        }
    }
}
using System;
using System.Collections.Generic;

namespace DrivesViewer.Models
{
    public class Recording
    {
        public int Id { get; set; }
        public DateTime StartDateTime { get; set; }
        public string SensorsDeviceName { get; set; }
    }
}
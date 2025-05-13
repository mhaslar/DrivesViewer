using System;

namespace DrivesViewer.Models
{
    public class DriveDataPoint
    {
        public int RecordingId { get; set; }
        public int TimeFromStartSeconds { get; set; }
        public double? LatRec { get; set; }
        public double? LonRec { get; set; }
        public double SpeedRec { get; set; }
        public bool InCurve { get; set; }
        public double Roll { get; set; }
        public double Ax { get; set; }
        public char CircuitCurveTurnDirection { get; set; }
    }
}
using System.Drawing;
using DrivesViewer.Properties;

namespace DrivesViewer.Utils
{
    public static class SettingsManager
    {
        public static bool AutoRedraw
        {
            get => Settings.Default.AutoRedraw;
            set { Settings.Default.AutoRedraw = value; Settings.Default.Save(); }
        }
        public static Color ColorSpeed
        {
            get => Settings.Default.ColorSpeed;
            set { Settings.Default.ColorSpeed = value; Settings.Default.Save(); }
        }
        public static Color ColorBrake
        {
            get => Settings.Default.ColorBrake;
            set { Settings.Default.ColorBrake = value; Settings.Default.Save(); }
        }
        public static Color ColorLeftCurve
        {
            get => Settings.Default.ColorLeftCurve;
            set { Settings.Default.ColorLeftCurve = value; Settings.Default.Save(); }
        }
        public static Color ColorRightCurve
        {
            get => Settings.Default.ColorRightCurve;
            set { Settings.Default.ColorRightCurve = value; Settings.Default.Save(); }
        }
    }
}
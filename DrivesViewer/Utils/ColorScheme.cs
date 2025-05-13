using System.Drawing;

namespace DrivesViewer.Utils
{
    public static class ColorScheme
    {
        public static Color Speed => SettingsManager.ColorSpeed;
        public static Color Brake => SettingsManager.ColorBrake;
        public static Color LeftCurve => SettingsManager.ColorLeftCurve;
        public static Color RightCurve => SettingsManager.ColorRightCurve;
    }
}
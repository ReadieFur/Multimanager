using Microsoft.Win32;
using System.Windows;
using System.Windows.Media;
using System.Timers;
using System.Windows.Controls;
using System;
using System.Windows.Data;
using System.Globalization;

namespace Multimanager
{
    internal class XAMLStyles
    {
        #region TEMPLATE
        #region Solids
        public string backgroundSolid { get; set; } = Styles.background;

        public string foregroundSolid { get; set; } = Styles.foreground;

        public string accentSolid { get; set; } = Styles.accent;

        public string borderSolid { get; set; } = Styles.border;
        #endregion

        #region Linear Gradients
        #region Black And White
        //Horizontal
        public LinearGradientBrush foregroundFadeRight { get; set; } = foregroundFadeRightGradient();
        private static LinearGradientBrush foregroundFadeRightGradient()
        {
            LinearGradientBrush linearGradientBrush = new LinearGradientBrush();
            linearGradientBrush.StartPoint = new Point(0, 0.5);
            linearGradientBrush.EndPoint = new Point(1, 0.5);
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.foreground), Offset = 0 });
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.foreground), Offset = 0.3 });
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.background), Offset = 0.7 });
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.background), Offset = 1 });
            return linearGradientBrush;
        }

        public LinearGradientBrush foregroundFadeLeft { get; set; } = foregroundFadeLeftGradient();
        private static LinearGradientBrush foregroundFadeLeftGradient()
        {
            LinearGradientBrush linearGradientBrush = new LinearGradientBrush();
            linearGradientBrush.StartPoint = new Point(0, 0.5);
            linearGradientBrush.EndPoint = new Point(1, 0.5);
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.background), Offset = 0 });
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.background), Offset = 0.3 });
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.foreground), Offset = 0.7 });
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.foreground), Offset = 1 });
            return linearGradientBrush;
        }

        public LinearGradientBrush foregroundFadeLeftRight { get; set; } = foregroundFadeLeftRightGradient();
        private static LinearGradientBrush foregroundFadeLeftRightGradient()
        {
            LinearGradientBrush linearGradientBrush = new LinearGradientBrush();
            linearGradientBrush.StartPoint = new Point(0, 0.5);
            linearGradientBrush.EndPoint = new Point(1, 0.5);
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.background), Offset = 0 });
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.foreground), Offset = 0.3 });
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.foreground), Offset = 0.7 });
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.background), Offset = 1 });
            return linearGradientBrush;
        }

        //Vertical
        public LinearGradientBrush foregroundFadeBottom { get; set; } = foregroundFadeBottomGradient();
        private static LinearGradientBrush foregroundFadeBottomGradient()
        {
            LinearGradientBrush linearGradientBrush = new LinearGradientBrush();
            linearGradientBrush.StartPoint = new Point(0.5, 0);
            linearGradientBrush.EndPoint = new Point(0.5, 1);
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.foreground), Offset = 0 });
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.foreground), Offset = 0.3 });
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.background), Offset = 0.7 });
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.background), Offset = 1 });
            return linearGradientBrush;
        }

        public LinearGradientBrush foregroundFadeTop { get; set; } = foregroundFadeTopGradient();
        private static LinearGradientBrush foregroundFadeTopGradient()
        {
            LinearGradientBrush linearGradientBrush = new LinearGradientBrush();
            linearGradientBrush.StartPoint = new Point(0.5, 0);
            linearGradientBrush.EndPoint = new Point(0.5, 1);
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.background), Offset = 0 });
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.background), Offset = 0.3 });
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.foreground), Offset = 0.7 });
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.foreground), Offset = 1 });
            return linearGradientBrush;
        }

        public LinearGradientBrush foregroundFadeTopBottom { get; set; } = foregroundFadeTopBottomGradient();
        private static LinearGradientBrush foregroundFadeTopBottomGradient()
        {
            LinearGradientBrush linearGradientBrush = new LinearGradientBrush();
            linearGradientBrush.StartPoint = new Point(0.5, 0);
            linearGradientBrush.EndPoint = new Point(0.5, 1);
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.background), Offset = 0 });
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.foreground), Offset = 0.3 });
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.foreground), Offset = 0.7 });
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.background), Offset = 1 });
            return linearGradientBrush;
        }
        #endregion

        #region Accents
        //Horizontal
        public LinearGradientBrush accentFadeRight { get; set; } = accentFadeRightGradient();
        private static LinearGradientBrush accentFadeRightGradient()
        {
            LinearGradientBrush linearGradientBrush = new LinearGradientBrush();
            linearGradientBrush.StartPoint = new Point(0, 0.5);
            linearGradientBrush.EndPoint = new Point(1, 0.5);
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.accent), Offset = 0 });
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.accent), Offset = 0.3 });
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.background), Offset = 0.7 });
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.background), Offset = 1 });
            return linearGradientBrush;
        }

        public LinearGradientBrush accentFadeLeft { get; set; } = accentFadeLeftGradient();
        private static LinearGradientBrush accentFadeLeftGradient()
        {
            LinearGradientBrush linearGradientBrush = new LinearGradientBrush();
            linearGradientBrush.StartPoint = new Point(0, 0.5);
            linearGradientBrush.EndPoint = new Point(1, 0.5);
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.background), Offset = 0 });
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.background), Offset = 0.3 });
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.accent), Offset = 0.7 });
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.accent), Offset = 1 });
            return linearGradientBrush;
        }

        //Vertical
        public LinearGradientBrush accentFadeBottom { get; set; } = accentFadeBottomGradient();
        private static LinearGradientBrush accentFadeBottomGradient()
        {
            LinearGradientBrush linearGradientBrush = new LinearGradientBrush();
            linearGradientBrush.StartPoint = new Point(0.5, 0);
            linearGradientBrush.EndPoint = new Point(0.5, 1);
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.accent), Offset = 0 });
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.accent), Offset = 0.3 });
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.background), Offset = 0.7 });
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.background), Offset = 1 });
            return linearGradientBrush;
        }

        public LinearGradientBrush accentFadeTop { get; set; } = accentFadeTopGradient();
        private static LinearGradientBrush accentFadeTopGradient()
        {
            LinearGradientBrush linearGradientBrush = new LinearGradientBrush();
            linearGradientBrush.StartPoint = new Point(0.5, 0);
            linearGradientBrush.EndPoint = new Point(0.5, 1);
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.background), Offset = 0 });
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.background), Offset = 0.3 });
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.accent), Offset = 0.7 });
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString(Styles.accent), Offset = 1 });
            return linearGradientBrush;
        }
        #endregion
        #endregion
        #endregion
    }

    static class Styles
    {
        #region TEMPLATE
        public static bool themeChanged = false;
        public static string background = "#FFFFFFFF"; //OLDTheme
        public static string foreground = "#FF000000"; //OLDText
        public static string accent = "#FF0078D7";
        public static string border = "#FFDDDDDD"; //OLDButton

        public static Brush bc(string colour) { return (Brush)new BrushConverter().ConvertFrom(colour); }

        public static void checkForChange()
        {
            getStyles();
            Timer cfc = new Timer();
            cfc.Interval = 2500;
            cfc.Elapsed += (se, ea) =>
            {
                string fetchedAccent = SystemParameters.WindowGlassBrush.ToString();
                string fetchedBackground = "#FFFFFFFF";

                try
                {
                    if (Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize").GetValue("AppsUseLightTheme").ToString() == "0") //Dark theme
                    { fetchedBackground = "#FF101011"; }

                    if ((background != fetchedBackground) || (accent != fetchedAccent)) { getStyles(); themeChanged = true; }
                    else { themeChanged = false; }
                }
                catch { themeChanged = false; }
            };
            cfc.Start();
        }

        private static void getStyles()
        {
            accent = SystemParameters.WindowGlassBrush.ToString();
            try
            {
                if (Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize").GetValue("AppsUseLightTheme").ToString() == "0") //Dark theme
                {
                    background = "#FF101011";
                    foreground = "#FFFFFFFF";
                    border = "#FF383838";
                }
                else
                {
                    background = "#FFFFFFFF";
                    foreground = "#FF000000";
                    border = "#FFDDDDDD";
                }
            }
            catch { }
        }
        #endregion
    }

    internal class StarWidthConverter : IValueConverter
    {
        #region TEMPLATE
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ListView listview = value as ListView;
            double width = listview.Width;
            GridView gv = listview.View as GridView;
            for (int i = 0; i < gv.Columns.Count; i++)
            {
                if (!double.IsNaN(gv.Columns[i].Width))
                    width -= gv.Columns[i].Width;
            }
            return width - 99;// this is to take care of margin/padding
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
        #endregion
    }
}

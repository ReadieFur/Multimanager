using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Multimanager
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            #region Styles
            appName.Foreground = Styles.text();
            appVersion.Foreground = Styles.text();
            author.Foreground = Styles.text();
            appNameLine.Stroke = Styles.gBWHorizontal;
            #endregion

            string[] appVersionSS = FileVersionInfo.GetVersionInfo(MainWindow.mmDIR + "Multimanager.dll").FileVersion.Split('.');
            appVersion.Text = $"v{appVersionSS[0]}.{appVersionSS[1]}.{appVersionSS[2]}";

            List<update> updateInfo = JsonConvert.DeserializeObject<List<update>>(MainWindow.updateInfoJSON);
            foreach (var updateText in updateInfo)
            {
                Grid grid = new Grid();
                grid.Margin = new Thickness(15, 15, 0, 0);
                grid.Width = 150;
                grid.Height = 210;

                Rectangle rectangle = new Rectangle();
                rectangle.Fill = Styles.button();
                rectangle.RadiusX = 10;
                rectangle.RadiusY = 10;

                WrapPanel wrappanel = new WrapPanel();
                wrappanel.Width = 150;
                wrappanel.Orientation = Orientation.Vertical;
                wrappanel.HorizontalAlignment = HorizontalAlignment.Left;
                wrappanel.VerticalAlignment = VerticalAlignment.Top;

                TextBlock Title = new TextBlock();
                Title.Margin = new Thickness(10, 10, 0, 0);
                Title.Width = 130;
                Title.Text = updateText.title;
                Title.FontFamily = new FontFamily("Century Gothic");
                Title.FontSize = 18;
                Title.Foreground = Styles.text();

                TextBlock body = new TextBlock();
                body.Margin = new Thickness(10, 5, 0, 0);
                body.Width = 130;
                body.Height = double.NaN;
                body.TextWrapping = TextWrapping.Wrap;
                body.Text = updateText.body;
                body.FontFamily = new FontFamily("Century Gothic");
                body.FontSize = 12;
                body.Foreground = Styles.text();

                TextBlock version = new TextBlock();
                version.Margin = new Thickness(10, 5, 0, 0);
                version.Width = 130;
                version.Text = "v" + updateText.version;
                version.FontFamily = new FontFamily("Century Gothic");
                version.FontSize = 10;
                version.Foreground = Styles.text();

                wrappanel.Children.Add(Title);
                wrappanel.Children.Add(body);
                wrappanel.Children.Add(version);
                grid.Children.Add(rectangle);
                grid.Children.Add(wrappanel);
                updateWrapPanel.Children.Insert(0, grid);
            }
        }
    }

    class update
    {
        public string version { get; set; }
        public string title { get; set; }
        public string body { get; set; }
    }
}

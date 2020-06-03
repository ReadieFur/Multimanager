using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Multimanager.SystemPages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        private MainWindow mw = Application.Current.MainWindow as MainWindow;

        public Home()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            mw.backButton.Visibility = Visibility.Hidden;
            Timer checkForChange = new Timer();
            DataContext = new XAMLStyles { };
            checkForChange.Interval = 1000;
            checkForChange.Elapsed += (se, ea) => { try { if (Styles.themeChanged) { Dispatcher.Invoke(() => { DataContext = new XAMLStyles { }; themeChanged(); }); } } catch { } };
            checkForChange.Start();
            themeChanged();
        }

        private void themeChanged()
        {
            if (Styles.background != "#FFFFFFFF")
            {
                displayImage.Source = new BitmapImage(new Uri($"../Resources/DisplayBlack.png", UriKind.Relative));
                personalizationImage.Source = new BitmapImage(new Uri($"../Resources/PenBlack.png", UriKind.Relative));
            }
            else
            {
                displayImage.Source = new BitmapImage(new Uri($"../Resources/DisplayWhite.png", UriKind.Relative));
                personalizationImage.Source = new BitmapImage(new Uri($"../Resources/PenWhite.png", UriKind.Relative));
            }
        }

        private void personalizationButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.wcPage = this;
            mw.backButton.Visibility = Visibility.Visible;
            mw.windowContent.Content = new Themes();
        }

        private void displayButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.wcPage = this;
            mw.backButton.Visibility = Visibility.Visible;
            mw.windowContent.Content = new Display();
        }
    }
}

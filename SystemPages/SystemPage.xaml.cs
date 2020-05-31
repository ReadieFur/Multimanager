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
    /// Interaction logic for SystemPage.xaml
    /// </summary>
    public partial class SystemPage : Page
    {
        public MainWindow mw = null;

        public SystemPage(MainWindow tw = null)
        {
            if (tw != null) { mw = tw; }
            InitializeComponent();
        }

        Timer themeChecker = new Timer();

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            systemPagesContent.Content = null;
            systemPagesContent.Visibility = Visibility.Hidden;
            backButton.Visibility = Visibility.Hidden;

            updateThemes();
        }

        public void updateThemes()
        {
            Dispatcher.Invoke(() =>
            {
                string displayButtonIcon;
                string personalizationButtonIcon;
                if (Styles.AppsUseLightTheme == "#FFFFFFFF")
                {
                    displayButtonIcon = $"/Multimanager;component/Resources/DisplayWhite.png";
                    personalizationButtonIcon = $"/Multimanager;component/Resources/PenWhite.png";
                }
                else
                {
                    displayButtonIcon = $"/Multimanager;component/Resources/DisplayBlack.png";
                    personalizationButtonIcon = $"/Multimanager;component/Resources/PenBlack.png";
                }

                pageTitle.Foreground = Styles.text();
                pageTitleLine.Stroke = Styles.gBWHorizontal;
                backButton.Foreground = Styles.text();

                //Display button            
                displayImage.Source = new BitmapImage(new Uri(displayButtonIcon, UriKind.Relative));
                displayText.Foreground = Styles.text();
                displayAccent.Background = Styles.accent();

                //Personalization button
                personalizationImage.Source = new BitmapImage(new Uri(personalizationButtonIcon, UriKind.Relative));
                personalizationText.Foreground = Styles.text();
                personalizationAccent.Background = Styles.accent();
            });
        }

        private void displayButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                systemPagesNavigation.Visibility = Visibility.Hidden;
                systemPagesContent.Visibility = Visibility.Visible;
                backButton.Visibility = Visibility.Visible;
                systemPagesContent.Content = new Display();
                pageTitle.Text = "Display";
            }
        }

        private void personalizationButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                systemPagesNavigation.Visibility = Visibility.Hidden;
                systemPagesContent.Visibility = Visibility.Visible;
                backButton.Visibility = Visibility.Visible;
                systemPagesContent.Content = new Themes(this);
                pageTitle.Text = "Personalization";

                themeChecker.Interval = 100;
                themeChecker.Elapsed += ThemeChecker_Elapsed;
                themeChecker.Start();
            }
        }

        private void ThemeChecker_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Themes.themeChangeCountOld != Themes.themeChangeCountNew)
            {
                Themes.themeChangeCountOld = Themes.themeChangeCountNew;
                updateThemes();
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            systemPagesContent.Content = null;
            systemPagesContent.Visibility = Visibility.Hidden;
            backButton.Visibility = Visibility.Hidden;
            systemPagesNavigation.Visibility = Visibility.Visible;
            systemPagesContent.Content = null;
            pageTitle.Text = "System";
            themeChecker.Stop();
        }
    }
}

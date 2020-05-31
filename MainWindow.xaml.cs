using Microsoft.WindowsAPICodePack.Taskbar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Multimanager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Shared data
        public static string updateInfoJSON;
        public static string mmDIR;
        #endregion

        #region MISC
        Timer winAero = new Timer();

        public MainWindow(string e)
        {
            getWorkingDIR();
            Styles.getStyles();
            InitializeComponent();
            SourceInitialized += (s, ev) => { TaskbarManager.Instance.SetApplicationIdForSpecificWindow(new WindowInteropHelper(this).Handle, "Multimanager"); };

            MinWidth = 800;
            MinHeight = 450;
            Height = 450;
            Width = 800;
            ResizeMode = ResizeMode.CanResizeWithGrip;
            windowBorder.Visibility = Visibility.Visible;

            processesGrid.Visibility = Visibility.Collapsed;

            setMainWindowThemes();
        }

        private void getWorkingDIR()
        {
            List<string> paths = new List<string>();
            foreach (string f in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory))
            {
                paths.Add(f);
            }

            foreach (string d in Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory))
            {
                foreach (string f in Directory.GetFiles(d))
                {
                    paths.Add(f);
                }
            }

            string dir = string.Empty;

            foreach (string s in paths)
            {
                if (s.Contains("Multimanager.dll"))
                {
                    List<string> path = s.Split('\\').ToList();
                    path.RemoveAt(path.Count - 1);
                    foreach (string d in path)
                    {
                        dir = dir + d + "\\";
                    }
                    break;
                }
                else { }
            }

            mmDIR = dir;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            System.Net.WebClient webclient = new System.Net.WebClient();
            updateInfoJSON = webclient.DownloadString("https://kofr.000webhostapp.com/apps/multimanager/updateInfo.json");

            winAero.Interval = 10;
            winAero.Elapsed += checkForAeroFC;
            winAero.Start();

            homeBorder.BorderBrush = Styles.accent();
            if (Styles.AppsUseLightTheme == "#FFFFFFFF") { homeGrid.Background = Styles.b("#33000000"); }
            else { homeGrid.Background = Styles.b("#33FFFFFF"); }

            windowContent.Content = new Home();

            System.Threading.Thread.Sleep(50);
            Activate();
        }

        private void checkForAeroFC(object sender, ElapsedEventArgs e)
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    if (WindowState == WindowState.Maximized)
                    {
                        WindowState = WindowState.Normal;
                        Top = 0;
                        Left = 0;
                        Width = SystemParameters.WorkArea.Width;
                        Height = SystemParameters.WorkArea.Height;
                        resizebtn.Content = "\uE923";
                        windowBorder.Visibility = Visibility.Hidden;
                    }
                    else if (Width != SystemParameters.WorkArea.Width && Height != SystemParameters.WorkArea.Height)
                    {
                        resizebtn.Content = "\uE922";
                        windowBorder.Visibility = Visibility.Visible;
                    }

                    if (Height > SystemParameters.WorkArea.Height)
                    {
                        Height = SystemParameters.WorkArea.Height;
                    }
                });
            } catch { }
        }

        private void windowHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (Width == SystemParameters.WorkArea.Width && Height == SystemParameters.WorkArea.Height)
                {
                    windowBorder.Visibility = Visibility.Visible;
                    Top = System.Windows.Forms.Control.MousePosition.Y - 15;
                    Left = System.Windows.Forms.Control.MousePosition.X - 400;
                    Width = 800;
                    Height = 450;
                    resizebtn.Content = "\uE922";
                    DragMove();
                }
                else if (e.ClickCount == 2)
                {
                    Top = 0;
                    Left = 0;
                    Width = SystemParameters.WorkArea.Width;
                    Height = SystemParameters.WorkArea.Height;
                    resizebtn.Content = "\uE923";
                    windowBorder.Visibility = Visibility.Hidden;
                }
                else
                {
                    DragMove();
                }
            }
        }
        #endregion

        #region Styles
        public void setMainWindowThemes()
        {
            windowContents.Background = Styles.theme();
            windowHeader.Background = Styles.theme();
            background.Background = Styles.theme();
            minimisebtn.Background = Styles.theme();
            resizebtn.Background = Styles.theme();
            closebtn.Background = Styles.theme();
            closebtn.Foreground = Styles.text();
            resizebtn.Foreground = Styles.text();
            minimisebtn.Foreground = Styles.text();
            appTitle.Foreground = Styles.text();
            windowBorder.BorderBrush = Styles.accent();

            #region Navigation buttons
            string homeBTN;
            string processesBTN;
            string systemBTN;
            string consoleBTN;
            if (Styles.AppsUseLightTheme == "#FFFFFFFF")
            {
                homeBTN = "HomeBlack";
                processesBTN = "ProcessesBlack";
                systemBTN = "SystemBlack";
                consoleBTN = "ConsoleBlack";
            }
            else
            {
                homeBTN = "HomeWhite";
                processesBTN = "ProcessesWhite";
                systemBTN = "SystemWhite";
                consoleBTN = "ConsoleWhite";
            }

            //Home button
            homeImage.Source = new BitmapImage(new Uri($"Resources/{homeBTN}.png", UriKind.Relative));
            homeText.Foreground = Styles.text();

            //Processes button
            processesImage.Source = new BitmapImage(new Uri($"Resources/{processesBTN}.png", UriKind.Relative));
            processesText.Foreground = Styles.text();

            //System button
            systemImage.Source = new BitmapImage(new Uri($"Resources/{systemBTN}.png", UriKind.Relative));
            systemText.Foreground = Styles.text();

            //Console button
            consoleImage.Source = new BitmapImage(new Uri($"Resources/{consoleBTN}.png", UriKind.Relative));
            consoleText.Foreground = Styles.text();
            #endregion

            //Misc
            horizontalLine.Stroke = Styles.gHorizontal;
            verticalLine.Stroke = Styles.gVertical;
        }
        #endregion

        #region Window buttons
        private void closebtn_Click(object sender, RoutedEventArgs e)
        {
            winAero.Stop();
            Close();
        }

        private void resizebtn_Click(object sender, RoutedEventArgs e)
        {
            if (Height != SystemParameters.WorkArea.Height && Width != SystemParameters.WorkArea.Width)
            {
                Top = 0;
                Left = 0;
                Height = SystemParameters.WorkArea.Height;
                Width = SystemParameters.WorkArea.Width;
                windowBorder.Visibility = Visibility.Hidden;
                resizebtn.Content = "\uE923";
            }
            else
            {
                WindowState = WindowState.Normal;
                Height = 450;
                Width = 800;
                Top = SystemParameters.WorkArea.Height / 4;
                Left = SystemParameters.WorkArea.Width / 4;
                windowBorder.Visibility = Visibility.Visible;
                resizebtn.Content = "\uE922";
            }
        }

        private void minimisebtn_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        #endregion

        #region Navigation bar buttons
        private void homeGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                string shade = "#33FFFFFF";
                if (Styles.AppsUseLightTheme == "#FFFFFFFF") { shade = "#33000000"; }

                //Set this button to toggled
                homeGrid.Background = Styles.b(shade);
                homeBorder.BorderBrush = Styles.accent();

                //Reset other buttons
                processesGrid.Background = Styles.b("#00000000");
                processesBorder.BorderBrush = Styles.b("#00000000");
                systemGrid.Background = Styles.b("#00000000");
                systemBorder.BorderBrush = Styles.b("#00000000");
                consoleGrid.Background = Styles.b("#00000000");
                consoleBorder.BorderBrush = Styles.b("#00000000");

                //Set frame content
                windowContent.Content = new Home();
            }
        }

        private void processesGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string shade = "#33FFFFFF";
            if (Styles.AppsUseLightTheme == "#FFFFFFFF") { shade = "#33000000"; }

            //Set this button to toggled
            processesGrid.Background = Styles.b(shade);
            processesBorder.BorderBrush = Styles.accent();

            //Reset other buttons
            homeGrid.Background = Styles.b("#00000000");
            homeBorder.BorderBrush = Styles.b("#00000000");
            systemGrid.Background = Styles.b("#00000000");
            systemBorder.BorderBrush = Styles.b("#00000000");
            consoleGrid.Background = Styles.b("#00000000");
            consoleBorder.BorderBrush = Styles.b("#00000000");

            //Set frame content
            //windowContent.Content = new TaskManager();
        }

        private void systemGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string shade = "#33FFFFFF";

            if (Styles.AppsUseLightTheme == "#FFFFFFFF") { shade = "#33000000"; }

            //Set this button to toggled
            systemGrid.Background = Styles.b(shade);
            systemBorder.BorderBrush = Styles.accent();

            //Reset other buttons
            homeGrid.Background = Styles.b("#00000000");
            homeBorder.BorderBrush = Styles.b("#00000000");
            processesGrid.Background = Styles.b("#00000000");
            processesBorder.BorderBrush = Styles.b("#00000000");
            consoleGrid.Background = Styles.b("#00000000");
            consoleBorder.BorderBrush = Styles.b("#00000000");

            //Set frame content
            windowContent.Content = new SystemPages.SystemPage(this);
        }

        private void consoleGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string shade = "#33FFFFFF";
            if (Styles.AppsUseLightTheme == "#FFFFFFFF") { shade = "#33000000"; }

            //Set this button to toggled
            consoleGrid.Background = Styles.b(shade);
            consoleBorder.BorderBrush = Styles.accent();

            //Reset other buttons
            homeGrid.Background = Styles.b("#00000000");
            homeBorder.BorderBrush = Styles.b("#00000000");
            processesGrid.Background = Styles.b("#00000000");
            processesBorder.BorderBrush = Styles.b("#00000000");
            systemGrid.Background = Styles.b("#00000000");
            systemBorder.BorderBrush = Styles.b("#00000000");

            //Set frame content
            windowContent.Content = new Console();
        }
        #endregion
    }
}

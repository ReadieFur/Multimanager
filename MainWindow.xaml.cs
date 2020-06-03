using Microsoft.WindowsAPICodePack.Taskbar;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Multimanager
{
    public partial class MainWindow : Window
    {
        internal static Page wcPage = null;

        #region TEMPLATE
        bool allowClose = true;
        Timer winAero = new Timer();
        Timer checkForChange = new Timer();
        double previousWidth = 0;
        double previousHeight = 0;
        double previousTop = 0;
        double previousLeft = 0;

        public MainWindow(string args)
        {
            InitializeComponent();
            string[] FVI = FileVersionInfo.GetVersionInfo(currentDirectory + baseFileName).FileVersion.Split('.');
            release.Content = $"Release: {FVI[0]}.{FVI[1]}.{FVI[2]}";
            if (taskbarGroup != string.Empty)
            { SourceInitialized += (s, ev) => { TaskbarManager.Instance.SetApplicationIdForSpecificWindow(new WindowInteropHelper(this).Handle, taskbarGroup); }; }
            windowBorder.Visibility = Visibility.Visible;
            appTitle.Content = windowTitle;
            if (!allowResize) { resizebtn.Visibility = Visibility.Collapsed; ResizeMode = ResizeMode.NoResize; }
            previousWidth = defaultWidth;
            previousHeight = defaultHeight;
            Width = defaultWidth;
            Height = defaultHeight;
            previousTop = Top;
            previousLeft = Left;
            if (minWidth > 0) { MinWidth = minWidth; }
            if (minHeight > 0) { MinHeight = minHeight; }
            if (maxWidth > 0) { MaxWidth = maxWidth; }
            if (maxHeight > 0) { MaxHeight = maxHeight; }
            WindowStartup(args);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            winAero.Interval = 10;
            winAero.Elapsed += checkForAeroFC;
            winAero.Start();

            DataContext = new XAMLStyles { };
            checkForChange.Interval = 1000;
            checkForChange.Elapsed += (se, ea) => { try { if (Styles.themeChanged) { Dispatcher.Invoke(() => { DataContext = new XAMLStyles { }; themeChanged(); }); } } catch { } };
            checkForChange.Start();

            windowLoaded();
        }

        #region Window functions
        private void topBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (Width == SystemParameters.WorkArea.Width && Height == SystemParameters.WorkArea.Height && allowResize == true)
                {
                    windowBorder.Visibility = Visibility.Visible;
                    Top = System.Windows.Forms.Control.MousePosition.Y - 15;
                    Left = System.Windows.Forms.Control.MousePosition.X - 400;
                    Width = previousWidth;
                    Height = previousHeight;
                    resizebtn.Content = "\uE922";
                    DragMove();
                }
                else if (e.ClickCount == 2 && allowResize == true)
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
                    previousWidth = Width;
                    previousHeight = Height;
                    previousTop = Top;
                    previousLeft = Left;
                }
            }
        }

        private void closebtn_Click(object sender, RoutedEventArgs e) { allowClose = true; Close(); }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (allowClose == false)
            {
                disallowClosing();
                e.Cancel = true;
            }
            else
            {
                winAero.Stop();
                allowClosing();
                e.Cancel = false;
            }
        }

        private void resizebtn_Click(object sender, RoutedEventArgs e)
        {
            if (Height != SystemParameters.WorkArea.Height && Width != SystemParameters.WorkArea.Width)
            {
                previousWidth = Width;
                previousHeight = Height;
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
                Width = previousWidth;
                Height = previousHeight;
                Top = previousTop;
                Left = previousLeft;
                windowBorder.Visibility = Visibility.Visible;
                resizebtn.Content = "\uE922";
            }
        }

        private void minimisebtn_Click(object sender, RoutedEventArgs e) { WindowState = WindowState.Minimized; }

        private void checkForAeroFC(object sender, ElapsedEventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (allowResize)
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

                        if (Height > SystemParameters.WorkArea.Height) { Height = SystemParameters.WorkArea.Height; }
                    }
                    else
                    {
                        if (WindowState == WindowState.Maximized)
                        {
                            WindowState = WindowState.Normal;
                            Top = previousTop;
                            Left = previousLeft;
                            Width = defaultWidth;
                            Height = defaultHeight;
                            resizebtn.Content = "\uE923";
                            windowBorder.Visibility = Visibility.Visible;
                        }
                    }
                });
            }
            catch { }
        }
        #endregion
        #endregion

        #region TEMPLATE MODIFIERS
        string windowTitle = "Multimanager";
        string baseFileName = "Multimanager.dll";
        string currentDirectory = Environment.CurrentDirectory + "\\";
        string taskbarGroup = "Multimanager";
        bool allowResize = true;
        double defaultWidth = 1020;
        double defaultHeight = 450;
        double minWidth = 1020; //0 = No minimum
        double minHeight = 450; //0 = No minimum
        double maxWidth = SystemParameters.WorkArea.Width; //0 = No maximum
        double maxHeight = SystemParameters.WorkArea.Height; //0 = No maximum

        private void WindowStartup(string args)
        {
            themeChanged();
        }

        private void windowLoaded()
        {

        }

        #region Window closing
        private void allowClosing()
        {

        }

        private void disallowClosing()
        {

        }
        #endregion

        #endregion

        #region TABS
        void resetAllTabs()
        {
            backButton.Visibility = Visibility.Hidden;
            wcPage = null;

            //SystemTab
            SystemTab.Background = Styles.bc(Styles.background);
            SystemTabBorder.Visibility = Visibility.Hidden;

            //ConsoleTab
            ConsoleTab.Background = Styles.bc(Styles.background);
            ConsoleTabBorder.Visibility = Visibility.Hidden;
        }

        private void SystemTab_MouseDown(object sender, MouseButtonEventArgs e)
        {
            resetAllTabs();
            SystemTab.Background = Styles.bc(Styles.border);
            SystemTabBorder.Visibility = Visibility.Visible;
            windowContent.Content = new SystemPages.Home();
        }

        private void ConsoleTab_MouseDown(object sender, MouseButtonEventArgs e)
        {
            resetAllTabs();
            ConsoleTab.Background = Styles.bc(Styles.border);
            ConsoleTabBorder.Visibility = Visibility.Visible;
            windowContent.Content = new UIConsole();
        }
        #endregion

        private void themeChanged()
        {
            if (Styles.background != "#FFFFFFFF")
            {
                SystemTabImage.Source = new BitmapImage(new Uri($"Resources/SystemWhite.png", UriKind.Relative));
                ConsoleTabImage.Source = new BitmapImage(new Uri($"Resources/ConsoleWhite.png", UriKind.Relative));
            }
            else
            {
                SystemTabImage.Source = new BitmapImage(new Uri($"Resources/SystemBlack.png", UriKind.Relative));
                ConsoleTabImage.Source = new BitmapImage(new Uri($"Resources/ConsoleBlack.png", UriKind.Relative));
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e) { windowContent.Content = wcPage; }
    }
}

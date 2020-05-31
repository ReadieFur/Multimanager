using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Forms;
using System.Windows.Media;
using Microsoft.WindowsAPICodePack.Taskbar;
using System.Windows.Interop;

namespace Multimanager
{
    /// <summary>
    /// Interaction logic for CMessageBox.xaml
    /// </summary>

    public class options
    {
        public enum b
        {
            ok = 0,
            okCancel = 1,
            yesNo = 2
        }
    }

    public class msgBox
    {
        public static string Show(string message, string title = "Multimanager alert", options.b buttons = options.b.ok, int time = 0)
        {
            var mbw = new CMessageBox(message, title, buttons, time);
            mbw.ShowDialog();
            return mbw.buttonPressed;
        }
    }

    public partial class CMessageBox : Window
    {
        public string buttonPressed;
        int timeR = 0;
        int uTime;
        Timer timeOpen = new Timer();

        private void setStyles()
        {
            windowTitle.Foreground = Styles.text();
            titleBar.Background = Styles.accent();
            windowBorder.BorderBrush = Styles.accent();
            messageBox.Foreground = Styles.text();
            messageBox.CaretBrush = Styles.text();
            background.Background = Styles.theme();
            btnYes.Background = Styles.button();
            btnCancel.Background = Styles.button();
            btnNo.Background = Styles.button();
            btnOk.Background = Styles.button();
            btnYes.Foreground = Styles.text();
            btnCancel.Foreground = Styles.text();
            btnNo.Foreground = Styles.text();
            btnOk.Foreground = Styles.text();
        }

        public CMessageBox(string message, string title = "Multimanager alert", options.b buttons = options.b.ok, int time = 0)
        {
            InitializeComponent();
            SourceInitialized += (s, ev) => { TaskbarManager.Instance.SetApplicationIdForSpecificWindow(new WindowInteropHelper(this).Handle, "Multimanager"); };

            try
            {
                MinHeight = 175;
                MinWidth = 400;

                setStyles();

                buttonPressed = "";
                timeR = 0;
                uTime = time + 2;
                if (time != 0) { timeR = 1; }

                /*try
                {
                    buttons = buttons.ToLower();
                    icon = icon.ToLower();
                }
                catch { }*/

                btnCancel.Visibility = Visibility.Collapsed;
                btnOk.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                btnYes.Visibility = Visibility.Collapsed;

                windowTitle.Content = title;
                messageBox.AppendText(message);

                //if (buttons != null)
                //{
                if (buttons == options.b.ok) { btnOk.Visibility = Visibility.Visible; }
                else if (buttons == options.b.okCancel) { btnOk.Visibility = Visibility.Visible; btnCancel.Visibility = Visibility.Visible; }
                else if (buttons == options.b.yesNo) { btnYes.Visibility = Visibility.Visible; btnNo.Visibility = Visibility.Visible; }
                //}         

                if (timeR != 0)
                {
                    timeOpen.Interval = 1000;
                    timeOpen.Tick += TimeOpen_Tick;
                    timeOpen.Start();
                }

                //SizeToContent = SizeToContent.WidthAndHeight;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"{ex.ToString()}\n\nPrevious error: {message}", "Fatal error | Previous error:" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                System.Windows.Application.Current.Shutdown(0x00040000);
            }
        }

        private void TimeOpen_Tick(object sender, EventArgs e)
        {
            if (timeR >= uTime)
            {
                buttonPressed = null;
                Close();
                timeOpen.Stop();
            }
            else
            {
                timeR += 1;
            }
        }

        private void titleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (titleBar.IsMouseOver == true)
            {
                if (e.ChangedButton == MouseButton.Left)
                {
                    DragMove();
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            buttonPressed = "cancel";
            Close();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            buttonPressed = "ok";
            Close();
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            buttonPressed = "no";
            Close();
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            buttonPressed = "yes";
            Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            buttonPressed = "close";
            Close();
        }
    }
}

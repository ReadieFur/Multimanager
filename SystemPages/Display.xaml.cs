using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Multimanager.SystemPages
{
    //Posts used:
    //https://stackoverflow.com/questions/30274071/how-to-get-the-column-value-of-the-selected-row-in-wpf-listview
    //https://www.codeproject.com/Articles/6810/Dynamic-Screen-Resolution
    //https://www.tutorialspoint.com/wpf/wpf_data_binding.htm

    public partial class Display : Page
    {
        public static int startScreenWidth = Convert.ToInt32(SystemParameters.PrimaryScreenWidth);
        public static int startScreenHeight = Convert.ToInt32(SystemParameters.PrimaryScreenHeight);
        public static string previousComboText;
        public static int revertSettings = 0;
        public static bool firstOpen = true;
        Timer updateComboResolutionsText = new Timer();

        public class DisplayItem
        {
            public string width { get; set; }
            public string height { get; set; }
            public string frequency { get; set; }
        }

        public Display()
        {
            InitializeComponent();
        }

        public void Page_Loaded(object sender, RoutedEventArgs e)
        {
            firstOpen = true;

            Timer checkForChange = new Timer();
            DataContext = new XAMLStyles { };
            checkForChange.Interval = 1000;
            checkForChange.Tick += (se, ea) => { try { if (Styles.themeChanged) { Dispatcher.Invoke(() => { DataContext = new XAMLStyles { }; }); } } catch { } };
            checkForChange.Start();

            DEVMODE vDevMode = new DEVMODE();
            List<string> displayResolutions1 = new List<string>();
            int i = 0;
            while (EnumDisplaySettings(null, i, ref vDevMode))
            {
                displayResolutions1.Add($"{vDevMode.dmPelsWidth.ToString()} x {vDevMode.dmPelsHeight.ToString()} @{vDevMode.dmDisplayFrequency.ToString()}hz");

                i++;
            }

            List<string> displayResolutions2 = new List<string>(displayResolutions1.Distinct());
            List<string> displayResolutions3 = new List<string>(displayResolutions2);

            bool firstPass = true;
            string toCompare = "";
            foreach (string str in displayResolutions2)
            {
                if (firstPass == false)
                {
                    string[] bounds1;
                    string[] bounds2;

                    bounds1 = str.Split('@');
                    bounds2 = toCompare.Split('@');
                    bounds1[1] = bounds1[1].Substring(0, bounds1[1].Length - 2);
                    bounds2[1] = bounds2[1].Substring(0, bounds2[1].Length - 2);

                    if (bounds1[0] == bounds2[0])
                    {
                        if (Convert.ToInt32(bounds1[1]) > Convert.ToInt32(bounds2[1]))
                        {
                            displayResolutions3.Remove(toCompare);
                        }
                        else if (Convert.ToInt32(bounds1[1]) < Convert.ToInt32(bounds2[1]))
                        {
                            displayResolutions3.Remove(str);
                        }
                        else if (Convert.ToInt32(bounds1[1]) == Convert.ToInt32(bounds2[1]))
                        {
                            displayResolutions3.Remove(toCompare);
                        }
                    }
                }
                else
                {
                    firstPass = false;
                }

                toCompare = str;
            }

            foreach (string resolution in displayResolutions3)
            {
                cboResolutions.Items.Add(new ComboBoxItem
                {
                    Content = resolution,
                    Background = Styles.bc(Styles.border),
                    Foreground = Styles.bc(Styles.foreground),
                    BorderThickness = new Thickness(0),
                    Style = FindResource("cboItemStyle") as Style,
                    Height = 40,
                    VerticalContentAlignment = VerticalAlignment.Center
                });
            }

            foreach (string resolution in displayResolutions3)
            {
                string[] bounds1;
                bounds1 = resolution.Split('@');

                if (bounds1[0].Contains(startScreenWidth.ToString()) && bounds1[0].Contains(startScreenHeight.ToString()))
                {
                    cboResolutions.Text = resolution;
                    previousComboText = resolution;
                    break;
                }
            }
        }

        private void cboResolutions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (firstOpen == false)
            {
                string[] selectedComboBoxItem = cboResolutions.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "").Split(' ');
                List<string> convertedItems = new List<string>();

                foreach (string toConvert in selectedComboBoxItem)
                {
                    if (toConvert != "x")
                    {
                        convertedItems.Add(toConvert);
                    }
                }
                convertedItems[2] = convertedItems[2].Replace("@", "");
                convertedItems[2] = convertedItems[2].Replace("hz", "");

                CResolution ChangeRes = new CResolution(Convert.ToInt32(convertedItems[0]), Convert.ToInt32(convertedItems[1]), Convert.ToInt32(convertedItems[2]));

                updateComboResolutionsText.Interval = 100;
                updateComboResolutionsText.Tick += UpdateComboResolutionsText_Tick;
                updateComboResolutionsText.Start();
            }
            else
            {
                firstOpen = false;
            }
        }

        private void UpdateComboResolutionsText_Tick(object sender, EventArgs e)
        {
            firstOpen = true;
            string width = SystemParameters.PrimaryScreenWidth.ToString();
            string height = SystemParameters.PrimaryScreenHeight.ToString();
            foreach (ComboBoxItem resolution in cboResolutions.Items)
            {
                string r1 = resolution.Content.ToString();
                if (r1.Contains(width) && r1.Contains(height))
                {
                    cboResolutions.Text = r1;
                    break;
                }
            }
            firstOpen = false;
            updateComboResolutionsText.Stop();
        }

        #region DEVMODE
        [DllImport("user32.dll")]
        public static extern bool EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);
        const int ENUM_CURRENT_SETTINGS = -1;
        const int ENUM_REGISTRY_SETTINGS = -2;

        class CResolution
        {
            public CResolution(int a, int b, int c)
            {
                Screen screen = Screen.PrimaryScreen;

                int iWidth = a;
                int iHeight = b;
                int iFrequency = c;

                DEVMODE dm = new DEVMODE();
                dm.dmDeviceName = new String(new char[32]);
                dm.dmFormName = new String(new char[32]);
                dm.dmSize = (short)Marshal.SizeOf(dm);

                if (0 != User_32.EnumDisplaySettings(null, User_32.ENUM_CURRENT_SETTINGS, ref dm))
                {
                    dm.dmPelsWidth = iWidth;
                    dm.dmPelsHeight = iHeight;
                    dm.dmDisplayFrequency = iFrequency;

                    int iRet = User_32.ChangeDisplaySettings(ref dm, User_32.CDS_TEST);

                    if (iRet == User_32.DISP_CHANGE_FAILED)
                    {
                        msgBox.Show("Description: Unable To Process Your Request. Sorry For This Inconvenience.", "Information", options.b.ok);
                    }
                    else
                    {
                        iRet = User_32.ChangeDisplaySettings(ref dm, User_32.CDS_UPDATEREGISTRY);

                        switch (iRet)
                        {
                            case User_32.DISP_CHANGE_SUCCESSFUL:
                                {
                                    if (Display.revertSettings == 0)
                                    {
                                        var result = msgBox.Show("Please press OK to confirm the new settings." +
                                        "\nThis window will close and revert to the old settings in 10 seconds if you do not press anything.",
                                        "Confirm changes", options.b.okCancel, 10);
                                        if (result == "cancel" || result == null)
                                        {
                                            string[] bounds1;
                                            string[] bounds2;
                                            bounds1 = previousComboText.Split('@');
                                            bounds1[1] = bounds1[1].Substring(0, bounds1[1].Length - 2);
                                            bounds2 = bounds1[0].Split('x');
                                            bounds2[0] = bounds2[0].Substring(0, bounds2[0].Length - 1);
                                            bounds2[1] = bounds2[1].Substring(1);
                                            bounds2[1] = bounds2[1].Substring(0, bounds2[1].Length - 1);

                                            Display.revertSettings = 1;
                                            CResolution ChangeRes = new CResolution(Convert.ToInt32(bounds2[0]), Convert.ToInt32(bounds2[1]), Convert.ToInt32(bounds1[1]));
                                            Display.revertSettings = 0;
                                            var dw = new Display();
                                        }
                                        else if (result == "ok")
                                        {
                                            Display.revertSettings = 0;
                                            Display.previousComboText = $"{dm.dmPelsWidth} x {dm.dmPelsHeight} @{dm.dmDisplayFrequency}hz";
                                        }
                                    }
                                    break;
                                    //successfull change
                                }
                            case User_32.DISP_CHANGE_RESTART:
                                {
                                    Display.revertSettings = 1;
                                    CResolution ChangeRes = new CResolution(Display.startScreenWidth, Display.startScreenHeight, 60);
                                    Display.revertSettings = 0;
                                    //System.Windows.MessageBox.Show("Description: You Need To Reboot For The Change To Happen.\n If You Feel Any Problem After Rebooting Your Machine\nThen Try To Change Resolution In Safe Mode.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                    break;
                                    //windows 9x series you have to restart
                                }
                            default:
                                {
                                    msgBox.Show("Description: Failed To Change The Resolution.", "Error updating display paramaters", options.b.ok);
                                    break;
                                    //failed to change
                                }
                        }
                    }

                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DEVMODE
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string dmDeviceName;
        public short dmSpecVersion;
        public short dmDriverVersion;
        public short dmSize;
        public short dmDriverExtra;
        public int dmFields;

        public short dmOrientation;
        public short dmPaperSize;
        public short dmPaperLength;
        public short dmPaperWidth;

        public short dmScale;
        public short dmCopies;
        public short dmDefaultSource;
        public short dmPrintQuality;
        public short dmColor;
        public short dmDuplex;
        public short dmYResolution;
        public short dmTTOption;
        public short dmCollate;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string dmFormName;
        public short dmLogPixels;
        public short dmBitsPerPel;
        public int dmPelsWidth;
        public int dmPelsHeight;

        public int dmDisplayFlags;
        public int dmDisplayFrequency;

        public int dmICMMethod;
        public int dmICMIntent;
        public int dmMediaType;
        public int dmDitherType;
        public int dmReserved1;
        public int dmReserved2;

        public int dmPanningWidth;
        public int dmPanningHeight;
    };

    class User_32
    {
        [DllImport("user32.dll")]
        public static extern int EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);
        [DllImport("user32.dll")]
        public static extern int ChangeDisplaySettings(ref DEVMODE devMode, int flags);

        public const int ENUM_CURRENT_SETTINGS = -1;
        public const int CDS_UPDATEREGISTRY = 0x01;
        public const int CDS_TEST = 0x02;
        public const int DISP_CHANGE_SUCCESSFUL = 0;
        public const int DISP_CHANGE_RESTART = 1;
        public const int DISP_CHANGE_FAILED = -1;
    }
    #endregion
}

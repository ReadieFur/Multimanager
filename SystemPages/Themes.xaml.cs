using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;
using System.Runtime.InteropServices;
using HWND = System.IntPtr;
using System.Linq;
using System.IO;

namespace Multimanager.SystemPages
{
    //Code resources used:
    //https://github.com/Raymai97/AccentPaletteTool
    //https://github.com/microsoft/terminal/issues/1963
    //https://stackoverflow.com/questions/20960316/get-folder-path-from-explorer-window
    //https://stackoverflow.com/questions/7268302/get-the-titles-of-all-open-windows
    //https://superuser.com/questions/1245923/registry-keys-to-change-personalization-settings
    //https://www.tenforums.com/tutorials/134060-change-window-frame-color-windows-10-a.html
    //https://social.technet.microsoft.com/Forums/Lync/en-US/2b762d55-7779-4b1f-bbb1-77e394394e80/change-windows-colors-in-registry
    //https://winaero.com/blog/change-title-bar-text-color-windows-10/

    public partial class Themes : Page
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern HWND FindWindow(string lpClassName, string lpWindowName);

        public static int themeChangeCountNew = 0;
        public static int themeChangeCountOld = 0;

        public string wallpaper = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Windows\\Themes\\TranscodedWallpaper"; //@"C:\Windows\Web\4K\Wallpaper\Windows\img0_3840x2160.jpg";
        public string b64image;

        bool firstPass = true;
        const string userRoot = "HKEY_CURRENT_USER";
        const string Lsubkey = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
        const string LkeyName = userRoot + "\\" + Lsubkey;

        const string WsubKey = @"Software\Microsoft\Windows\CurrentVersion\Explorer\Wallpapers";
        const string WkeyName = userRoot + "\\" + WsubKey;

        internal SystemPage sp = null;

        public Themes(SystemPage stw = null)
        {
            if (stw != null) { sp = stw; }
            InitializeComponent();
            setThemes();
            try { loadColourPallet(); } catch { }

            try { imagepreview.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(Registry.GetValue(WkeyName, "BackgroundHistoryPath0", string.Empty).ToString())); }
            catch { imagepreview.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(wallpaper)); }
            try { image1.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(Registry.GetValue(WkeyName, "BackgroundHistoryPath1", string.Empty).ToString())); } catch { }
            try { image2.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(Registry.GetValue(WkeyName, "BackgroundHistoryPath2", string.Empty).ToString())); } catch { }
        }

        private void background_Loaded(object sender, RoutedEventArgs e)
        {
            if (Styles.AppsUseLightTheme == "#FFFFFFFF")
            {
                ColourLight.IsChecked = true;
            }
            else
            {
                ColourDark.IsChecked = true;
            }
        }

        private void reloadThemes(string theme, string accent)
        {
            Styles.Set(theme, accent);
            setThemes();
            //var mw = Application.Current.MainWindow as MainWindow;
            sp.mw.setMainWindowThemes();
            string shade = "#33FFFFFF";
            if (Styles.AppsUseLightTheme == "#FFFFFFFF")
            {
                shade = "#33000000";
            }
            Dispatcher.Invoke(() =>
            {
                sp.mw.systemGrid.Background = Styles.b(shade);
                sp.mw.systemBorder.BorderBrush = Styles.b(Styles.accentColour);
            });
            themeChangeCountNew += 1;
        }

        private void setThemes()
        {
            ColourTXT.Foreground = Styles.text();
            ColourLine.Stroke = Styles.text();
            ColourLight.Foreground = Styles.text();
            ColourLight.Background = Styles.button();
            ColourDark.Foreground = Styles.text();
            ColourDark.Background = Styles.button();
            AccentTXT.Foreground = Styles.text();
            AccentLine.Stroke = Styles.text();
            ThemeAccentTXT.Foreground = Styles.text();
            ThemeAccentTXT.Background = Styles.button();
            HexCheckBox.Foreground = Styles.text();
            HexCheckBox.Background = Styles.button();
            BackgroundText.Foreground = Styles.text();
            BackgroundLine.Stroke = Styles.text();
            ChooseWallpaper.Background = Styles.button();
            ChooseWallpaper.Foreground = Styles.text();
            PreviewTXT.Foreground = Styles.text();
            PreviewLine.Stroke = Styles.button();
            defaultWallpaper.Foreground = Styles.accent();

            //Preview colours
            StartMenu.Background = Styles.b("#F2" + Styles.AppsUseLightTheme.Substring(3));
            StartMenu_Copy1.Background = Styles.text();
            StartMenu_Copy3.Foreground = Styles.text();
            StartMenu_Copy19.Background = Styles.text();
            StartMenu_Copy20.Background = Styles.text();
            StartMenu_Copy21.Background = Styles.text();
            StartMenu_Copy22.Background = Styles.text();
            StartMenu_Copy23.Background = Styles.text();
            StartMenu_Copy24.Background = Styles.text();
            StartMenu_Copy25.Background = Styles.text();
            StartMenu_Copy26.Background = Styles.text();
            StartMenu_Copy27.Background = Styles.text();
            StartMenu_Copy28.Background = Styles.theme();
            StartMenu_Copy30.Background = Styles.text();
            StartMenu_Copy.Background = Styles.accent();
            StartMenu_Copy29.Background = Styles.accent();
            StartMenu_Copy2.Background = Styles.accent();
            StartMenu_Copy3.Background = Styles.accent();
            StartMenu_Copy4.Background = Styles.accent();
            StartMenu_Copy5.Background = Styles.accent();
            StartMenu_Copy6.Background = Styles.accent();
            StartMenu_Copy7.Background = Styles.accent();
            StartMenu_Copy8.Background = Styles.accent();
            StartMenu_Copy9.Background = Styles.accent();
            StartMenu_Copy10.Background = Styles.accent();
            StartMenu_Copy11.Background = Styles.accent();
            StartMenu_Copy12.Background = Styles.accent();
            StartMenu_Copy13.Background = Styles.accent();
            StartMenu_Copy14.Background = Styles.accent();
            StartMenu_Copy15.Background = Styles.accent();
            StartMenu_Copy16.Background = Styles.accent();
            StartMenu_Copy17.Background = Styles.accent();
            StartMenu_Copy18.Background = Styles.accent();
            SampleText.Foreground = Styles.text();
        }

        #region Light/Dark Theme
        private void ColourLight_Checked(object sender, RoutedEventArgs e)
        {
            if (firstPass == false)
            {
                Registry.SetValue(LkeyName, "AppsUseLightTheme", 1);
                reloadThemes("#FFFFFFFF", Styles.accentColour);
            }

            firstPass = false;
        }

        private void ColourDark_Checked(object sender, RoutedEventArgs e)
        {
            if (firstPass == false)
            {
                Registry.SetValue(LkeyName, "AppsUseLightTheme", 0);
                reloadThemes("#FF101011", Styles.accentColour);
            }

            firstPass = false;
        }
        #endregion

        #region Colour Pallet & Accent Colour (FIX ACCENT BORDER ISSUE)
        const long MAX_LEN_OF_APDAT = 44;
        const long MIN_LEN_OF_BIN = 0x20;

        private void loadColourPallet()
        {
            var Key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Accent");
            var Val = Key.GetValue("AccentPalette");
            if (Val != null)
            {
                bin = (byte[])Val;
            }
        }

        private void updateColourPallet(string accent)
        {
            //Fix window border bug not changing

            #region Set accent colour
            string colour = accent.Substring(1);
            List<string> colourFlip = new List<string>();
            colourFlip.Add(colour.Substring(colour.Length - 8, 2));
            colourFlip.Add(colour.Substring(colour.Length - 2));
            colourFlip.Add(colour.Substring(colour.Length - 4, 2));
            colourFlip.Add(colour.Substring(colour.Length - 6, 2));
            colour = null;
            foreach (string s in colourFlip)
            {
                colour = colour + s;
            }

            int value = Int32.Parse(colour, System.Globalization.NumberStyles.HexNumber);

            var AccentKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\DWM", true);
            AccentKey.SetValue("AccentColor", value, RegistryValueKind.DWord);
            AccentKey.SetValue("ColorizationAfterglow", value, RegistryValueKind.DWord);
            AccentKey.SetValue("ColorizationColor", value, RegistryValueKind.DWord);
            #endregion

            #region Set colour pallet
            colorPanelToBin(accent);
            var Key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Accent", true);
            Key.SetValue("AccentPalette", bin, RegistryValueKind.Binary);
            #endregion

            #region Set more accents
            Key.SetValue("AccentColorMenu", value, RegistryValueKind.DWord);
            Key.SetValue("StartColorMenu", value, RegistryValueKind.DWord);

            //Is this needed?
            var ControlPanel = Registry.CurrentUser.OpenSubKey("CONTROL PANEL\\Colors", true);
            System.Drawing.Color rgb = System.Drawing.ColorTranslator.FromHtml("#" + accent.Substring(3));
            int r = Convert.ToInt16(rgb.R);
            int g = Convert.ToInt16(rgb.G);
            int b = Convert.ToInt16(rgb.B);
            ControlPanel.SetValue("WindowFrame", $"{r} {g} {b}");
            #endregion

            //Get open windows explorer paths
            List<string> paths = new List<string>();
            foreach (KeyValuePair<IntPtr, string> window in OpenWindowGetter.GetOpenWindows())
            {
                HWND MyHwnd = FindWindow(null, window.Value);
                var t = Type.GetTypeFromProgID("Shell.Application");
                dynamic o = Activator.CreateInstance(t);
                try
                {
                    var ws = o.Windows();
                    for (int i = 0; i < ws.Count; i++)
                    {
                        var ie = ws.Item(i);
                        if (ie == null || ie.hwnd != (long)MyHwnd) { continue; }
                        var path = Path.GetFileName((string)ie.FullName);
                        //Console.WriteLine(path); //Output Explorer
                        if (path.ToLower() == "explorer.exe")
                        {
                            string explorepath = ie.document.focuseditem.path;
                            List<string> pathSplit = explorepath.Split('\\').ToList();
                            pathSplit.RemoveAt(pathSplit.Count - 1);
                            string filePath = "";
                            foreach (string s in pathSplit) { filePath = filePath + s + "\\"; }
                            filePath = filePath.Substring(0, filePath.Length - 1);
                            paths.Add(filePath);
                        }
                    }
                }
                catch /*(Exception ex)*/ { /*Add to logs*/ }
            }

            //Close windows explorer
            foreach (Process p in Process.GetProcessesByName("explorer")) { p.Kill(); }

            //Restore closed explorer windows
            if (paths.Count == 0)
            {
                //Restart windows explorer if it hasnt auto restarted
                string pe = Process.GetProcesses().ToString();
                if (!pe.Contains("explorer"))
                {
                    System.Threading.Thread.Sleep(1500);
                    try
                    {
                        try { Process.Start(@"C:\Windows\System32\dwm.exe"); } catch { }
                        /*Process.Start(@"C:\Windows\explorer.exe");
                        foreach (Process p in Process.GetProcessesByName("explorer"))
                        {
                            if (p.MainWindowTitle.ToLower().Contains("this pc") || p.MainWindowTitle.ToLower().Contains("file explorer"))
                            {
                                p.CloseMainWindow();
                            }
                        }*/
                    }
                    catch
                    {
                        msgBox.Show("Failed to restart windows explorer. You must relogin to start up this process.", "Fatal error");
                    }
                }
            }
            else
            {
                foreach (string p in paths)
                {
                    if (string.IsNullOrEmpty(p))
                    {
                        Process.Start("explorer");
                    }
                    else
                    {
                        Process.Start("explorer", p);
                    }
                }
            }

            //Update app themes
            reloadThemes(Styles.AppsUseLightTheme, accent);

            //Bring app back to front
            Dispatcher.Invoke(() => { sp.mw.Activate(); });
        }

        byte[] bin;

        void colorPanelToBin(string accent)
        {
            Color c;
            for (int i = 0; i < 0x1F; i += 4)
            {
                switch (i)
                {
                    case 0x00:
                        c = (Color)ColorConverter.ConvertFromString(accent);
                        break;
                    case 0x04:
                        c = (Color)ColorConverter.ConvertFromString(accent);
                        break;
                    case 0x08:
                        c = (Color)ColorConverter.ConvertFromString(accent);
                        break;
                    case 0x0C:
                        c = (Color)ColorConverter.ConvertFromString(accent);
                        break;
                    case 0x10:
                        c = (Color)ColorConverter.ConvertFromString(accent);
                        break;
                    case 0x14:
                        c = (Color)ColorConverter.ConvertFromString(accent);
                        break;
                    case 0x18:
                        c = (Color)ColorConverter.ConvertFromString(accent);
                        break;
                    case 0x1C:
                        c = (Color)ColorConverter.ConvertFromString(accent);
                        break;
                    default:
                        return;
                }
                bin[i + 0] = c.R;
                bin[i + 1] = c.G;
                bin[i + 2] = c.B;
            }
        }

        string binToString()
        {
            string s = "";
            for (int i = 0; i < bin.Length; i++)
            {
                s += Convert.ToString(bin[i], 16).PadLeft(2, '0').ToUpper();
                if (i < bin.Length - 1)
                {
                    s += ((i + 1) % 8 == 0) ? Environment.NewLine : " ";
                }
            }
            return s;
        }
        #endregion

        #region Wallpaper Stuff
        private void ChooseWallpaper_Click(object sender, RoutedEventArgs e)
        {
            using (System.Windows.Forms.OpenFileDialog o = new System.Windows.Forms.OpenFileDialog())
            {
                o.Title = "Open Image";
                o.Filter = "Images (*.BMP;*.JPG;*.PNG,*.TIFF)|*.BMP;*.JPG;*.PNG;*.TIFF|All Files (*.*)|*.*";
                o.InitialDirectory = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents");
                if (o.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        wallpaper = o.FileName;

                        using (System.Drawing.Image image = System.Drawing.Image.FromFile(o.FileName))
                        {
                            using (MemoryStream m = new MemoryStream())
                            {
                                image.Save(m, image.RawFormat);
                                byte[] imageBytes = m.ToArray();

                                // Convert byte[] to Base64 String
                                string base64String = Convert.ToBase64String(imageBytes);
                                b64image = base64String;

                                imagepreview.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(wallpaper));
                                wallpaperSetter.Set(o.FileName);
                                //try { imagepreview.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(Registry.GetValue(WkeyName, "BackgroundHistoryPath0", string.Empty).ToString())); } catch { }
                                try { image1.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(Registry.GetValue(WkeyName, "BackgroundHistoryPath1", string.Empty).ToString())); } catch { }
                                try { image2.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(Registry.GetValue(WkeyName, "BackgroundHistoryPath2", string.Empty).ToString())); } catch { }
                            }
                        }
                    }
                    catch
                    {
                        msgBox.Show("Error trying to extract the file", "File Error", options.b.ok);
                    }
                }
            }
        }

        private void image1_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            wallpaperSetter.Set(image1.Source.ToString().Substring(8));
            try { imagepreview.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(Registry.GetValue(WkeyName, "BackgroundHistoryPath0", string.Empty).ToString())); } catch { }
            try { image1.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(Registry.GetValue(WkeyName, "BackgroundHistoryPath1", string.Empty).ToString())); } catch { }
            try { image2.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(Registry.GetValue(WkeyName, "BackgroundHistoryPath2", string.Empty).ToString())); } catch { }
        }

        private void image2_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            wallpaperSetter.Set(image2.Source.ToString().Substring(8));
            try { imagepreview.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(Registry.GetValue(WkeyName, "BackgroundHistoryPath0", string.Empty).ToString())); } catch { }
            try { image1.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(Registry.GetValue(WkeyName, "BackgroundHistoryPath1", string.Empty).ToString())); } catch { }
            try { image2.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(Registry.GetValue(WkeyName, "BackgroundHistoryPath2", string.Empty).ToString())); } catch { }
        }

        private void defaultWallpaper_Click(object sender, RoutedEventArgs e)
        {
            wallpaperSetter.Set(@"C:\Windows\Web\4K\Wallpaper\Windows\img0_3840x2160.jpg");
            try { imagepreview.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(Registry.GetValue(WkeyName, "BackgroundHistoryPath0", string.Empty).ToString())); } catch { }
            try { image1.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(Registry.GetValue(WkeyName, "BackgroundHistoryPath1", string.Empty).ToString())); } catch { }
            try { image2.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(Registry.GetValue(WkeyName, "BackgroundHistoryPath2", string.Empty).ToString())); } catch { }
        }
        #endregion

        #region Colour Buttons
        private void ThemeAccentTXT_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ThemeAccentTXT.Text.Length == 6)
            {
                updateColourPallet(ThemeAccentTXT.Text);
            }
            else
            {
                msgBox.Show("Please enter a valid HEX value in the form of ######", "Invalid Option", options.b.ok);
            }
        }

        private void CustomHEX_Checked(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();
            ThemeAccentTXT.IsEnabled = true;
        }

        private void CustomHEX_Unchecked(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();
            ThemeAccentTXT.IsEnabled = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FFFFB900");
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FFFF8C00");
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FFF7630C");
        }
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FFCA5010");
        }
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FFDA3B01");
        }
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FFEF6950");
        }
        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FFD13438");
        }
        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FFFF4343");
        }
        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FFE74856");
        }
        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FFE81123");
        }
        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FFEA005E");
        }
        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FFC30052");
        }
        private void Button_Click_13(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FFE3008C");
        }
        private void Button_Click_14(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FFBF0077");
        }
        private void Button_Click_15(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FFC239B3");
        }
        private void Button_Click_16(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FF9A0089");
        }
        private void Button_Click_17(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FF0078D7");
        }
        private void Button_Click_18(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FF0063B1");
        }
        private void Button_Click_19(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FF8E8CD8");
        }
        private void Button_Click_20(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FF6B69D6");
        }
        private void Button_Click_21(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FF8764B8");
        }
        private void Button_Click_22(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FF744DA9");
        }
        private void Button_Click_23(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FFB146C2");
        }
        private void Button_Click_24(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FF881798");
        }
        private void Button_Click_25(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FF0099BC");
        }
        private void Button_Click_26(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FF2D7D9A");
        }
        private void Button_Click_27(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FF00B7C3");
        }
        private void Button_Click_28(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FF038387");
        }
        private void Button_Click_29(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FF00B294");
        }
        private void Button_Click_30(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FF018574");
        }
        private void Button_Click_31(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FF00CC6A");
        }
        private void Button_Click_32(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FF10893E");
        }
        private void Button_Click_33(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FF7A7574");
        }
        private void Button_Click_34(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FF5D5A58");
        }
        private void Button_Click_35(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FF68768A");
        }
        private void Button_Click_36(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FF515C6B");
        }
        private void Button_Click_37(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FF567C73");
        }
        private void Button_Click_38(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FF486860");
        }
        private void Button_Click_39(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FF498205");
        }
        private void Button_Click_40(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FF107C10");
        }
        private void Button_Click_41(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FF767676");
        }
        private void Button_Click_42(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FF4C4A48");
        }
        private void Button_Click_43(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FF69797E");
        }
        private void Button_Click_44(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FF4A5459");
        }
        private void Button_Click_45(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FF647C64");
        }
        private void Button_Click_46(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FF525E54");
        }
        private void Button_Click_47(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FF847545");
        }
        private void Button_Click_48(object sender, RoutedEventArgs e)
        {
            updateColourPallet("#FF7E735F");
        }
        #endregion
    }

    internal static class OpenWindowGetter
    {
        /// <summary>Returns a dictionary that contains the handle and title of all the open windows.</summary>
        /// <returns>A dictionary that contains the handle and title of all the open windows.</returns>
        public static IDictionary<HWND, string> GetOpenWindows()
        {
            HWND shellWindow = GetShellWindow();
            Dictionary<HWND, string> windows = new Dictionary<HWND, string>();

            EnumWindows(delegate (HWND hWnd, int lParam)
            {
                if (hWnd == shellWindow) return true;
                if (!IsWindowVisible(hWnd)) return true;

                int length = GetWindowTextLength(hWnd);
                if (length == 0) return true;

                StringBuilder builder = new StringBuilder(length);
                GetWindowText(hWnd, builder, length + 1);

                windows[hWnd] = builder.ToString();
                return true;

            }, 0);

            return windows;
        }

        private delegate bool EnumWindowsProc(HWND hWnd, int lParam);

        [DllImport("USER32.DLL")]
        private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

        [DllImport("USER32.DLL")]
        private static extern int GetWindowText(HWND hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("USER32.DLL")]
        private static extern int GetWindowTextLength(HWND hWnd);

        [DllImport("USER32.DLL")]
        private static extern bool IsWindowVisible(HWND hWnd);

        [DllImport("USER32.DLL")]
        private static extern HWND GetShellWindow();
    }

    internal class wallpaperSetter
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SystemParametersInfo(UInt32 action, UInt32 uParam, String vParam, UInt32 winIni);
        private static readonly UInt32 SPI_SETDESKWALLPAPER = 0x14;
        private static readonly UInt32 SPIF_UPDATEINIFILE = 0x01;
        private static readonly UInt32 SPIF_SENDWININICHANGE = 0x02;

        internal static void Set(String path)
        {
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE); //Set wallpaper
        }
    }
}

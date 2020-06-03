using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Multimanager
{
    public partial class UIConsole : Page
    {
        public UIConsole()
        {
            InitializeComponent();
        }

        private void commandtxt_Loaded(object sender, RoutedEventArgs e)
        {
            consoletxt.Text = $"Multimanager [Version {FileVersionInfo.GetVersionInfo(Environment.CurrentDirectory + "\\Multimanager.dll").FileVersion}] kOF Group.\nFor help, please type 'HELP'";
            commandtxt.Focus();
        }

        private void consoletxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.LeftCtrl || e.Key != Key.C || (e.Key != Key.LeftCtrl && e.Key != Key.C))
            {
                commandtxt.Focus();
            }
        }

        public string nl = "\n\n"; //If the console has been used then a new line will be added before the beginning of each command [UNUSED]
        private void commandtxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                string uc = commandtxt.Text;
                commandtxt.Clear();

                string[] tmpcmd = uc.ToLower().Split(' ');
                List<string> cmd = new List<string>();
                foreach (string word in tmpcmd)
                {
                    cmd.Add(word);
                }
                cmd.Add(" ");

                if (cmd[0] == "" || cmd[0] == " ") { /*Do Nothing*/ }
                else
                {
                    consoletxt.AppendText($"{nl}>{uc}\n");
                    //nl = "\n\n"; [NO LONGER USED]

                    if (cmd[0] == "list")
                    {
                        if (cmd[1] == "processes")
                        {
                            /*consoletxt.AppendText("Name           ID            " +
                                                "\n============== ==============");*/

                            Process[] processes = Process.GetProcesses();

                            List<string> sp1 = new List<string>();
                            foreach (Process p in processes)
                            {
                                if (!p.Responding)
                                {
                                    sp1.Add($"{p.ProcessName} - Not Responding" /*"PID - {p.Id}"*/);
                                }
                                else
                                {
                                    sp1.Add($"{p.ProcessName}" /*"PID - {p.Id}"*/);
                                }
                            }

                            List<string> sp2 = sp1.Distinct().ToList();
                            sp2.Sort();
                            consoletxt.AppendText(string.Join(Environment.NewLine, sp2));
                        }
                        else //Invalid input
                        {
                            consoletxt.AppendText($"The function {cmd[1]} for 'list' was not found.\nFor help with this command type HELP list");
                        } //Invalid input
                    }
                    else if (cmd[0] == "kill")
                    {
                        string subProcess = "";
                        bool subCommandParsed = false;
                        foreach (string s in cmd)
                        {
                            if (subCommandParsed == true) { subProcess = subProcess + s + " "; }
                            else { subCommandParsed = true; }
                        }
                        subProcess = subProcess.Substring(0, subProcess.Length - 3);
                        //MessageBox.Show($"'{subProcess}'");

                        if (subProcess != "")
                        {
                            Process[] processes = Process.GetProcesses();
                            List<string> sp1 = new List<string>();
                            foreach (Process p in processes)
                            {
                                sp1.Add(p.ProcessName);
                            }
                            List<string> sp2 = sp1.Distinct().ToList();

                            if (!sp2.Contains(subProcess))
                            {
                                consoletxt.AppendText($"The process '{subProcess}' is invalid.");
                            }
                            else
                            {
                                foreach (var process in Process.GetProcessesByName(subProcess))
                                {
                                    try
                                    {
                                        process.Kill();
                                        consoletxt.AppendText($"\nKilled: {process.ProcessName} - PID: {process.Id}");
                                    }
                                    catch (Exception ex)
                                    {
                                        consoletxt.AppendText($"Error killing process: {process.ProcessName} - PID:{process.Id} => {ex}");
                                    }
                                }
                            }
                        }
                        else
                        {
                            consoletxt.AppendText("The use of the command is invalid.\nFor help with this command type HELP kill.");
                        }
                    }
                    else if (cmd[0] == "show")
                    {
                        /*if (cmd[1] == "taskmanager")
                        {
                            mw.windowContent.Content = new TaskManager.TaskManager();
                        }*/
                    }
                    else if (cmd[0] == "clear")
                    {
                        consoletxt.Text = "";
                        //nl = "";
                    }
                    else if (cmd[0] == "help")
                    {
                        if (cmd[1] == "list")
                        {
                            consoletxt.AppendText("List usage:" +
                                "\nProcesses - Gets a list of all currently running processes");
                        }
                        else if (cmd[1] == "kill")
                        {
                            consoletxt.AppendText("Kill usage:" +
                                "\n [Process Name] - Kills the program by name.");
                        }
                        else if (cmd[1] == "show")
                        {
                            consoletxt.AppendText("Show usage:" +
                                "\nTaskManager - Shows the Task Manager page.");
                        }
                        else
                        {
                            consoletxt.AppendText("For more information on a specific command, type HELP [command-name]" +
                                "\nlist - Used to list information." +
                                "\nkill - Kill or stop a running process or application." +
                                "\nshow - Shows hidden/WIP windows." +
                                "\nclear - Clears the console.");
                        }
                    }
                    else //Invalid command
                    {
                        consoletxt.AppendText($"The command '{uc}' was not found.\nPlease type HELP for command information.");
                    } //Invalid command
                }

                consoletxt.ScrollToEnd();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Timer checkForChange = new Timer();
            DataContext = new XAMLStyles { };
            checkForChange.Interval = 1000;
            checkForChange.Elapsed += (se, ea) => { try { if (Styles.themeChanged) { Dispatcher.Invoke(() => { DataContext = new XAMLStyles { }; }); } } catch { } };
            checkForChange.Start();
        }
    }
}

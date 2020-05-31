using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Multimanager.TaskManager
{
    /// <summary>
    /// Interaction logic for Processes.xaml
    /// </summary>

    internal class processInfo
    {
        public string icon { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public string status { get; set; }
        public float cpu { get; set; }
        public int memory { get; set; }
        public string commandLine { get; set; }
    }

    public partial class Processes : Page
    {
        public Processes()
        {
            InitializeComponent();
        }

        int timeToWait = 1000;

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            new Task(() =>
            {
                Process[] op = Process.GetProcesses();

                while (true)
                {
                    Process[] p = Process.GetProcesses();

                    Dispatcher.Invoke(() => { processesList.Items.Clear(); });

                    foreach (Process pr in p)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            processesList.Items.Add(new ListViewItem
                            {
                                Content = new processInfo
                                {
                                    name = pr.ProcessName,
                                    id = pr.Id
                                },
                                Background = null,
                                Foreground = Styles.text()
                            });
                        });
                    }

                    op = p;
                    System.Threading.Thread.Sleep(timeToWait); //Wait 1s before regathering data
                }
            }).Start();

            /*foreach (Process p in Process.GetProcesses())
            {
                float CPUUsage;
                using (PerformanceCounter pcCPU = new PerformanceCounter("Process", "% Processor Time", p.ProcessName))
                {
                    pcCPU.NextValue();
                    //Make a string and store the value till next run
                    //System.Threading.Thread.Sleep(100);
                    CPUUsage = pcCPU.NextValue();
                }

                processesList.Items.Add(new processInfo() { name = p.ProcessName, id = p.Id, cpu = CPUUsage});
            }*/
        }
    }
}

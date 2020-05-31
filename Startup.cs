using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Multimanager
{
    public class Startup
    {
        static void Main(string[] args) //Entery point for exe
        {
            MessageBox.Show("this is an exe");
        }

        public void tasks(string e) //Entry point for DLL
        {
            new MainWindow(e).ShowDialog();
        }
    }
}

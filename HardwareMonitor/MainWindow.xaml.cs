using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
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
using System.Windows.Threading;

namespace HardwareMonitor
{
    public partial class MainWindow : Window
    {
        private PerformanceCounter cpuCounter;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;

            // Initialize the PerformanceCounter for CPU usage
            cpuCounter = new PerformanceCounter("Processor", "% Idle Time", "_Total");
        }

        private void MainWindow_Loaded(object sender, EventArgs e)
        {
            // Get CPU information
            string cpuName = GetCPUName();

            // Display CPU information in the GUI
            lblCPUName.Text = $"CPU Name: {cpuName}";

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromSeconds(2); // Update every 1 second
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        { 
            // Get CPU usage and display it in the GUI
            float cpuUsage = cpuCounter.NextValue();
        
            // Introduce a delay before the first reading
            System.Threading.Thread.Sleep(1000);

            cpuUsage = cpuCounter.NextValue();

            lblCPUsage.Text = $"CPU Usage: {cpuUsage / Environment.ProcessorCount:F2} %";
        }

        private string GetCPUName()
        {
            string cpuName = "";
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor"))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    cpuName = obj["Name"].ToString();
                    break; // Assuming there's only one processor
                }
            }
            return cpuName;
        }
    }
}

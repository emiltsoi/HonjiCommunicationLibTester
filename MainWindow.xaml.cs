using System;
using System.Collections.Generic;
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
using System.Windows.Threading;
using HonjiS1200CommLib;

namespace SiemensCommunicatinLibTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public S1200Client client = new S1200Client();
        //public S1200ClientStud client = new S1200ClientStud(); 
        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(PlcSync);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            var result = client.ConnectToPLC(TextBoxIPAddress.GetLineText(0));
            TextBoxResults.Text = client.GetErrorText(result);
        }

        private void ButtonDisconnect_Click(object sender, RoutedEventArgs e)
        {
            var result = client.Disconnect();
            TextBoxResults.Text = client.GetErrorText(result);
        }

        private void PlcSync(object sender, EventArgs e)
        {
            if (client.IsConnected())
            {
                client.Synchronize();
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (client.IsConnected())
                client.Disconnect();
        }

        private void StartHighVacuumFlow(object sender, RoutedEventArgs e)
        {
            if (client.IsConnected())
            {
                client.GetProcesses().GetHighVacuumFlow().StartFlow();
            }
        }

        private void StopHighVacuumFlow(object sender, RoutedEventArgs e)
        {
            if (client.IsConnected())
            {
                client.GetProcesses().GetHighVacuumFlow().StopFlow();
            }
        }

        private void StartLowVacuumFlow(object sender, RoutedEventArgs e)
        {
            if (client.IsConnected())
            {
                client.GetProcesses().GetLowVacuumFlow().StartFlow();
            }
        }

        private void StopLowVacuumFlow(object sender, RoutedEventArgs e)
        {
            if (client.IsConnected())
            {
                client.GetProcesses().GetLowVacuumFlow().StopFlow();
            }
        }


        private void ForceHeaterOn(object sender, RoutedEventArgs e)
        {
            if (client.IsConnected())
            {
                client.ForceHeaterContactOn();
            }
        }

        private void ForceHeaterOff(object sender, RoutedEventArgs e)
        {
            if (client.IsConnected())
            {
                client.ForceHeaterContactOff();
            }
        }
    }
}

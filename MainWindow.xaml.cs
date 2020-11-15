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
using HonjiS1200CommLib.Devices;

namespace SiemensCommunicatinLibTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public S1200Client client = new S1200Client(); // need PLC connection
        //public S1200ClientReadOnly client = new S1200ClientReadOnly(); // need PLC connection
        public S1200ClientStud client = new S1200ClientStud(); // for local testing

        public TimeSpan refreshRateTimeSpan = new TimeSpan(0, 0, 0, 1, 0);
        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(PlcSync);
            dispatcherTimer.Interval = refreshRateTimeSpan;
            dispatcherTimer.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            var result = client.ConnectToPLC(TextBoxIPAddress.GetLineText(0));
            TextBoxResults.Text = client.GetCommunicationErrorText(result);
        }

        private void ButtonDisconnect_Click(object sender, RoutedEventArgs e)
        {
            var result = client.Disconnect();
            TextBoxResults.Text = client.GetCommunicationErrorText(result);
        }

        private void PlcSync(object sender, EventArgs e)
        {
            if (client.IsConnected())
            {
                var result = client.GetLastCommunicationErrorCode();
                TextBoxResults.Text = client.GetCommunicationErrorText(result);
                TextBoxDateTime.Text = client.GetPLCDateTime().ToLongDateString() + " " +  client.GetPLCDateTime().ToLongTimeString();
            }
            else
            {
                TextBoxResults.Text = "Not connected";
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (client.IsConnected())
                client.Disconnect();
        }

        private void ButtonClearAllManualOperation_Click(object sender, RoutedEventArgs e)
        {
            client.GetDevices().ClearAllManualOperations();
        }
    }
}

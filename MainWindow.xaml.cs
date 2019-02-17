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
        //public S1200ClientReadOnly client = new S1200ClientReadOnly();

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
            //TextBoxResults.Text = client.GetPLCCommunicationErrorText(result);
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
                var result = client.GetLastErrorCode();
                TextBoxResults.Text = client.GetErrorText(result);
                TextBoxDateTime.Text = client.GetPLCDateTime().ToLongDateString() + client.GetPLCDateTime().ToLongTimeString();
                var alarmStringList = client.GetAlarms().GetAlarmStringList();
                var alarmList = client.GetAlarms().GetAlarmList();
                AlarmBlock.Text = "Alarms:";
                foreach (var alarm in alarmStringList)
                {
                    AlarmBlock.Text += ("\r" + alarm);
                }
                AlarmBlock.Text += "\r\rAlarmID:";
                foreach (var alarm in alarmList)
                {
                    AlarmBlock.Text += ("\r" + alarm);
                }
                var warningStringList = client.GetAlarms().GetWarningStringList();
                var warningList = client.GetAlarms().GetWarningList();
                WarningBlock.Text = "Warnings:";
                foreach (var warning in warningStringList)
                {
                    WarningBlock.Text += ("\r" + warning);
                }
                WarningBlock.Text += "\r\rWarningID:" ;
                foreach (var warning in warningList)
                {
                    WarningBlock.Text += ("\r" + warning);
                }
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

        private void ClearAlarms(object sender, RoutedEventArgs e)
        {
            if (client.IsConnected())
            {
                client.GetAlarms().AcknowledgeAlarms();
            }
        }
    }
}

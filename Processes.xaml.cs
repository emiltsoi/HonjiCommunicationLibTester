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
using HonjiS1200CommLib;
using System.Windows.Threading;
using HonjiS1200CommLib.Process;

namespace SiemensCommunicatinLibTester
{
    public partial class Processes : UserControl
    {
        IS1200Client client;
        IProcessControl process;

        public Processes()
        {
            InitializeComponent();
            var myWin = (MainWindow)Application.Current.MainWindow;
            client = myWin.client;
            process = client.GetProcesses();
            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(UpdateDisplay);
            dispatcherTimer.Interval = myWin.refreshRateTimeSpan;
            dispatcherTimer.Start();
        }

        private void UpdateDisplay(object sender, EventArgs e)
        {
            if (client.IsConnected())
            {
                UpdateProcessDisplay(process.GetLowVacuumFlow(), LVFState, LVFError);
                UpdateProcessDisplay(process.GetHighVacuumFlow(), HVFState, HVFError);
                UpdateProcessDisplay(process.GetVentFlow(), VFState, VFError);
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void LVFStartButton_Click(object sender, RoutedEventArgs e)
        {
            process.GetLowVacuumFlow().StartFlow();
        }

        private void LVFStopButton_Click(object sender, RoutedEventArgs e)
        {
            process.GetLowVacuumFlow().StopFlow();
        }

        private void HVFStartButton_Click(object sender, RoutedEventArgs e)
        {
            process.GetHighVacuumFlow().StartFlow();
        }

        private void HVFStopButton_Click(object sender, RoutedEventArgs e)
        {
            process.GetHighVacuumFlow().StopFlow();
        }

        private void VFStartButton_Click(object sender, RoutedEventArgs e)
        {
            process.GetVentFlow().StartFlow();
        }

        private void VFStopButton_Click(object sender, RoutedEventArgs e)
        {
            process.GetVentFlow().StopFlow();
        }

        private void UpdateProcessDisplay(ISimpleFlow flow, TextBox state, TextBox errors)
        {
            if (flow.IsRunning()){ state.Text = "Running"; }
            else if (flow.IsIdle()){ state.Text = "Idle"; }
            else if (flow.IsStopping()) { state.Text = "Stopping"; }
            errors.Text = "";
            foreach (var error in flow.GetErrors())
            {
                errors.Text += error + ", ";
            }
        }
    }
}

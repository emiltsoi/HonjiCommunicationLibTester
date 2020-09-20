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
using HonjiS1200CommLib.Devices.GasControlDevices;

namespace SiemensCommunicatinLibTester
{
    /// <summary>
    /// Interaction logic for GasControl.xaml
    /// </summary>
    public partial class GasControl : UserControl
    {
        IS1200Client client;
        IGasControlSystem gasControlSystem;

        public GasControl()
        {
            InitializeComponent();
            var myWin = (MainWindow)Application.Current.MainWindow;
            client = myWin.client;
            gasControlSystem = client.GetDevices().GetGasControlSystem();
            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(UpdateDisplay);
            dispatcherTimer.Interval = myWin.refreshRateTimeSpan;
            dispatcherTimer.Start();
        }

        private void UpdateDisplay(object sender, EventArgs e)
        {
            if (client.IsConnected())
            {
                DisplayUpdateHelper.UpdateMFCState(gasControlSystem.GetMFC1(), MFCH2FlowState, MFCH2ValveOpenState, MFCH2OnState);
                DisplayUpdateHelper.UpdateMFCState(gasControlSystem.GetMFC2(), MFCN2FlowState, MFCN2ValveOpenState, MFCN2OnState);
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void MFC1ForceOnButton_Click(object sender, RoutedEventArgs e)
        {
            gasControlSystem.GetMFC1().ForceFlow(Convert.ToSingle(MFC1ForceValue.Text));
        }

        private void MFC1ClearButton_Click(object sender, RoutedEventArgs e)
        {
            gasControlSystem.GetMFC1().ClearForceFlow();
        }

        private void MFC2ForceOnButton_Click(object sender, RoutedEventArgs e)
        {
            gasControlSystem.GetMFC2().ForceFlow(Convert.ToSingle(MFC2ForceValue.Text));
        }

        private void MFC2ClearButton_Click(object sender, RoutedEventArgs e)
        {
            gasControlSystem.GetMFC2().ClearForceFlow();
        }
    }
}

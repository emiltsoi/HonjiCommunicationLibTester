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

namespace SiemensCommunicatinLibTester
{
    /// <summary>
    /// Interaction logic for CoolingManualControl.xaml
    /// </summary>
    /// 
    public partial class Utility : UserControl
    {
        IS1200Client client;
        IUtility utility;

        public Utility()
        {
            InitializeComponent();
            var myWin = (MainWindow)Application.Current.MainWindow;
            client = myWin.client;
            utility = client.GetDevices().GetUtility();
            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(UpdateDisplay);
            dispatcherTimer.Interval = myWin.refreshRateTimeSpan;
            dispatcherTimer.Start();
        }

        private void UpdateDisplay(object sender, EventArgs e)
        {
            if (client.IsConnected())
            {
                DisplayUpdateHelper.UpdateValveState(utility.GetCoolWaterValve(), CoolWaterValveState);
                DisplayUpdateHelper.UpdateValveState(utility.GetColdWaterValve(), ColdWaterValveState);
                UpdateWaterFlowArc123();
                UpdateWaterFlowArc456();
                UpdateCompressedAirInet();
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void CoolWaterValveOnButton_Click(object sender, RoutedEventArgs e)
        {
            utility.GetCoolWaterValve().ManuallyOpen();
        }

        private void CoolWaterValveOffButton_Click(object sender, RoutedEventArgs e)
        {
            utility.GetCoolWaterValve().ManuallyClose();
        }

        private void CoolWaterValveClear_Click(object sender, RoutedEventArgs e)
        {
            utility.GetCoolWaterValve().ClearManualOperation();
        }

        private void ColdWaterValveOnButton_Click(object sender, RoutedEventArgs e)
        {
            utility.GetColdWaterValve().ManuallyOpen();
        }

        private void ColdWaterValveOffButton_Click(object sender, RoutedEventArgs e)
        {
            utility.GetColdWaterValve().ManuallyClose();
        }

        private void ColdWaterValveClear_Click(object sender, RoutedEventArgs e)
        {
            utility.GetColdWaterValve().ClearManualOperation();
        }

        private void UpdateWaterFlowArc123()
        {
            if (utility.GetWaterFlowSensors().IsArc123WaterFlowInError())
                WaterFlowArc123State.Text = "Error";
            else if (utility.GetWaterFlowSensors().IsArc123WaterFlowInWarning())
                WaterFlowArc123State.Text = "Warning";
            else
                WaterFlowArc123State.Text = "OK";
        }

        private void UpdateWaterFlowArc456()
        {
            if (utility.GetWaterFlowSensors().IsArc456WaterFlowInError())
                WaterFlowArc456State.Text = "Error";
            else if (utility.GetWaterFlowSensors().IsArc456WaterFlowInWarning())
                WaterFlowArc456State.Text = "Warning";
            else
                WaterFlowArc456State.Text = "OK";
        }

        private void UpdateCompressedAirInet()
        {
            if (utility.IsCompressedAirInError())
                CompressedAirState.Text = "Error";
            else
                CompressedAirState.Text = "OK";
        }

        private void ArcSourcesOn(object sender, RoutedEventArgs e)
        {
            client.GetDevices().GetPowerSystem().GetArcSource1().ForceCurrentInAmp(50.0f);
            client.GetDevices().GetPowerSystem().GetArcSource2().ForceCurrentInAmp(60.0f);
            client.GetDevices().GetPowerSystem().GetArcSource3().ForceCurrentInAmp(70.0f);
            client.GetDevices().GetPowerSystem().GetArcSource4().ForceCurrentInAmp(80.0f);
            client.GetDevices().GetPowerSystem().GetArcSource5().ForceCurrentInAmp(90.0f);
            client.GetDevices().GetPowerSystem().GetArcSource6().ForceCurrentInAmp(100.0f);
        }

        private void ArcSourcesOff(object sender, RoutedEventArgs e)
        {
            client.GetDevices().GetPowerSystem().GetArcSource1().ClearForcedCurrent();
            client.GetDevices().GetPowerSystem().GetArcSource2().ClearForcedCurrent();
            client.GetDevices().GetPowerSystem().GetArcSource3().ClearForcedCurrent();
            client.GetDevices().GetPowerSystem().GetArcSource4().ClearForcedCurrent();
            client.GetDevices().GetPowerSystem().GetArcSource5().ClearForcedCurrent();
            client.GetDevices().GetPowerSystem().GetArcSource6().ClearForcedCurrent();
        }

        private void resetOvervoltageRelays(object sender, RoutedEventArgs e)
        {
            client.GetDevices().GetPowerSystem().ResetOverVoltageRelays();
        }
    }
}

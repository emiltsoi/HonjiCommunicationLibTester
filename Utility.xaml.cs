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
using HonjiS1200CommLib.Devices.UtilityDevices;

namespace SiemensCommunicatinLibTester
{
    /// <summary>
    /// Interaction logic for CoolingManualControl.xaml
    /// </summary>
    /// 
    public partial class Utility : UserControl
    {
        IS1200Client client;
        IUtilityDevices utility;

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
                DisplayUpdateHelper.UpdateValveState(utility.GetColdWaterValve(), ColdWaterValveState);
                UpdateMagnetronWaterFlow();
                UpdateChamberWaterFlow();
                UpdateCompressedAirInLet();
                UpdateMainsOn();
                UpdateEmergencyButton();
            }
            CommandManager.InvalidateRequerySuggested();
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

        private void UpdateMagnetronWaterFlow()
        {
            if (utility.GetWaterFlowSensors().IsMagnetronsWaterFlowInError())
                MagnetronsFlowState.Text = "Error";
            else if (utility.GetWaterFlowSensors().IsMagnetronsWaterFlowInWarning())
                MagnetronsFlowState.Text = "Warning";
            else
                MagnetronsFlowState.Text = "OK";
        }

        private void UpdateChamberWaterFlow()
        {
            if (utility.GetWaterFlowSensors().IsChamberWaterFlowInError())
                ChamberFlowState.Text = "Error";
            else if (utility.GetWaterFlowSensors().IsChamberWaterFlowInWarning())
                ChamberFlowState.Text = "Warning";
            else
                ChamberFlowState.Text = "OK";
        }

        private void UpdateCompressedAirInLet()
        {
            if (utility.IsCompressedAirInError())
                CompressedAirState.Text = "Error";
            else
                CompressedAirState.Text = "OK";
        }

        private void UpdateMainsOn()
        {
            if (utility.IsMainPowerOn())
                MainsOnState.Text = "Mains On";
            else
                MainsOnState.Text = "Mains Off";
        }

        private void UpdateEmergencyButton()
        {
            if (utility.IsEstopClear())
                EmergecyButtonState.Text = "EStop Clear";
            else
                EmergecyButtonState.Text = "EStop Engaged";
        }
    }
}

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
using HonjiS1200CommLib.Devices.VacuumSystemDevices;

namespace SiemensCommunicatinLibTester
{
    /// <summary>
    /// Interaction logic for VacuumManualControl.xaml
    /// </summary>
    public partial class VacuumManualControl : UserControl
    {
        IS1200Client client;
        IVacuumSystem vacuumSystem;
        public VacuumManualControl()
        {
            InitializeComponent();
            var myWin = (MainWindow)Application.Current.MainWindow;
            client = myWin.client;
            vacuumSystem = client.GetDevices().GetVacuumSystem();
            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(UpdateDisplay);
            dispatcherTimer.Interval = myWin.refreshRateTimeSpan;
            dispatcherTimer.Start();
        }

        private void BackingValveOnButton_Click(object sender, RoutedEventArgs e)
        {
            vacuumSystem.GetBackingValve().ManuallyOpen();
        }

        private void BackingValveOffButton_Click(object sender, RoutedEventArgs e)
        {
            vacuumSystem.GetBackingValve().ManuallyClose();
        }

        private void BackingValveClear_Click(object sender, RoutedEventArgs e)
        {
            vacuumSystem.GetBackingValve().ClearManualOperation();
        }

        private void IsolationValveOnButton_Click(object sender, RoutedEventArgs e)
        {
            vacuumSystem.GetIsolationValve().ManuallyOpen();
        }

        private void IsolationValveOffButton_Click(object sender, RoutedEventArgs e)
        {
            vacuumSystem.GetIsolationValve().ManuallyClose();
        }

        private void IsolationValveClear_Click(object sender, RoutedEventArgs e)
        {
            vacuumSystem.GetIsolationValve().ClearManualOperation();
        }

        private void ChamberVentValveOnButton_Click(object sender, RoutedEventArgs e)
        {
            vacuumSystem.GetChamberVentValve().ManuallyOpen();
        }

        private void ChamberVentValveOffButton_Click(object sender, RoutedEventArgs e)
        {
            vacuumSystem.GetChamberVentValve().ManuallyClose();
        }

        private void ChamberVentValveClear_Click(object sender, RoutedEventArgs e)
        {
            vacuumSystem.GetChamberVentValve().ClearManualOperation();
        }

        private void SoftPumpValveOnButton_Click(object sender, RoutedEventArgs e)
        {
            vacuumSystem.GetSoftPumpValve().ManuallyOpen();
        }

        private void SoftPumpValveOffButton_Click(object sender, RoutedEventArgs e)
        {
            vacuumSystem.GetSoftPumpValve().ManuallyClose();
        }

        private void SoftPumpValveClear_Click(object sender, RoutedEventArgs e)
        {
            vacuumSystem.GetSoftPumpValve().ClearManualOperation();
        }

        private void RoughingValveOnButton_Click(object sender, RoutedEventArgs e)
        {
            vacuumSystem.GetRoughingValve().ManuallyOpen();
        }

        private void RoughingValveOffButton_Click(object sender, RoutedEventArgs e)
        {
            vacuumSystem.GetRoughingValve().ManuallyClose();
        }

        private void RoughingValveClear_Click(object sender, RoutedEventArgs e)
        {
            vacuumSystem.GetRoughingValve().ClearManualOperation();
        }

        private void RotaryPumpOnButton_Click(object sender, RoutedEventArgs e)
        {
            vacuumSystem.GetRotaryPump().SetManualOn();
        }

        private void RotaryPumpOffButton_Click(object sender, RoutedEventArgs e)
        {
            vacuumSystem.GetRotaryPump().SetManualOff();
        }

        private void RotaryPumpClear_Click(object sender, RoutedEventArgs e)
        {
            vacuumSystem.GetRotaryPump().ResetManualOperation();
        }

        private void UpdateDisplay(object sender, EventArgs e)
        {
            if (client.IsConnected())
            {
                DisplayUpdateHelper.UpdateValveState(vacuumSystem.GetBackingValve(), BackingValveState);
                DisplayUpdateHelper.UpdateValveState(vacuumSystem.GetChamberVentValve(), ChamberVentValveState);
                DisplayUpdateHelper.UpdateValveState(vacuumSystem.GetIsolationValve(), IsolationValveState);
                DisplayUpdateHelper.UpdateValveState(vacuumSystem.GetSoftPumpValve(), SoftPumpValveState);
                DisplayUpdateHelper.UpdateValveState(vacuumSystem.GetRoughingValve(), RoughingValveState);
                DisplayUpdateHelper.UpdateGaugeReading(vacuumSystem.GetWideRangeVacuumGauge(), WideRangeGaugeState, WideRangeGaugeReading);
                DisplayUpdateHelper.UpdateGaugeReading(vacuumSystem.GetPiraniVacuumGauge(), PiraniGaugeState, PiraniGaugeReading);
                DisplayUpdateHelper.UpdateGaugeReading(vacuumSystem.GetAbsouteGauge(), AbsoluteGaugeState, AbsoluteGaugeReading);
                UpdateDoorState();
                UpdateRotaryPumpState();
                UpdateVacuumSwitchState();
                UpdateTMPStatus();
            }
            CommandManager.InvalidateRequerySuggested(); 
        }

        private void UpdateRotaryPumpState()
        {
            if (vacuumSystem.GetRotaryPump().IsOn())
                RotaryPumpState.Text = "On";
            else if (vacuumSystem.GetRotaryPump().IsOff())
                RotaryPumpState.Text = "Off";
            else if (vacuumSystem.GetRotaryPump().IsOverloaded())
                RotaryPumpState.Text = "Overloaded";
            else
                RotaryPumpState.Text = "Unknown";
        }

        private void UpdateDoorState()
        {
            if (vacuumSystem.GetDoor().IsOn())
                DoorOpenState.Text = "Opened";
            else
                DoorOpenState.Text = "Closed";
        }

        private void UpdateVacuumSwitchState()
        {
            if (vacuumSystem.GetVacuumSwitchLowVacuum().IsOn())
                LowVacuumSwitchState.Text = "In low vacuum";
            else
                LowVacuumSwitchState.Text = "Not in low vacuum";
            if (vacuumSystem.GetVacuumSwitchHighVacuum().IsOn())
                HighVacuumSwitchState.Text = "In high vacuum";
            else
                HighVacuumSwitchState.Text = "Not in high vacuum";
            if (!vacuumSystem.GetVacuumSwitchChamber().IsOn())
                ChamberVacuumSwitchState.Text = "In vacuum";
            else
                ChamberVacuumSwitchState.Text = "Not invacuum";
        }

        private void TMPSetSpeedButton_Click(object sender, RoutedEventArgs e)
        {
            vacuumSystem.GetTMP().SetRotationSpeedInHz(Convert.ToUInt16(TMPManualSpeed.Text));
        }

        private void TMPStartButton_Click(object sender, RoutedEventArgs e)
        {
            vacuumSystem.GetTMP().SetManualStart();
        }

        private void TMPStopButton_Click(object sender, RoutedEventArgs e)
        {
            vacuumSystem.GetTMP().SetManualStop();
        }

        private void TMPClearButton_Click(object sender, RoutedEventArgs e)
        {
            vacuumSystem.GetTMP().ResetManualOperation();
        }

        private void TMPClearErrorButton_Click(object sender, RoutedEventArgs e)
        {
            vacuumSystem.GetTMP().ManualClearError();
        }

        private void UpdateTMPStatus()
        {
            TMPCurrentSpeed.Text = vacuumSystem.GetTMP().GetActualSpeedInHz().ToString() + " Hz";
            TMPSpeedSetpoint.Text = vacuumSystem.GetTMP().GetSpeedSetpointInHz().ToString() + " Hz";
            TMPBearingTemperature.Text = vacuumSystem.GetTMP().GetBearingTemperatureInDegreeC().ToString() + " degC";
            TMPMotorTemperature.Text = vacuumSystem.GetTMP().GetMotorTemperatureInDegreeC().ToString() + " degC";
            TMPBaseTemperature.Text = vacuumSystem.GetTMP().GetBaseTemperatureInDegreeC().ToString() + " degC";
            TMPVoltage.Text = vacuumSystem.GetTMP().GetVoltageInVolt().ToString() + " V";
            TMPCurrent.Text = vacuumSystem.GetTMP().GetCurrentInAmp().ToString() + " A";
            TMPReady.IsChecked = vacuumSystem.GetTMP().IsReady();
            TMPWarning.IsChecked = vacuumSystem.GetTMP().HasWarning();
            TMPError.IsChecked = vacuumSystem.GetTMP().InError();
            TMPConnected.IsChecked = vacuumSystem.GetTMP().IsConnected();
            TMPAccelerating.IsChecked = vacuumSystem.GetTMP().IsAccelerating();
            TMPDecelerating.IsChecked = vacuumSystem.GetTMP().IsDecelerating();
            TMPRotating.IsChecked = vacuumSystem.GetTMP().IsRotating();
        }
    }
}

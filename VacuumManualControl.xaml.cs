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
                DisplayUpdateHelper.UpdateGaugeReading(vacuumSystem.GetWideRangeVacuumGauge(), WideRangeGaugeState, WideRangeGaugeReading);
                DisplayUpdateHelper.UpdateGaugeReading(vacuumSystem.GetPiraniVacuumGauge(), PiraniGaugeState, PiraniGaugeReading);
                UpdateDoorState();
                UpdateRotaryPumpState();
                UpdateVacuumSwitchState();
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
            if (!vacuumSystem.GetVacuumSwitchLowVacuum().IsOn())
                LowVacuumSwitchState.Text = "In low vacuum";
            else
                LowVacuumSwitchState.Text = "Not in low vacuum";
            if (!vacuumSystem.GetVacuumSwitchHighVacuum().IsOn())
                HighVacuumSwitchState.Text = "In high vacuum";
            else
                HighVacuumSwitchState.Text = "Not in vacuum";
        }
    }
}

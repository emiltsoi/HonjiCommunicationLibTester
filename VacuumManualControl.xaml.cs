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
    /// Interaction logic for VacuumManualControl.xaml
    /// </summary>
    public partial class VacuumManualControl : UserControl
    {
        IS1200Client client;
        public VacuumManualControl()
        {
            InitializeComponent();
            var myWin = (MainWindow)Application.Current.MainWindow;
            client = myWin.client;
            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(UpdateDisplay);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void BackingAngleValveOnButton_Click(object sender, RoutedEventArgs e)
        {
            client.GetDevices().GetVacuumSystem().GetBackingAngleValve().ManuallyOpen();
        }

        private void BackingAngleValveOffButton_Click(object sender, RoutedEventArgs e)
        {
            client.GetDevices().GetVacuumSystem().GetBackingAngleValve().ManuallyClose();
        }

        private void BackingAngleValveClear_Click(object sender, RoutedEventArgs e)
        {
            client.GetDevices().GetVacuumSystem().GetBackingAngleValve().ClearManualOperation();
        }

        private void RoughingAngleValveOnButton_Click(object sender, RoutedEventArgs e)
        {
            client.GetDevices().GetVacuumSystem().GetRoughingAngleValve().ManuallyOpen();
        }

        private void RoughingAngleValveOffButton_Click(object sender, RoutedEventArgs e)
        {
            client.GetDevices().GetVacuumSystem().GetRoughingAngleValve().ManuallyClose();
        }

        private void RoughingAngleValveClear_Click(object sender, RoutedEventArgs e)
        {
            client.GetDevices().GetVacuumSystem().GetRoughingAngleValve().ClearManualOperation();
        }

        private void ChamberVentValveOnButton_Click(object sender, RoutedEventArgs e)
        {
            client.GetDevices().GetVacuumSystem().GetChamberVentValve().ManuallyOpen();
        }

        private void ChamberVentValveOffButton_Click(object sender, RoutedEventArgs e)
        {
            client.GetDevices().GetVacuumSystem().GetChamberVentValve().ManuallyClose();
        }

        private void ChamberVentValveClear_Click(object sender, RoutedEventArgs e)
        {
            client.GetDevices().GetVacuumSystem().GetChamberVentValve().ClearManualOperation();
        }

        private void TMPVentValveOnButton_Click(object sender, RoutedEventArgs e)
        {
            client.GetDevices().GetVacuumSystem().GetTMPVentValve().ManuallyOpen();
        }

        private void TMPVentValveOffButton_Click(object sender, RoutedEventArgs e)
        {
            client.GetDevices().GetVacuumSystem().GetTMPVentValve().ManuallyClose();
        }

        private void TMPVentValveClear_Click(object sender, RoutedEventArgs e)
        {
            client.GetDevices().GetVacuumSystem().GetTMPVentValve().ClearManualOperation();
        }

        private void RotaryPumpOnButton_Click(object sender, RoutedEventArgs e)
        {
            client.GetDevices().GetVacuumSystem().GetRotaryPump().SetManualOn();
        }

        private void RotaryPumpOffButton_Click(object sender, RoutedEventArgs e)
        {
            client.GetDevices().GetVacuumSystem().GetRotaryPump().SetManualOff();
        }

        private void RotaryPumpClear_Click(object sender, RoutedEventArgs e)
        {
            client.GetDevices().GetVacuumSystem().GetRotaryPump().ResetManualOperation();
        }

        private void DoorLockLockButton_Click(object sender, RoutedEventArgs e)
        {
            client.GetDevices().GetVacuumSystem().GetDoor().ManualLockDoor();
        }

        private void DoorLockUnlockButton_Click(object sender, RoutedEventArgs e)
        {
            client.GetDevices().GetVacuumSystem().GetDoor().ManualUnlockDoor();
        }

        private void DoorLockClear_Click(object sender, RoutedEventArgs e)
        {
            client.GetDevices().GetVacuumSystem().GetDoor().ClearManualOperation();
        }

        private void UpdateDisplay(object sender, EventArgs e)
        {
            if (client.IsConnected())
            {
                UpdateBackingAngleValveState();
                UpdateRoughingAngleValveState();
                UpdateChamberVentValveState();
                UpdateTMPVentValveState();
                UpdateChamberVacuumReading();
                UpdatePneumaticVacuumReading();
                UpdateDoorState();
                UpdateRotaryPumpState();
                updateVacuumSwitchState();
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void UpdateBackingAngleValveState()
        {
            string stateText;
            if (client.GetDevices().GetVacuumSystem().GetBackingAngleValve().IsOpen())
                stateText = "Open";
            else if (client.GetDevices().GetVacuumSystem().GetBackingAngleValve().IsClosed())
                stateText = "Closed";
            else if (client.GetDevices().GetVacuumSystem().GetBackingAngleValve().IsInError())
                stateText = "Error";
            else
                stateText = "Unknown";
            BackingAngleValveState.Text = stateText;
        }

        private void UpdateRoughingAngleValveState()
        {
            string stateText;
            if (client.GetDevices().GetVacuumSystem().GetRoughingAngleValve().IsOpen())
                stateText = "Open";
            else if (client.GetDevices().GetVacuumSystem().GetRoughingAngleValve().IsClosed())
                stateText = "Closed";
            else if (client.GetDevices().GetVacuumSystem().GetRoughingAngleValve().IsInError())
                stateText = "Error";
            else
                stateText = "Unknown";
            RoughingAngleValveState.Text = stateText;
        }

        private void UpdateChamberVentValveState()
        {
            string stateText;
            if (client.GetDevices().GetVacuumSystem().GetChamberVentValve().IsOpen())
                stateText = "Open";
            else if (client.GetDevices().GetVacuumSystem().GetChamberVentValve().IsClosed())
                stateText = "Closed";
            else if (client.GetDevices().GetVacuumSystem().GetChamberVentValve().IsInError())
                stateText = "Error";
            else
                stateText = "Unknown";
            ChamberVentValveState.Text = stateText;
        }

        private void UpdateTMPVentValveState()
        {
            string stateText;
            if (client.GetDevices().GetVacuumSystem().GetTMPVentValve().IsOpen())
                stateText = "Open";
            else if (client.GetDevices().GetVacuumSystem().GetTMPVentValve().IsClosed())
                stateText = "Closed";
            else if (client.GetDevices().GetVacuumSystem().GetTMPVentValve().IsInError())
                stateText = "Error";
            else
                stateText = "Unknown";
            TMPVentValveState.Text = stateText;
        }

        private void UpdateChamberVacuumReading()
        {
            if (client.GetDevices().GetVacuumSystem().GetChamberVacuumGauge().IsInError())
                ChamberGaugeState.Text = "Error";
            else
                ChamberGaugeState.Text = "Normal";
            ChamberGaugeReading.Text = client.GetDevices().GetVacuumSystem().GetChamberVacuumGauge().GetReadingInPa().ToString() + "Pa";
        }

        private void UpdatePneumaticVacuumReading()
        {
            if (client.GetDevices().GetVacuumSystem().GetPneumaticVacuumGauge().IsInError())
                PneumaticGaugeState.Text = "Error";
            else
                PneumaticGaugeState.Text = "Normal";
            PneumaticGaugeReading.Text = client.GetDevices().GetVacuumSystem().GetPneumaticVacuumGauge().GetReadingInPa().ToString() + "Pa";
        }

        private void UpdateRotaryPumpState()
        {
            if (client.GetDevices().GetVacuumSystem().GetRotaryPump().IsOn())
                RotaryPumpState.Text = "On";
            else if (client.GetDevices().GetVacuumSystem().GetRotaryPump().IsOff())
                RotaryPumpState.Text = "Off";
            else
                RotaryPumpState.Text = "Unknown";
        }

        private void UpdateDoorState()
        {
            if (client.GetDevices().GetVacuumSystem().GetDoor().IsDoorOpened())
                DoorOpenState.Text = "Opened";
            else if (client.GetDevices().GetVacuumSystem().GetDoor().IsDoorClosed())
                DoorOpenState.Text = "Closed";
            else
                DoorOpenState.Text = "Unknown";
            if (client.GetDevices().GetVacuumSystem().GetDoor().IsDoorLocked())
                DoorLockState.Text = "Locked";
            else
                DoorLockState.Text = "Unlocked";
        }

        private void updateVacuumSwitchState()
        {
            if (client.GetDevices().GetVacuumSystem().GetVacuumSwitch().IsOn())
                VacuumSwitchState.Text = "In Vacuum";
            else
                VacuumSwitchState.Text = "Not in vacuum";
        }
    }
}

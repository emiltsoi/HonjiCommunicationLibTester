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
        IVacuumSystem vacuumSystem;
        public VacuumManualControl()
        {
            InitializeComponent();
            var myWin = (MainWindow)Application.Current.MainWindow;
            client = myWin.client;
            vacuumSystem = client.GetDevices().GetVacuumSystem();
            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(UpdateDisplay);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void BackingAngleValveOnButton_Click(object sender, RoutedEventArgs e)
        {
            vacuumSystem.GetBackingAngleValve().ManuallyOpen();
        }

        private void BackingAngleValveOffButton_Click(object sender, RoutedEventArgs e)
        {
            vacuumSystem.GetBackingAngleValve().ManuallyClose();
        }

        private void BackingAngleValveClear_Click(object sender, RoutedEventArgs e)
        {
            vacuumSystem.GetBackingAngleValve().ClearManualOperation();
        }

        private void RoughingAngleValveOnButton_Click(object sender, RoutedEventArgs e)
        {
            vacuumSystem.GetRoughingAngleValve().ManuallyOpen();
        }

        private void RoughingAngleValveOffButton_Click(object sender, RoutedEventArgs e)
        {
            vacuumSystem.GetRoughingAngleValve().ManuallyClose();
        }

        private void RoughingAngleValveClear_Click(object sender, RoutedEventArgs e)
        {
            vacuumSystem.GetRoughingAngleValve().ClearManualOperation();
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

        private void TMPVentValveOnButton_Click(object sender, RoutedEventArgs e)
        {
            vacuumSystem.GetTMPVentValve().ManuallyOpen();
        }

        private void TMPVentValveOffButton_Click(object sender, RoutedEventArgs e)
        {
            vacuumSystem.GetTMPVentValve().ManuallyClose();
        }

        private void TMPVentValveClear_Click(object sender, RoutedEventArgs e)
        {
            vacuumSystem.GetTMPVentValve().ClearManualOperation();
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

        private void DoorLockLockButton_Click(object sender, RoutedEventArgs e)
        {
            vacuumSystem.GetDoor().ManualLockDoor();
        }

        private void DoorLockUnlockButton_Click(object sender, RoutedEventArgs e)
        {
            vacuumSystem.GetDoor().ManualUnlockDoor();
        }

        private void DoorLockClear_Click(object sender, RoutedEventArgs e)
        {
            vacuumSystem.GetDoor().ClearManualOperation();
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
                UpdatePiraniVacuumReading();
                UpdateDoorState();
                UpdateRotaryPumpState();
                updateVacuumSwitchState();
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void UpdateBackingAngleValveState()
        {
            string stateText;
            if (vacuumSystem.GetBackingAngleValve().IsOpen())
                stateText = "Open";
            else if (vacuumSystem.GetBackingAngleValve().IsClosed())
                stateText = "Closed";
            else if (vacuumSystem.GetBackingAngleValve().IsInError())
                stateText = "Error";
            else
                stateText = "Unknown";
            BackingAngleValveState.Text = stateText;
        }

        private void UpdateRoughingAngleValveState()
        {
            string stateText;
            if (vacuumSystem.GetRoughingAngleValve().IsOpen())
                stateText = "Open";
            else if (vacuumSystem.GetRoughingAngleValve().IsClosed())
                stateText = "Closed";
            else if (vacuumSystem.GetRoughingAngleValve().IsInError())
                stateText = "Error";
            else
                stateText = "Unknown";
            RoughingAngleValveState.Text = stateText;
        }

        private void UpdateChamberVentValveState()
        {
            string stateText;
            if (vacuumSystem.GetChamberVentValve().IsOpen())
                stateText = "Open";
            else if (vacuumSystem.GetChamberVentValve().IsClosed())
                stateText = "Closed";
            else if (vacuumSystem.GetChamberVentValve().IsInError())
                stateText = "Error";
            else
                stateText = "Unknown";
            ChamberVentValveState.Text = stateText;
        }

        private void UpdateTMPVentValveState()
        {
            string stateText;
            if (vacuumSystem.GetTMPVentValve().IsOpen())
                stateText = "Open";
            else if (vacuumSystem.GetTMPVentValve().IsClosed())
                stateText = "Closed";
            else if (vacuumSystem.GetTMPVentValve().IsInError())
                stateText = "Error";
            else
                stateText = "Unknown";
            TMPVentValveState.Text = stateText;
        }

        private void UpdateChamberVacuumReading()
        {
            if (vacuumSystem.GetChamberVacuumGauge().IsInError())
                ChamberGaugeState.Text = "Error";
            else if (vacuumSystem.GetChamberVacuumGauge().isInvalid())
                ChamberGaugeState.Text = "Invalid";
            else
                ChamberGaugeState.Text = "Normal";
            ChamberGaugeReading.Text = vacuumSystem.GetChamberVacuumGauge().GetReadingInPa().ToString() + "Pa";
        }

        private void UpdatePiraniVacuumReading()
        {
            if (vacuumSystem.GetPiraniVacuumGauge().IsInError())
                PiraniGaugeState.Text = "Error";
            else if (vacuumSystem.GetPiraniVacuumGauge().isInvalid())
                PiraniGaugeState.Text = "Invalid";
            else
                PiraniGaugeState.Text = "Normal";
            PneumaticGaugeReading.Text = vacuumSystem.GetPiraniVacuumGauge().GetReadingInPa().ToString() + "Pa";
        }

        private void UpdateRotaryPumpState()
        {
            if (vacuumSystem.GetRotaryPump().IsOn())
                RotaryPumpState.Text = "On";
            else if (vacuumSystem.GetRotaryPump().IsOff())
                RotaryPumpState.Text = "Off";
            else
                RotaryPumpState.Text = "Error";
        }

        private void UpdateDoorState()
        {
            if (vacuumSystem.GetDoor().IsDoorOpened())
                DoorOpenState.Text = "Opened";
            else if (vacuumSystem.GetDoor().IsDoorClosed())
                DoorOpenState.Text = "Closed";
            else
                DoorOpenState.Text = "Unknown";
            if (vacuumSystem.GetDoor().IsDoorLocked())
                DoorLockState.Text = "Locked";
            else
                DoorLockState.Text = "Unlocked";
        }

        private void updateVacuumSwitchState()
        {
            if (vacuumSystem.GetVacuumSwitch().IsOn())
                VacuumSwitchState.Text = "Not in Vacuum";
            else
                VacuumSwitchState.Text = "In vacuum";
        }
    }
}

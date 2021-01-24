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
using HonjiS1200CommLib.Devices.PowerSystemDevices;
using HonjiS1200CommLib.Devices.BaseDevices;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SiemensCommunicatinLibTester
{
    /// <summary>
    /// Interaction logic for CoolingManualControl.xaml
    /// </summary>
    /// 
    public class PSUStatus : INotifyPropertyChanged
    {
        private IAEPowerSupplyBase device;
        public string PSU { get; set; }
        private string voltage, current, setpoint;
        private bool psuConnected, psuError;
        public string Voltage { get { return voltage; } set { if (voltage != value) { voltage = value; NotifyPropertyChanged(); } } }
        public string Current { get { return current; } set { if (current != value) { current = value; NotifyPropertyChanged(); } } }
        public string Setpoint { get { return setpoint; } set { if (setpoint != value) { setpoint = value; NotifyPropertyChanged(); } } }
        public bool PSUConnected { get { return psuConnected; } set { if (psuConnected != value) { psuConnected = value; NotifyPropertyChanged(); } } }
        public bool PSUError { get { return psuError; } set { if (psuError != value) { psuError = value; NotifyPropertyChanged(); } } }

        public PSUStatus(IAEPowerSupplyBase device, string psuName)
        {
            this.PSU = psuName;
            this.device = device;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RefreshData()
        {
            Voltage = this.device.GetVoltageReadingInVolt().ToString();
            Current = this.device.GetCurrentReadingInAmp().ToString();
            Setpoint = this.device.GetRawSetpoint().ToString();
            PSUConnected = this.device.IsConnected();
            PSUError = this.device.IsInError();
        }
    }

    public partial class PowerSupplyControl : UserControl
    {
        IS1200Client client;
        IPowerSystem powerSystem;
        ObservableCollection<PSUStatus> memberData = new ObservableCollection<PSUStatus>();

        public PowerSupplyControl()
        {
            InitializeComponent();
            var myWin = (MainWindow)Application.Current.MainWindow;
            client = myWin.client;
            powerSystem = client.GetDevices().GetPowerSystem();
            populatePSUStatus();
            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(UpdateDisplay);
            dispatcherTimer.Interval = myWin.refreshRateTimeSpan;
            dispatcherTimer.Start();
        }

        private void UpdateDisplay(object sender, EventArgs e)
        {
            if (client.IsConnected())
            {
                DisplayUpdateHelper.UpdateSimpleOnOffDeviceState(powerSystem.GetAESupplyContactor(), AESupplyContactState);
                DisplayUpdateHelper.UpdateSimpleOnOffDeviceState(powerSystem.GetBiasSupplyContactor(), BiasContactState);
                updatePSUStatus();
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void AESupplyContactOnButton_Click(object sender, RoutedEventArgs e)
        {
            powerSystem.GetAESupplyContactor().SetManualOn();
        }

        private void AESupplyContactOffButton_Click(object sender, RoutedEventArgs e)
        {
            powerSystem.GetAESupplyContactor().SetManualOff();
        }

        private void AESupplyContactClearButton_Click(object sender, RoutedEventArgs e)
        {
            powerSystem.GetAESupplyContactor().ResetManualOperation();
        }

        private void BiasContactOnButton_Click(object sender, RoutedEventArgs e)
        {
            powerSystem.GetBiasSupplyContactor().SetManualOn();
        }

        private void BiasContactOffButton_Click(object sender, RoutedEventArgs e)
        {
            powerSystem.GetBiasSupplyContactor().SetManualOff();
        }

        private void BiasContactClearButton_Click(object sender, RoutedEventArgs e)
        {
            powerSystem.GetBiasSupplyContactor().ResetManualOperation();
        }

        private void AE1AForceVoltageButton_Click(object sender, RoutedEventArgs e)
        {
            powerSystem.GetAEPowerSupply1A().ForceManualCurrentInAmp(Convert.ToSingle(AE1AForcedCurrentValue.Text));
        }

        private void AE1AClearButton_Click(object sender, RoutedEventArgs e)
        {
            powerSystem.GetAEPowerSupply1A().ClearForcedSetpoint();
        }

        private void AE2AForceVoltageButton_Click(object sender, RoutedEventArgs e)
        {
            powerSystem.GetAEPowerSupply2A().ForceManualCurrentInAmp(Convert.ToSingle(AE2AForcedCurrentValue.Text));
        }

        private void AE2AClearButton_Click(object sender, RoutedEventArgs e)
        {
            powerSystem.GetAEPowerSupply2A().ClearForcedSetpoint();
        }

        private void AE3AForceVoltageButton_Click(object sender, RoutedEventArgs e)
        {
            powerSystem.GetAEPowerSupply3A().ForceManualCurrentInAmp(Convert.ToSingle(AE3AForcedCurrentValue.Text));
        }

        private void AE3AClearButton_Click(object sender, RoutedEventArgs e)
        {
            powerSystem.GetAEPowerSupply3A().ClearForcedSetpoint();
        }

        private void AE1BForceVoltageButton_Click(object sender, RoutedEventArgs e)
        {
            powerSystem.GetAEPowerSupply1B().ForceManualCurrentInAmp(Convert.ToSingle(AE1BForcedCurrentValue.Text));
        }

        private void AE1BClearButton_Click(object sender, RoutedEventArgs e)
        {
            powerSystem.GetAEPowerSupply1B().ClearForcedSetpoint();
        }

        private void AE2BForceVoltageButton_Click(object sender, RoutedEventArgs e)
        {
            powerSystem.GetAEPowerSupply2B().ForceManualCurrentInAmp(Convert.ToSingle(AE2BForcedCurrentValue.Text));
        }

        private void AE2BClearButton_Click(object sender, RoutedEventArgs e)
        {
            powerSystem.GetAEPowerSupply2B().ClearForcedSetpoint();
        }

        private void AE3BForceVoltageButton_Click(object sender, RoutedEventArgs e)
        {
            powerSystem.GetAEPowerSupply3B().ForceManualCurrentInAmp(Convert.ToSingle(AE3BForcedCurrentValue.Text));
        }

        private void AE3BClearButton_Click(object sender, RoutedEventArgs e)
        {
            powerSystem.GetAEPowerSupply3B().ClearForcedSetpoint();
        }

        private void BiasForceVoltageButton_Click(object sender, RoutedEventArgs e)
        {
            powerSystem.GetBiasPowerSupply().ForceManualVoltageInVolt(Convert.ToUInt16(BiasForcedVoltageValue.Text));
        }

        private void BiasClearButton_Click(object sender, RoutedEventArgs e)
        {
            powerSystem.GetBiasPowerSupply().ClearForcedSetpoint();
        }

        private void populatePSUStatus()
        {
            memberData.Add(new PSUStatus(powerSystem.GetAEPowerSupply1A(), "AEPSU1A"));
            memberData.Add(new PSUStatus(powerSystem.GetAEPowerSupply2A(), "AEPSU2A"));
            memberData.Add(new PSUStatus(powerSystem.GetAEPowerSupply3A(), "AEPSU3A"));
            memberData.Add(new PSUStatus(powerSystem.GetAEPowerSupply1B(), "AEPSU1B"));
            memberData.Add(new PSUStatus(powerSystem.GetAEPowerSupply2B(), "AEPSU2B"));
            memberData.Add(new PSUStatus(powerSystem.GetAEPowerSupply3B(), "AEPSU3B"));
            memberData.Add(new PSUStatus(powerSystem.GetBiasPowerSupply(), "BiasPSU"));
            dataGrid.DataContext = memberData;
        }

        private void updatePSUStatus()
        {
            foreach(var member in memberData)
            {
                member.RefreshData();
            }
        }
    }
}

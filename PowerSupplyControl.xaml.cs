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
        private string voltage, current;
        private bool psuConnected, psuError;
        public string Voltage { get { return voltage; } set { if (voltage != value) { voltage = value; NotifyPropertyChanged(); } } }
        public string Current { get { return current; } set { if (current != value) { current = value; NotifyPropertyChanged(); } } }
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

        private void AE1ForceVoltageButton_Click(object sender, RoutedEventArgs e)
        {
            powerSystem.GetAEPowerSupply1().ForceManualCurrentInAmp(Convert.ToSingle(AE1ForcedCurrentValue.Text));
        }

        private void AE1ClearButton_Click(object sender, RoutedEventArgs e)
        {
            powerSystem.GetAEPowerSupply1().ClearForcedSetpoint();
        }

        private void AE2ForceVoltageButton_Click(object sender, RoutedEventArgs e)
        {
            powerSystem.GetAEPowerSupply2().ForceManualCurrentInAmp(Convert.ToSingle(AE2ForcedCurrentValue.Text));
        }

        private void AE2ClearButton_Click(object sender, RoutedEventArgs e)
        {
            powerSystem.GetAEPowerSupply2().ClearForcedSetpoint();
        }

        private void AE3ForceVoltageButton_Click(object sender, RoutedEventArgs e)
        {
            powerSystem.GetAEPowerSupply3().ForceManualCurrentInAmp(Convert.ToSingle(AE3ForcedCurrentValue.Text));
        }

        private void AE3ClearButton_Click(object sender, RoutedEventArgs e)
        {
            powerSystem.GetAEPowerSupply3().ClearForcedSetpoint();
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
            memberData.Add(new PSUStatus(powerSystem.GetAEPowerSupply1(), "AEPSU1"));
            memberData.Add(new PSUStatus(powerSystem.GetAEPowerSupply2(), "AEPSU2"));
            memberData.Add(new PSUStatus(powerSystem.GetAEPowerSupply3(), "AEPSU3"));
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

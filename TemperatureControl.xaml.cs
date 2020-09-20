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
using HonjiS1200CommLib.Devices.TemperatureControlDevices;
using HonjiS1200CommLib.Devices.BaseDevices;

namespace SiemensCommunicatinLibTester
{
    /// <summary>
    /// Interaction logic for CoolingManualControl.xaml
    /// </summary>
    /// 
    public partial class TemperatureControl : UserControl
    {
        IS1200Client client;
        ITemperatureControlSystem temperatureControl;

        public TemperatureControl()
        {
            InitializeComponent();
            var myWin = (MainWindow)Application.Current.MainWindow;
            client = myWin.client;
            temperatureControl = client.GetDevices().GetTemperatureControlSystem();
            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(UpdateDisplay);
            dispatcherTimer.Interval = myWin.refreshRateTimeSpan;
            dispatcherTimer.Start();
        }

        private void UpdateDisplay(object sender, EventArgs e)
        {
            if (client.IsConnected())
            {
                DisplayUpdateHelper.UpdateSimpleOnOffDeviceState(temperatureControl.GetHeater1Contactor(), Heater1ContactState);
                DisplayUpdateHelper.UpdateSimpleOnOffDeviceState(temperatureControl.GetHeater2Contactor(), Heater2ContactState);
                DisplayUpdateHelper.UpdateSimpleOnOffDeviceState(temperatureControl.GetHeater3Contactor(), Heater3ContactState);
                UpdateTemperature();
            }
            CommandManager.InvalidateRequerySuggested();
        }


        private void UpdateTemperature()
        {
            ChamberTemperatureState.Text = temperatureControl.GetChamberTemperatureSensor().GetTemperature().ToString() + " DegC";
        }

        private void Heater1ContactOnButton_Click(object sender, RoutedEventArgs e)
        {
            temperatureControl.GetHeater1Contactor().SetManualOn();
        }

        private void Heater1ContactOffButton_Click(object sender, RoutedEventArgs e)
        {
            temperatureControl.GetHeater1Contactor().SetManualOff();
        }

        private void Heater1ContactClearButton_Click(object sender, RoutedEventArgs e)
        {
            temperatureControl.GetHeater1Contactor().ResetManualOperation();
        }

        private void Heater2ContactOnButton_Click(object sender, RoutedEventArgs e)
        {
            temperatureControl.GetHeater2Contactor().SetManualOn();
        }

        private void Heater2ContactOffButton_Click(object sender, RoutedEventArgs e)
        {
            temperatureControl.GetHeater2Contactor().SetManualOff();
        }

        private void Heater2ContactClearButton_Click(object sender, RoutedEventArgs e)
        {
            temperatureControl.GetHeater2Contactor().ResetManualOperation();
        }

        private void Heater3ContactOnButton_Click(object sender, RoutedEventArgs e)
        {
            temperatureControl.GetHeater3Contactor().SetManualOn();
        }

        private void Heater3ContactOffButton_Click(object sender, RoutedEventArgs e)
        {
            temperatureControl.GetHeater3Contactor().SetManualOff();
        }

        private void Heater3ContactClearButton_Click(object sender, RoutedEventArgs e)
        {
            temperatureControl.GetHeater3Contactor().ResetManualOperation();
        }

        private void TemperatureController1ForceButton_Click(object sender, RoutedEventArgs e)
        {
            temperatureControl.GetTemperatureController1().SetManualTemperatureSetpt(Convert.ToInt16(TemperatureController1Setpoint.Text));
        }

        private void TemperatureController1ClearButton_Click(object sender, RoutedEventArgs e)
        {
            temperatureControl.GetTemperatureController1().ResetManualOperation();
        }

        private void TemperatureController2ForceButton_Click(object sender, RoutedEventArgs e)
        {
            temperatureControl.GetTemperatureController2().SetManualTemperatureSetpt(Convert.ToInt16(TemperatureController2Setpoint.Text));
        }

        private void TemperatureController2ClearButton_Click(object sender, RoutedEventArgs e)
        {
            temperatureControl.GetTemperatureController2().ResetManualOperation();
        }

        private void TemperatureController3ForceButton_Click(object sender, RoutedEventArgs e)
        {
            temperatureControl.GetTemperatureController3().SetManualTemperatureSetpt(Convert.ToInt16(TemperatureController3Setpoint.Text));
        }

        private void TemperatureController3ClearButton_Click(object sender, RoutedEventArgs e)
        {
            temperatureControl.GetTemperatureController3().ResetManualOperation();
        }
    }
}

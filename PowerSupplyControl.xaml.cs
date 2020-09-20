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

namespace SiemensCommunicatinLibTester
{
    /// <summary>
    /// Interaction logic for CoolingManualControl.xaml
    /// </summary>
    /// 
    public partial class PowerSupplyControl : UserControl
    {
        IS1200Client client;
        IPowerSystem powerSystem;

        public PowerSupplyControl()
        {
            InitializeComponent();
            var myWin = (MainWindow)Application.Current.MainWindow;
            client = myWin.client;
            powerSystem = client.GetDevices().GetPowerSystem();
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

    }
}

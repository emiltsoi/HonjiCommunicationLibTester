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
using HonjiS1200CommLib.Devices.MotionSystemDevices;

namespace SiemensCommunicatinLibTester
{
    /// <summary>
    /// Interaction logic for CarouselManualControl.xaml
    /// </summary>
    public partial class MotionControl : UserControl
    {
        IS1200Client client;
        IMotionSystem motionSystem;

        public MotionControl()
        {
            InitializeComponent();
            var myWin = (MainWindow)Application.Current.MainWindow;
            client = myWin.client;
            motionSystem = client.GetDevices().GetMotionSystem();
            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(UpdateDisplay);
            dispatcherTimer.Interval = myWin.refreshRateTimeSpan;
            dispatcherTimer.Start();
        }

        private void UpdateDisplay(object sender, EventArgs e)
        {
            if (client.IsConnected())
            {
                CurrentSpeedTextBox.Text = motionSystem.GetVSD().GetMotorSpeedinRPM().ToString();
                CurrentSpeedSetpointTextBox.Text = motionSystem.GetVSD().GetMotorSpeedSetpoint().ToString();
                if (motionSystem.GetVSD().IsOn())
                    PumpStateTextBox.Text = "ON";
                else if (motionSystem.GetVSD().IsOff())
                    PumpStateTextBox.Text = "OFF";
                else if (motionSystem.GetVSD().IsOverloaded())
                    PumpStateTextBox.Text = "Overloaded";
                else
                    PumpStateTextBox.Text = "Unknown";
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void ForcePumpOnButton_Click(object sender, RoutedEventArgs e)
        {
            motionSystem.GetVSD().SetManualOn();
        }

        private void ForcePumpOffButton_Click(object sender, RoutedEventArgs e)
        {
            motionSystem.GetVSD().SetManualOff();
        }

        private void UnforcePumpButton_Click(object sender, RoutedEventArgs e)
        {
            motionSystem.GetVSD().ResetManualOperation();
        }

        private void SetpeedButton_Click(object sender, RoutedEventArgs e)
        {
            var speed = Convert.ToSingle(SetSpeedTextBox.Text);
            motionSystem.GetVSD().SetMotorSpeedinRPM(speed);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            motionSystem.GetVSD().SetMotorDirection(true);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            motionSystem.GetVSD().SetMotorDirection(false);
        }
    }
}

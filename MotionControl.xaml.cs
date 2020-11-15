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
                DisplayUpdateHelper.UpdateSimpleOnOffDeviceState(motionSystem.GetLiftMotorContactor(), LiftMotorContactStateTextBox);
                DisplayUpdateHelper.UpdateSimpleOnOffDeviceState(motionSystem.GetSwingMotorContactor(), SwingMotorContactStateTextBox);
                UpdateMotorDisplay();
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

        private void LiftMotorContactOnButton_Click(object sender, RoutedEventArgs e)
        {
            motionSystem.GetLiftMotorContactor().SetManualOn();
        }

        private void LiftMotorContactOffButton_Click(object sender, RoutedEventArgs e)
        {
            motionSystem.GetLiftMotorContactor().SetManualOff();
        }

        private void LiftMotorForwardMoveButton_Click(object sender, RoutedEventArgs e)
        {
            motionSystem.GetLiftMotor().StartForwardMove();
        }

        private void LiftMotorReverseMoveButton_Click(object sender, RoutedEventArgs e)
        {
            motionSystem.GetLiftMotor().StartReverseMove();
        }

        private void LiftMotorStopButton_Click(object sender, RoutedEventArgs e)
        {
            motionSystem.GetLiftMotor().StopMove();
        }

        private void SwingMotorContactOnButton_Click(object sender, RoutedEventArgs e)
        {
            motionSystem.GetSwingMotorContactor().SetManualOn();
        }

        private void SwingMotorContactOffButton_Click(object sender, RoutedEventArgs e)
        {
            motionSystem.GetSwingMotorContactor().SetManualOff();
        }

        private void SwingMotorForwardMoveButton_Click(object sender, RoutedEventArgs e)
        {
            motionSystem.GetSwingMotor().StartForwardMove();
        }

        private void SwingMotorReverseMoveButton_Click(object sender, RoutedEventArgs e)
        {
            motionSystem.GetSwingMotor().StartReverseMove();
        }

        private void SwingMotorStopButton_Click(object sender, RoutedEventArgs e)
        {
            motionSystem.GetSwingMotor().StopMove();
        }

        private void UpdateMotorDisplay()
        {
            LiftMotorReady.IsChecked = motionSystem.GetLiftMotor().IsReady();
            LiftMotorInAlarm.IsChecked = motionSystem.GetLiftMotor().HasAlarm();
            LiftMotorMoving.IsChecked = motionSystem.GetLiftMotor().IsMoving();
            LiftMotorAtForwardEnd.IsChecked = motionSystem.GetLiftMotor().AtForwardStrokeEnd();
            LiftMotorAtReverseEnd.IsChecked = motionSystem.GetLiftMotor().AtReverseStrokeEnd();

            SwingMotorReady.IsChecked = motionSystem.GetSwingMotor().IsReady();
            SwingMotorInAlarm.IsChecked = motionSystem.GetSwingMotor().HasAlarm();
            SwingMotorMoving.IsChecked = motionSystem.GetSwingMotor().IsMoving();
            SwingMotorAtForwardEnd.IsChecked = motionSystem.GetSwingMotor().AtForwardStrokeEnd();
            SwingMotorAtReverseEnd.IsChecked = motionSystem.GetSwingMotor().AtReverseStrokeEnd();
        }
    }
}

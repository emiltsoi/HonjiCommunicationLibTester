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
    /// Interaction logic for CarouselManualControl.xaml
    /// </summary>
    public partial class CarouselManualControl : UserControl
    {
        IS1200Client client;
        ICarouselSystem carouselSystem;

        public CarouselManualControl()
        {
            InitializeComponent();
            var myWin = (MainWindow)Application.Current.MainWindow;
            client = myWin.client;
            carouselSystem = client.GetDevices().GetCarouselSystem();
            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(UpdateDisplay);
            dispatcherTimer.Interval = myWin.refreshRateTimeSpan;
            dispatcherTimer.Start();
        }

        private void UpdateDisplay(object sender, EventArgs e)
        {
            if (client.IsConnected())
            {
                var speed = carouselSystem.GetVSD().GetMotorSpeedinRPM();
                CurrentSpeedTextBox.Text = speed.ToString();
                if (carouselSystem.GetVSD().IsOn())
                    PumpStateTextBox.Text = "ON";
                else if (carouselSystem.GetVSD().IsOff())
                    PumpStateTextBox.Text = "OFF";
                else if (carouselSystem.GetVSD().IsOverloaded())
                    PumpStateTextBox.Text = "Overloaded";
                else
                    PumpStateTextBox.Text = "Unknown";
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void ForcePumpOnButton_Click(object sender, RoutedEventArgs e)
        {
            carouselSystem.GetVSD().SetManualOn();
        }

        private void ForcePumpOffButton_Click(object sender, RoutedEventArgs e)
        {
            carouselSystem.GetVSD().SetManualOff();
        }

        private void UnforcePumpButton_Click(object sender, RoutedEventArgs e)
        {
            carouselSystem.GetVSD().ResetManualOperation();
        }

        private void SetpeedButton_Click(object sender, RoutedEventArgs e)
        {
            var speed = Convert.ToSingle(SetSpeedTextBox.Text);
            carouselSystem.GetVSD().SetMotorSpeedinRPM(speed);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            carouselSystem.GetVSD().SetMotorDirection(true);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            carouselSystem.GetVSD().SetMotorDirection(false);
        }
    }
}

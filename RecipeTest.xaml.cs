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
    public partial class RecipeTest : UserControl
    {
        IS1200Client client;
        IRecipeControl recipeControl;

        public RecipeTest()
        {
            InitializeComponent();
            var myWin = (MainWindow)Application.Current.MainWindow;
            client = myWin.client;
            recipeControl = client.GetProcesses().GetRecipeControl();
            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(HandleProcessSync);
            dispatcherTimer.Interval = myWin.refreshRateTimeSpan;
            dispatcherTimer.Start();
        }

        private void HandleProcessSync(object sender, EventArgs e)
        {
            if (client.IsConnected())
            {
                if (recipeControl.GetHeatingProcess().IsCompleted() && recipeControl.GetRunningProcessType() == ProcessType.Heating)
                {
                    if (recipeControl.GetRunningProcessStepNumber() == 1)
                        StartHeatingStep2();
                    else if (recipeControl.GetRunningProcessStepNumber() == 2)
                        StartHeatingStep3();
                    else if (recipeControl.GetRunningProcessStepNumber() == 3)
                        StartHeatingStep2();
                }
                else if (recipeControl.GetPlasmaProcess().IsCompleted() && recipeControl.GetRunningProcessType() == ProcessType.Plasma)
                {
                    if (recipeControl.GetRunningProcessStepNumber() == 1)
                        StartPlasmaStep2();
                    else if (recipeControl.GetRunningProcessStepNumber() == 2)
                        StartPlasmaStep3();
                    else if (recipeControl.GetRunningProcessStepNumber() == 3)
                        StartSputteringStep1();
                }
                else if (recipeControl.GetSputteringProcess().IsCompleted() && recipeControl.GetRunningProcessType() == ProcessType.Sputtering)
                {
                    if (recipeControl.GetRunningProcessStepNumber() == 1)
                        StartSputteringStep2();
                    else if (recipeControl.GetRunningProcessStepNumber() == 2)
                        StartCoatingStep1();
                }
                else if (recipeControl.GetCoatingProcess().IsCompleted() && recipeControl.GetRunningProcessType() == ProcessType.Coating)
                {
                    if (recipeControl.GetRunningProcessStepNumber() == 1)
                        StartCoatingStep2();
                    else if (recipeControl.GetRunningProcessStepNumber() == 2)
                        StartCoatingStep3();
                    else if (recipeControl.GetRunningProcessStepNumber() == 3)
                        recipeControl.StopAllRecipeProcesses();
                }

                bool heatingRun = recipeControl.GetHeatingProcess().IsRunning();
                bool plasmaRun = recipeControl.GetPlasmaProcess().IsRunning();
                bool sputterRun = recipeControl.GetSputteringProcess().IsRunning();
                bool coatRun = recipeControl.GetCoatingProcess().IsRunning();
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void StartHeating(object sender, RoutedEventArgs e)
        {
            recipeControl.GetHeatingProcess().SetParameters(1, 100.0f, 250, 200, 180);
            recipeControl.GetHeatingProcess().StartFlow();
        }

        private void StartHeatingStep2()
        {
            recipeControl.GetHeatingProcess().SetParameters(2, 100.0f, 250, 200, 60);
            recipeControl.GetHeatingProcess().StartFlow();
        }

        private void StartHeatingStep3()
        {
            recipeControl.GetHeatingProcess().SetParameters(3, 100.0f, 200, 250, 60);
            recipeControl.GetHeatingProcess().StartFlow();
        }

        private void StartPlasmaStep1()
        {
            recipeControl.GetPlasmaProcess().SetParameters(1, 150, 120.0f, 60, 0);
            recipeControl.GetPlasmaProcess().StartFlow();
        }

        private void StartPlasmaStep2()
        {
            recipeControl.GetPlasmaProcess().SetParameters(2, 150, 120.0F, 150, 180);
            recipeControl.GetPlasmaProcess().StartFlow();
        }

        private void StartPlasmaStep3()
        {
            recipeControl.GetPlasmaProcess().SetParameters(3, 150, 120.0F, 180, 120);
            recipeControl.GetPlasmaProcess().StartFlow();
        }

        private void StartSputteringStep1()
        {
            recipeControl.GetSputteringProcess().SetParameters(1, 180, 50.0f, 60, 0, 0, 0, 0, 0, 0, 0);
            recipeControl.GetSputteringProcess().StartFlow();
        }

        private void StartSputteringStep2()
        {
            recipeControl.GetSputteringProcess().SetParameters(2, 180, 50.0f, 120, 0, 0, 0, 0, 0, 0, 60);
            recipeControl.GetSputteringProcess().StartFlow();
        }

        private void StartCoatingStep1()
        {
            recipeControl.GetCoatingProcess().SetParameters(1, 100, 0, 40, 10, 70, 0, 0, 0, 0, 0, 0, 0);
            recipeControl.GetCoatingProcess().StartFlow();
        }

        private void StartCoatingStep2()
        {
            recipeControl.GetCoatingProcess().SetParameters(2, 100, 0, 100, 0, 60, 0, 0, 0, 0, 0, 0, 180);
            recipeControl.GetCoatingProcess().StartFlow();
        }

        private void StartCoatingStep3()
        {
            recipeControl.GetCoatingProcess().SetParameters(3, 100, 0, 100, 0, 60, 0, 60, 0, 0, 0, 0, 10);
            recipeControl.GetCoatingProcess().StartFlow();
        }

        private void SetBackgroundParameters(object sender, RoutedEventArgs e)
        {
            recipeControl.SetVacuumThreshold(Convert.ToSingle(TBVacuumThreshold.Text));
            client.GetDevices().GetVacuumSystem().GetTMP().SetRotationSpeedInPercentage(Convert.ToSingle(TBTMPSpeed.Text));
            client.GetDevices().GetCarouselSystem().GetVSD().SetMotorDirection(Convert.ToSingle(TBVSDSpeed.Text) > 0);
            client.GetDevices().GetCarouselSystem().GetVSD().SetMotorSpeedinRPM(Math.Abs(Convert.ToSingle(TBVSDSpeed.Text)));
        }

        private void StopAll(object sender, RoutedEventArgs e)
        {
            recipeControl.StopAllRecipeProcesses();
        }
    }
}

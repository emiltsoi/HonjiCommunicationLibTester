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
using HonjiS1200CommLib.Process;

namespace SiemensCommunicatinLibTester
{
    public partial class Processes : UserControl
    {
        IS1200Client client;
        IProcessControl process;

        public Processes()
        {
            InitializeComponent();
            var myWin = (MainWindow)Application.Current.MainWindow;
            client = myWin.client;
            process = client.GetProcesses();
            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(UpdateDisplay);
            dispatcherTimer.Interval = myWin.refreshRateTimeSpan;
            dispatcherTimer.Start();
        }

        private void UpdateDisplay(object sender, EventArgs e)
        {
            if (client.IsConnected())
            {
                UpdateProcessDisplay(process.GetLowVacuumFlow(), LVFState, LVFError);
                UpdateProcessDisplay(process.GetHighVacuumFlow(), HVFState, HVFError);
                UpdateProcessDisplay(process.GetVentFlow(), VFState, VFError);
                UpdateProcessDisplay(process.GetRecipe(), PFState, PFError);
                RunStep.IsEnabled = process.GetRecipe().IsReadyToExecuteRecipeStep();
                CanRunStep.IsChecked = process.GetRecipe().IsReadyToExecuteRecipeStep();
                StepElapsedTime.Text = process.GetRecipe().GetRemainingTimeOfCurrentStepInSecond().ToString() + "s";
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void LVFStartButton_Click(object sender, RoutedEventArgs e)
        {
            process.GetLowVacuumFlow().StartFlow();
        }

        private void LVFStopButton_Click(object sender, RoutedEventArgs e)
        {
            process.GetLowVacuumFlow().StopFlow();
        }

        private void HVFStartButton_Click(object sender, RoutedEventArgs e)
        {
            process.GetHighVacuumFlow().StartFlow();
        }

        private void HVFStopButton_Click(object sender, RoutedEventArgs e)
        {
            process.GetHighVacuumFlow().StopFlow();
        }

        private void VFStartButton_Click(object sender, RoutedEventArgs e)
        {
            process.GetVentFlow().StartFlow();
        }

        private void VFStopButton_Click(object sender, RoutedEventArgs e)
        {
            process.GetVentFlow().StopFlow();
        }

        private void UpdateProcessDisplay(ISimpleFlow flow, TextBox state, TextBox errors)
        {
            if (flow.IsRunning()){ state.Text = "Running"; }
            else if (flow.IsIdle()){ state.Text = "Idle"; }
            else if (flow.IsStopping()) { state.Text = "Stopping"; }
            errors.Text = "";
            foreach (var error in flow.GetErrors())
            {
                errors.Text += error + ", ";
            }
        }

        private void PFStartButton_Click(object sender, RoutedEventArgs e)
        {
            process.GetRecipe().StartFlow();
        }

        private void PFStopButton_Click(object sender, RoutedEventArgs e)
        {
            process.GetRecipe().StopFlow();
        }

        private void RunStep_Click(object sender, RoutedEventArgs e)
        {
            var settings = new RecipeSettings();
            settings.basePressureInPa = 3e-5f;
            settings.basePressureTimeoutTime = new TimeSpan(0, 60, 0);
            settings.biasCurrentLimitTimeout = new TimeSpan(0, 0, 5);
            settings.biasCurrentMaxLimitInAmpere = 10;
            settings.biasVoltageMaxVariationInVolt = 30;
            settings.biasVoltageVariationTimeout = new TimeSpan(0, 0, 5);
            settings.magnetronCurrentLimitTimeout = new TimeSpan(0, 0, 5);
            settings.magnetronCurrentMaxLimitInAmpere = 10;
            settings.magnetronCurrentMaxVariationInAmpere = 0.5f;
            settings.magnetronCurrentVariationTimeout = new TimeSpan(0, 0, 5);
            settings.mfcFlowRateMaxVariationInSccm = 10;
            settings.mfcFlowRateVariationTimeout = new TimeSpan(0, 0, 5);
            settings.sampleRotationMaxVariationInRPM = 1.0f;
            settings.sampleRotationSpeedInRPM = 3.0f;
            settings.sampleRotationVariationTimeout = new TimeSpan(0, 0, 5);
            settings.tmpRotationSpeedInHz = 440;
            process.GetRecipe().SetRecipeSettings(settings);

            var stepParams = new RecipeStepParameter();
            stepParams.arc1CurrentInAmpere = 1.0f;
            stepParams.arc2CurrentInAmpere = 1.1f;
            stepParams.arc3CurrentInAmpere = 1.2f;
            stepParams.arc4CurrentInAmpere = 1.3f;
            stepParams.arc5CurrentInAmpere = 1.4f;
            stepParams.arc6CurrentInAmpere = 1.5f;
            stepParams.biasVoltageInVolt = 100;
            stepParams.durationInSecond = 300;
            stepParams.heatingPlate1SetpointInC = 40;
            stepParams.heatingPlate2SetpointInC = 50;
            stepParams.heatingPlate3SetpointInC = 60;
            stepParams.mfc1FlowRateInSccm = 20;
            stepParams.mfc2FlowRateInSccm = 30;
            stepParams.stepNumber = 1;
            stepParams.stepType = RecipeStepType.Etching;
            process.GetRecipe().RunRecipeStep(stepParams);
        }
    }
}

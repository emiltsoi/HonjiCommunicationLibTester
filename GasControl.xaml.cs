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
    /// Interaction logic for GasControl.xaml
    /// </summary>
    public partial class GasControl : UserControl
    {
        IS1200Client client;
        IGasControlSystem gasControlSystem;

        public GasControl()
        {
            InitializeComponent();
            var myWin = (MainWindow)Application.Current.MainWindow;
            client = myWin.client;
            gasControlSystem = client.GetDevices().GetGasControlSystem();
            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(UpdateDisplay);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void UpdateDisplay(object sender, EventArgs e)
        {
            if (client.IsConnected())
            {
                DisplayUpdateHelper.UpdateMFCState(gasControlSystem.GetMFCAcetylene(), MFCH2FlowState, MFCH2ValveOpenState, MFCH2OnState);
                DisplayUpdateHelper.UpdateMFCState(gasControlSystem.GetMFCNitrogen(), MFCN2FlowState, MFCN2ValveOpenState, MFCN2OnState);
                DisplayUpdateHelper.UpdateMFCState(gasControlSystem.GetMFCArgon(), MFCArFlowState, MFCArValveOpenState, MFCArOnState);
            }
            CommandManager.InvalidateRequerySuggested();
        }
    }
}

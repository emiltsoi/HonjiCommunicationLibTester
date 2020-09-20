using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using HonjiS1200CommLib;
using HonjiS1200CommLib.Devices.BaseDevices;
using HonjiS1200CommLib.Devices.VacuumSystemDevices;
using HonjiS1200CommLib.Devices.GasControlDevices;

namespace SiemensCommunicatinLibTester
{
    class DisplayUpdateHelper
    {
        public static void UpdateValveState(IValve valve, TextBox state)
        {
            string stateText;
            if (valve.IsOpen())
                stateText = "Open";
            else if (valve.IsClosed())
                stateText = "Closed";
            else if (valve.IsInError())
                stateText = "Error";
            else
                stateText = "Unknown";
            state.Text = stateText;
        }

        public static void UpdateGaugeReading(IGauge gauge, TextBox state, TextBox reading)
        {
            if (gauge.IsInError())
                state.Text = "Error";
            else if (gauge.isInvalid())
                state.Text = "Invalid";
            else
                state.Text = "Normal";
            reading.Text = gauge.GetReadingInPa().ToString() + "Pa";
        }

        public static void UpdateMFCState(IMassFlowController mfc, TextBox reading, TextBox valveOpen, TextBox mfcOn)
        {
            reading.Text = mfc.GetFlowReading().ToString() + "sccm";
            valveOpen.Text = mfc.IsShutoffValveOpen() ? "Open" : "Close";
        }

        public static void UpdateSimpleOnOffDeviceState(ISimpleOnOffDevice device, TextBox state)
        {
            if (device.IsOn())
                state.Text = "On";
            else
                state.Text = "Off";
        }
    }
}

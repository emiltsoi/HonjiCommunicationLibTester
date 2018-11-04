using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using HonjiS1200CommLib;

namespace SiemensCommunicatinLibTester
{
    class DisplayUpdateHelper
    {
        public static void UpdateValveState(IValve valve, TextBox textbox)
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
            textbox.Text = stateText;
        }
    }
}

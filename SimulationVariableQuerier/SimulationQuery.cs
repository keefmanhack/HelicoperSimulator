using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.FlightSimulator.SimConnect;
using System.Runtime.InteropServices;


namespace SimulationVariableQuerier
{
    class SimulationQuery
    {
        SimConnect sc = null;
        public bool connectToSim()
        {
           const int WM_USER_SIMCONNECT = 0x0402;
            try
            {
                sc = new SimConnect("Managed Data Request", this.Handle, WM_USER_SIMCONNECT, null, 0);
                sc.OnRecvOpen += new SimConnect.RecvOpenEventHandler(SimConnect_OnRecvOpen);
            }
            catch(COMException ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            return true;
        }

        private void SimConnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
        {

        }

    }

}

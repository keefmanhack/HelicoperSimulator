using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FlightSimulator.SimConnect;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using Simvars;

namespace SimWatcher
{
    interface SysQuerier
    {
        // bool connectToSim(IntPtr ptr);
        // bool loadQueryVariables();
        // void requestSimData();
        // void printData();
        // bool disconnectFromSim();
    }

    class SysQuerierImp : SysQuerier
    {        
        SimConnectClient scc;
        SysVarLoader svl;
        string testFileLocation = @"D:\MSFS SDK\Samples\Test1\sysVars.txt";

        public SysQuerierImp(){
            scc = new SimConnectClient();
            scc.DataReceived += dataReceived;

            svl = new SysVarLoader();
            svl.setTxtFileLocation(testFileLocation);
            svl.loadSysVariables();
        }

        public bool handleDef(ref Message m)
        {
            return scc.handleDefWndProc(ref m);
        }


        public bool connectToSim(IntPtr ptr){
            return scc.attempClientConnect(ptr);
        }

        public void disconnectFromSim()
        {
            scc.disconnectFromSim();
        }

        public void loadRequestVariables(){
            List<object> varsAndUnits = svl.getVarsAndUnits();
            for(int i =0; i<varsAndUnits.Count; i++){
                keyUnit temp = (keyUnit)varsAndUnits[i];
                scc.addDataRequest(temp.key, temp.unit);
            }
        }

        public void requestSimData(){
            scc.requestSimData();
        }


        private void dataReceived(object sender, EventArgs e)
        {
            var x = (DataReceivedEventArgs)e;
            Console.WriteLine(x.name + ": " + x.value);
        }

    }
}

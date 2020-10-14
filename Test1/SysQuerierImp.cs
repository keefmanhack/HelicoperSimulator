using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FlightSimulator.SimConnect;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections.ObjectModel;

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

        public SysQuerierImp(){
            scc = new SimConnectClient();
            scc.DataReceived += dataReceived();

            svl = new SysVarLoader();
            svl.setTxtFileLocation("doesn't exists yet");
            svl.loadSysVariables();
        }


        public bool connectToSim(IntPtr ptr){
            return scc.attempClientConnect(IntPtr ptr);
        }

        public void loadRequestVariables(){
            List<object> varsAndUnits = svl.getVarsAndUnits();
            for(int i =0; i<varsAndUnits.size(); i++){
                scc.addDataRequest(varsAndUnits[i].var, varsAndUnits[i].unit);
            }
        }

        public void requestSimData(){
            scc.requestSimData();
        }


        private static void dataReceived(object sender, EventArgs e){
            Console.WriteLine("Data Received");
        }

    }
}

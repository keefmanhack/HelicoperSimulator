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
        bool connectToSim(IntPtr ptr);
        void loadRequestVariables();
        void requestSimData();
        void setRequestFrequency();
        // void printData();
        bool disconnectFromSim();
    }

    class SysQuerierImp : SysQuerier
    {   
        string TEST_FILE_LOCATION = @"D:\MSFS SDK\Samples\Test1\sysVars.txt";
        int MAX_QUEUE_COUNT = 5;

        SimConnectClient scc;
        SysVarLoader svl;
        private DispatcherTimer request_Timer;
        private DispatcherTimer queue_Timer;
        Queue<Hashtable> q;

        public event Notify DataDispatch;


        public SysQuerierImp(){
            scc = new SimConnectClient();
            scc.DataReceived += dataReceived;

            svl = new SysVarLoader();
            svl.setTxtFileLocation(TEST_FILE_LOCATION);
            svl.loadSysVariables();

            //setup for request timer
            request_Timer = new DispatcherTimer();
            request_Timer.Interval = new TimeSpan(0, 0, 0, 1, 0); //default to 1 second
            request_Timer.Tick += new EventHandler(requestSimData);

            //setup for queue pop timer
            queue_Timer = new DispatcherTimer();
            queue_Timer.Interval = new TimeSpan(0, 0, 0, 2, 0); //default to 1 second
            queue_Timer.Tick += new EventHandler(dispatchDequeue);

            q = new Queue<Hashtable>(MAX_QUEUE_COUNT);
        }

        public bool handleDef(ref Message m)
        {
            return scc.handleDefWndProc(ref m);
        }

        public void setRequestFrequency(TimeSpan time){
            request_Timer.Interval = time;
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

            //hashtable should have received all of the data by the next call of this function
            q.Enqueue(svl.getTable());
        }


        private void dataReceived(object sender, EventArgs e)
        {
            var x = (DataReceivedEventArgs)e;
            svl.setKeyValue(x.name, x.value);

            Console.WriteLine(x.name + ": " + x.value);
        }

        private void dispatchDequeue(){
            onDequeue(q.Dequeue());
        }

        protected virtual void onDequeue(Hashtable e) //protected virtual method
        {
            //if ProcessCompleted is not null then call delegate
            DataDispatch?.Invoke(this, e); 
        }

    }
}

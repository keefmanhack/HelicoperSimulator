namespace SimWatcher
{
    class SimulateState : State{
        PlatformController pc;
        SysQuerierImp sq;

        public SimulateState(){
            sq = new SysQuerierImp(Resources.simulateVars);
            sq.DataDispatch += handleSimResponse;
            pc = new PlatformController();
        }
        
        private void handleSimResponse(object sender, EventArgs e)
        {
            var data = (DequeueEventArgs)e;
            Hashtable t = data.t;
        }
        
    }
}
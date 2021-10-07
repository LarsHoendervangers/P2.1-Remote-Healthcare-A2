using Newtonsoft.Json.Linq;
using RemoteHealthcare_Client;
using RemoteHealthcare_Dokter.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Dokter.BackEnd
{
    class DashboardManager : DataManager
    {
        private DashboardViewModel dashboard;

        public DashboardManager(DashboardViewModel dashboard)
        {
            this.dashboard = dashboard;
        }



        public override void ReceivedData(JObject data)
        {
            throw new NotImplementedException();
        }

        private void HandleIncoming(JObject data)
        {

        }

        public void RequestActiveClients()
        {

        }

        public void SendAbort(int id)
        {

        }

        public void SendResistance(int id, int res)
        {

        }

        public void BroadcastMessage(string message)
        {

        }

        public void StartSession(int id)
        {

        }

        public void SubToPatient(int id)
        {
            
        }

        private void WrapIncomingClients(JObject data)
        {

        }

        private void WrapIncomingErgoData(JObject data)
        {

        }

    }
}

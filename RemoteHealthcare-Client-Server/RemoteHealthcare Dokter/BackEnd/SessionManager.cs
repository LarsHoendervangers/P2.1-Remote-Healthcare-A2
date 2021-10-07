using Newtonsoft.Json.Linq;
using RemoteHealthcare_Client;
using RemoteHealthcare_Dokter.ViewModels;
using RemoteHealthcare_Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Dokter.BackEnd
{
    class SessionManager : DataManager
    {
        private Patient patient;
        private Session session;
        private SessionDetailViewModel DetailViewModel;

        public SessionManager(Patient patient, Session session, SessionDetailViewModel sessionDetailViewModel)
        {
            this.patient = patient;
            this.session = session;
            this.DetailViewModel = sessionDetailViewModel;
        }

        public override void ReceivedData(JObject data)
        {
            throw new NotImplementedException();
        }

        private void HandleIncoming(JObject data)
        {

        }

        private void HandleIncomingErgoData(JObject data)
        {

        }

        public void SendAbort()
        {

        }

        public void SendResistance(int res)
        {

        }

        public void SendStopSession()
        {

        }

        public void SendMessage(string message)
        {

        }
    }
}

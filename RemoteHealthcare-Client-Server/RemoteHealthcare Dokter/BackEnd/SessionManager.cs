using Newtonsoft.Json.Linq;
using RemoteHealthcare_Client;
using RemoteHealthcare_Dokter.ViewModels;
using RemoteHealthcare_Server;
using RemoteHealthcare_Shared.DataStructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Dokter.BackEnd
{
    class SessionManager : DataManager
    {

        public SessionManager() { }

        public SessionManager(SharedPatient patient)
        {
            SubscribeToPatient(patient);
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

        private void SubscribeToPatient(SharedPatient patient)
        {
            string[] patientsIDs = new string[] { patient.ID };

            // The command to subscribe to a patient at the server
            object o = new
            {
                command = "subtopatient",
                data = patientsIDs
            };
        }


    }
}

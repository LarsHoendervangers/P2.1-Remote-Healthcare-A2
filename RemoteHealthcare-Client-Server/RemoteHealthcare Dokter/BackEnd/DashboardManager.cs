using Newtonsoft.Json.Linq;
using RemoteHealthcare_Client;
using RemoteHealthcare_Dokter.ViewModels;
using RemoteHealthcare_Shared.DataStructs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Dokter.BackEnd
{
    class DashboardManager : DataManager
    {
        public event EventHandler<List<SharedPatient>> OnPatientUpdated;

        public DashboardManager()
        {
            RequestActiveClients();
        }

        public override void ReceivedData(JObject data)
        {
            JToken value;

            bool correctCommand = data.TryGetValue("command", StringComparison.InvariantCulture, out value);

            // Return if the parsing of command was not succesfull
            if (!correctCommand) return;
           
            switch (value.ToString())
            {
                case "getactivepatients":
                    // Setting the command to the command to ask for detailed data and sending to server
                    data["command"] = "getdetailpatient";
                    SendToManagers(data);
                    break;
                case "detaildata":
                    ParseIncomingPatients(data);
                    break;

            }
        }

        public void RequestActiveClients()
        {
            // Command to request all the logged in clients, see dataprotocol
            object o = new
            {
                command = "getactivepatients",
            };

            SendToManagers(JObject.FromObject(o));
        }

        private void ParseIncomingPatients(JObject data)
        {
            JArray patientIDs = data.GetValue("data") as JArray;

            List<SharedPatient> patients = new List<SharedPatient>();
            foreach(JObject jo in patientIDs)
            {
                patients.Add(jo.ToObject<SharedPatient>());
            }

            this.OnPatientUpdated.Invoke(this, patients);
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

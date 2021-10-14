using Newtonsoft.Json.Linq;
using RemoteHealthcare_Client;
using RemoteHealthcare_Dokter.ViewModels;
using RemoteHealthcare_Server;
using RemoteHealthcare_Shared.DataStructs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Dokter.BackEnd
{
    class SessionManager : DataManager
    {
        public List<HRMeasurement> hRMeasurements;
        public List<BikeMeasurement> bikeMeasurements;

        public event EventHandler NewDataTriggered;

        public SessionManager() { }

        public SessionManager(SharedPatient patient)
        {
            SubscribeToPatient(patient);
        }

        public override void ReceivedData(JObject data)
        {
            JToken value;

            bool correctCommand = data.TryGetValue("command", StringComparison.InvariantCulture, out value);

            // Return if the parsing of command was not succesfull
            if (!correctCommand) return;

            switch (value.ToString())
            {
                case "livepatientdata":
                    // Setting the command to the command to ask for detailed data and sending to server
                    HandleIncomingErgoData(data);
                    break;
            }
        }

        private void HandleIncomingErgoData(JObject data)
        {
            Trace.WriteLine($"DATA KOMT BINNEN ERGO LIVER HETLPELPLPFLS {data}");
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

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
        private SharedPatient Patient;

        public SessionManager() { }

        public SessionManager(SharedPatient patient)
        {
            SubscribeToPatient(patient, false);
            this.Patient = patient;
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

        private void SubscribeToPatient(SharedPatient patient, bool unsubscribe)
        {
            string[] patientsIDs = new string[] { patient.ID };
             
            // The command to subscribe to a patient at the server
            object o = new
            {
                command = "subtopatient",
                data = new
                {
                    patid = patientsIDs,
                    state = unsubscribe ? 1 : 0
                }
            };

            SendToManagers(JObject.FromObject(o));
        }

        public void AbortSession()
        {
            string[] patients = new string[] { this.Patient.ID };

            // JSON object to start a new session
            object o = new
            {
                command = "abort",
                data = new
                {
                    patid = patients
                }
            };

            //Asking the sever to start a session, no further actions are taken until new userineraction
            this.SendToManagers(JObject.FromObject(o));
        }

        public void PersonalMessage(string message)
        {
            string[] patients = new string[] { this.Patient.ID };

            object o = new
            {
                command = "message",
                data = new
                {
                    message = message,
                    patid = patients
                },
                flag = "2"
            };

            this.SendToManagers(JObject.FromObject(o));
        }
    }
}

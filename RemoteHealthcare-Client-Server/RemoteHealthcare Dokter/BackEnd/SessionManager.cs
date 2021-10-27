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
        public List<HRMeasurement> HRMeasurements;
        public List<BikeMeasurement> BikeMeasurements;

        public event EventHandler NewDataTriggered;

        public SharedPatient Patient;

        public SessionManager(SharedPatient patient)
        {
            this.Patient = patient;

            this.HRMeasurements = new List<HRMeasurement>();
            this.BikeMeasurements = new List<BikeMeasurement>();

            SubscribeToPatient(patient, true);
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
            // Getting the data object from 
            JToken dataObject = (data as JToken).SelectToken("data.data");

            if (dataObject == null) return;

            // Ignore if patient ID is not the current patient (for redundantie + expandability)
            JToken patienIDToken = (data as JToken).SelectToken("data.id");
            if (patienIDToken == null || patienIDToken.ToString() != this.Patient.ID) return;

            // Determine if the incoming data is HR or bike readings
            if (dataObject.SelectToken("CurrentHeartrate") != null)
                this.HRMeasurements.Add(JSONConverter.ConvertHRObject(dataObject as JObject));
            else
                this.BikeMeasurements.Add(JSONConverter.ConverBikeObject(dataObject as JObject));

            this.NewDataTriggered?.Invoke(this, null);
        }

        private void SubscribeToPatient(SharedPatient patient, bool subscribe)
        {
            string[] patientsIDs = new string[] { patient.ID };
             
            // The command to subscribe to a patient at the server
            object o = new
            {
                command = "subtopatient",
                data = new
                {
                    patid = patientsIDs,
                    state = subscribe ? 0 : 1
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

        public void SetResistance(int value)
        {
            string[] patients = new string[] { this.Patient.ID };

            object o = new
            {
                command = "setresist",
                data = new
                {
                    value = value,
                    patid = patients
                },
            };

            this.SendToManagers(JObject.FromObject(o));
        }

        public void CloseManager()
        {
            // performing all the actions needed when the window is closed
            SubscribeToPatient(this.Patient, false);
        }

        public void StopSession(SharedPatient patient)
        {
            string[] patients = new string[] { patient.ID };

            // JSON object to start a new session
            object o = new
            {
                command = "newsession",
                data = new
                {
                    patid = patients,
                    state = 1
                }
            };

            //Asking the sever to start a session, no further actions are taken until new userineraction
            this.SendToManagers(JObject.FromObject(o));
        }
    }
}

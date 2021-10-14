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
        private SharedPatient Patient;

        public SharedPatient Patient;

        public SessionManager(SharedPatient patient)
        {
            this.Patient = patient;

            this.HRMeasurements = new List<HRMeasurement>();
            this.BikeMeasurements = new List<BikeMeasurement>();

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
            // Getting the data object from 
            JToken commandData;
            

            if (data.TryGetValue("data", out commandData)) return;

            // Casting the token to a usable object
            JObject dataObject = commandData as JObject;

            string patientID = (commandData as JObject)?.GetValue("id").ToString();

            // Ignore if patient ID is not the current patient (for redundantie + expandability)
            if (patientID != this.Patient.ID) return;

            // Determine if the incoming data is HR or bike readings
            bool isHR = data.TryGetValue("data", out commandData);

            if (isHR)
                this.HRMeasurements.Add(ConvertHRObject(dataObject));
            else
                this.BikeMeasurements.Add(ConverBikeObject(dataObject));


        }

        private HRMeasurement ConvertHRObject(JObject dataObject)
        {
            return new HRMeasurement(
                DateTime.Parse(dataObject.GetValue("MeasurementTime").ToString()),
                int.Parse(dataObject.GetValue("CurrentHeartrate").ToString())
                );
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
            

            // JSON object to start a new session
            object o = new
            {
                command = "abort",
                data = new
                {
                    
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

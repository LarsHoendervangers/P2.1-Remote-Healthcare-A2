﻿using Newtonsoft.Json.Linq;
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

        /// <summary>
        /// Method which checks whether a command is valid and calls the right method belong to the command
        /// </summary>
        /// <param name="data"></param>
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

        /// <summary>
        /// Method which checks if the data from the JObjebct is pasable and adds a new HRMeasurement and BikeMeasurement to their
        /// corresponding lists. Also invokes the NewDataTriggered event
        /// </summary>
        /// <param name="data"></param>
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

        /// <summary>
        /// Method which sends an object to all handlers asking to sub to all patients
        /// from the list from the parameters
        /// </summary>
        /// <param name="patient"></param>
        /// <param name="subscribe"></param>
        public void SubscribeToPatient(SharedPatient patient, bool subscribe)
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

        /// <summary>
        /// method which sends an object to all the handlers asking to abort an session
        /// </summary>
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

        /// <summary>
        /// Method which sends an object to all the handlers asking to send a personal message to a 
        /// specific person.
        /// </summary>
        /// <param name="message"></param>
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

        /// <summary>
        /// method which sends an object to all the handlers to set the resistance of the bike to a certain value for a psecific person
        /// </summary>
        /// <param name="value"></param>
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

        /// <summary>
        /// Method whihc unsubscribes to a certain patient when the window is closed
        /// </summary>
        public void CloseManager()
        {
            // performing all the actions needed when the window is closed
            SubscribeToPatient(this.Patient, false);
        }

        /// <summary>
        /// Method which sends an object to all hadlers asking to stop the session with a specific person
        /// </summary>
        /// <param name="patient"></param>
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

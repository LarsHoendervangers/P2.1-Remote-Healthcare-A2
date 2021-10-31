using Newtonsoft.Json.Linq;
using RemoteHealthcare_Client;
using RemoteHealthcare_Dokter.ViewModels;
using RemoteHealthcare_Shared.DataStructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RemoteHealthcare_Dokter.BackEnd
{
    /// <summary>
    /// Class is a DataManger, so it can send data to the server.
    /// It handles getting all the patients and patient sessions, used in historic data.
    /// </summary>
    class PatientManager : DataManager
    {
        public event EventHandler<List<SharedPatient>> OnPatientsReceived;
        public event EventHandler<List<SessionWrap>> OnSessionReceived;

        /// <summary>
        /// Constructor for the PatientManager.
        /// </summary>
        public PatientManager()
        {
            GetAllPatients();
        }

        /// <summary>
        /// Method that is called when data is received from the server, is handles the following commands:
        /// - Patient data comming from the server for all the clients that have had sessions
        /// - All the saved sessions of a patient.
        /// The method only checks for these commands, when detected it calles the method to handles that specific command
        /// </summary>
        /// <param name="data">The data received</param>
        public override void ReceivedData(JObject data)
        {
            JToken value;

            // Try to get the command tab of the data
            bool correctCommand = data.TryGetValue("command", StringComparison.InvariantCulture, out value);

            // Return if the parsing of command was not succesfull
            if (!correctCommand) return;

            switch (value.ToString())
            {
                case "getallpatients":
                    // Setting the command to the command to ask for detailed data and sending to server
                    data["command"] = "getdetailpatient";
                    SendToManagers(data);
                    break;
                case "detaildata":
                    HandleIncomingPatients(data);
                    break;
                case "getsessions":
                    // Calling the method that handles the getsessions command
                    HandleIncomingSessions(data);
                    break;
            }
        }

        /// <summary>
        /// This method handles the converstion for the getsessions command:
        /// it gets the patientID and names
        /// </summary>
        /// <param name="data">The data with the getsessions command</param>
        private void HandleIncomingPatients(JObject data)
        {
            // Getting the data field on the json object
            JArray patientIDs = data.GetValue("data") as JArray;

            // looping through all the patients in the json
            List<SharedPatient> patients = new List<SharedPatient>();
            foreach (JObject jo in patientIDs)
            {
                // Converting the object to a SharedPatient object
                patients.Add(jo.ToObject<SharedPatient>());
            }

            // Invoking the event to tell the GUI to update the list
            this.OnPatientsReceived?.Invoke(this, patients);
        }

        /// <summary>
        /// Method which handles the incoming sessions. First checks whether all the data can be found and
        /// parsed from the JObject. next it creates an SessionWrap for each session that has been found 
        /// within the JArray parsed from the JObject
        /// </summary>
        /// <param name="data"></param>
        private void HandleIncomingSessions(JObject data)
        {
            // Getting the patientID and date fields on the json object
            JToken patientID = data.SelectToken("data.patientid");
            JArray startDates = data.SelectToken("data.startdates") as JArray;
            JArray endDates = data.SelectToken("data.enddates") as JArray;

            // Checking for possible errors in data: objects could not parse && if Arrays match in size
            // Resending the getSession command if the patientID is oke
            if (patientID == null) return;
            if (startDates == null || endDates == null || startDates.Count != endDates.Count) GetSessions(patientID.ToString());

            // Looping trough dates and creating sessions
            List<SessionWrap> sessions = new List<SessionWrap>();
            for (int i = 0; i < startDates.Count; i++)
            {
                sessions.Add(new SessionWrap(null, null, DateTime.Parse(startDates[i].ToString()), DateTime.Parse(endDates[i].ToString())));
            }

            // Invoking the event to tell the GUI to update the list
            this.OnSessionReceived?.Invoke(this, sessions);
        }

        /// <summary>
        /// Method which sends an object asking for all the clients from the server to all the handlers
        /// </summary>
        public void GetAllPatients()
        {
            // Command to request all clients that are stored in the server, see dataprotocol
            object o = new
            {
                command = "getallpatients",
            };

            SendToManagers(JObject.FromObject(o));
        }

        /// <summary>
        /// Method creates the json object to request all sessions from a given patient.
        /// </summary>
        /// <param name="userID">The ID of the patient data is asked from</param>
        public void GetSessions(string userID)
        {
            // Command to request all sessions from a given patients
            object o = new
            {
                command = "getsessions",
                data = userID
            };

            SendToManagers(JObject.FromObject(o));
        }
    }
}

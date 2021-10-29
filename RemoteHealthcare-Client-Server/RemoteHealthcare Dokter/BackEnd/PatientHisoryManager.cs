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
    class PatientHisoryManager : DataManager
    {
        public event EventHandler<SessionWrap> OnSessionUpdate;
        SessionWrap session;
        
        public PatientHisoryManager(SessionWrap session, string userID)
        {
            this.session = session;

            GetSessionData(userID);
        }

        /// <summary>
        /// Method which checks whether a command is valid and calls the right method belong to the command
        /// </summary>
        /// <param name="data"></param>
        public override void ReceivedData(JObject data)
        {
            JToken value;

            // Try to get the command tab of the data
            bool correctCommand = data.TryGetValue("command", StringComparison.InvariantCulture, out value);

            // Return if the parsing of command was not succesfull
            if (!correctCommand) return;

            switch (value.ToString())
            {
                case "getsessionsdetails":
                    // Calling the method that handles the getsessions command
                    HandleIncomingSession(data);
                    break;
            }
        }

        /// <summary>
        /// Method which sends an object asking for all the current data from a user to all the handlers
        /// </summary>
        /// <param name="userID"></param>
        public void GetSessionData(string userID)
        {
            // Command to request details from a session, see dataprotocol
            object o = new
            {
                command = "getsessionsdetails",
                data = new
                {
                    patid = userID,
                    date = session.Enddate
                }
            };

            SendToManagers(JObject.FromObject(o));
        }

        /// <summary>
        /// Method which is called on when a response from rhe server for the getsessiondata method. 
        /// Checks whether all the data from the JObject can be found and parsed and creates a 
        /// BikeMeasuremnt and HRMeasurement List. Also calls the invoke of the OnSessionUpdate event
        /// </summary>
        /// <param name="data"></param>
        private void HandleIncomingSession(JObject data)
        {
            // Getting the patientID and sesion fields on the json object
            JToken patientID = data.SelectToken("data.patientid");
            JObject session = data.SelectToken("data.session") as JObject;
            
            // Checking if the fields exist
            if (patientID == null || session == null) return;

            JArray hrJson = session.SelectToken("hrdata") as JArray;
            JArray bikeJson = session.SelectToken("bikedata") as JArray;

            // Returning if the values are null
            if (hrJson == null || bikeJson == null) return;

            List<HRMeasurement> hRMeasurements = new List<HRMeasurement>();
            foreach(JObject hr in hrJson)
            {
                hRMeasurements.Add(JSONConverter.ConvertHRObject(hr));
            }

            List<BikeMeasurement> bikeMeasurements = new List<BikeMeasurement>();
            foreach (JObject bike in bikeJson)
            {
                bikeMeasurements.Add(JSONConverter.ConverBikeObject(bike));
            }

            this.session.BikeMeasurements = bikeMeasurements;
            this.session.HRMeasurements = hRMeasurements;
            // Invoking the event to tell the GUI to update the list
            this.OnSessionUpdate?.Invoke(this, this.session);
        }
    }
}

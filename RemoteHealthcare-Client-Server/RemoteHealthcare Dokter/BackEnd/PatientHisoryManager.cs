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
        public event EventHandler<Session> OnSessionUpdate;
        SessionWrap session;
        
        public PatientHisoryManager(SessionWrap session, string userID)
        {
            this.session = session;

            GetSessionData(userID);
        }

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

        private void HandleIncomingSession(JObject data)
        {
            // Getting the patientID and sesion fields on the json object
            JToken patientID = data.SelectToken("data.patientid");
            JObject session = data.SelectToken("data.session") as JObject;
            
            // Checking if the fields exist
            if (patientID == null || session == null) return;

            //Parsing the measurements
            JObject startDates = session.SelectToken("startdates") as JObject;
            JObject endDates = session.SelectToken("enddates") as JObject;

            JArray hrJson = session.SelectToken("hrdata") as JArray;
            JArray bikeJson = session.SelectToken("bikedata") as JArray;

            // Returning if the values are null
            if (startDates == null || endDates == null || hrJson == null || bikeJson == null) return;

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
            //this.OnSessionReceived?.Invoke(this, sessions);
        }
    }
}

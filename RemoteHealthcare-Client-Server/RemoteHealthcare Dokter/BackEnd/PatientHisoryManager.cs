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
    class PatientHisoryManager : DataManager
    {

        public PatientHisoryManager(PatientHistoryViewModel historyViewModel, SessionWrap session, string userID)
        {
            HistoryViewModel = historyViewModel;

            GetSessionData(session.Enddate, userID);
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
                    WrapSessionData(data);
                    break;
            }
        }

        private void HandleIncoming(JObject data)
        {

        }

        public void GetSessionData(DateTime dateTime, string userID)
        {

        }

        private void WrapSessionData(JObject data)
        {

        }
    }
}

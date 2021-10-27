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
        private PatientHistoryViewModel HistoryViewModel;

        public PatientHisoryManager(PatientHistoryViewModel historyViewModel, SessionWrap session)
        {
            HistoryViewModel = historyViewModel;
        }

        public override void ReceivedData(JObject data)
        {
            throw new NotImplementedException();
        }

        private void HandleIncoming(JObject data)
        {

        }

        public void GetPreviousSessions(int id)
        {

        }

        private void WrapPreviousSessionResponse(JObject data)
        {

        }

        public void GetPatientData(int id)
        {

        }

        private void WrapPatientData(JObject data)
        {

        }
    }
}

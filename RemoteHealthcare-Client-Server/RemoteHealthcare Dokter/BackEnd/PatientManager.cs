using Newtonsoft.Json.Linq;
using RemoteHealthcare_Client;
using RemoteHealthcare_Dokter.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Dokter.BackEnd
{
    class PatientManager : DataManager
    {
        private PatientListViewModel PatientViewModel;

        public PatientManager(PatientListViewModel patientViewModel)
        {
            PatientViewModel = patientViewModel;
        }

        public override void ReceivedData(JObject data)
        {
            throw new NotImplementedException();
        }

        private void HandleIncoming(JObject data)
        {

        }

        public List<User> GetAllPatients()
        {
            return null;
        }

        private void HandleIncomingPatients(JObject data)
        {

        }
    }
}

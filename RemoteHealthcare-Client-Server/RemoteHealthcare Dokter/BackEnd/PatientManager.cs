using Newtonsoft.Json.Linq;
using RemoteHealthcare_Client;
using RemoteHealthcare_Dokter.ViewModels;
using RemoteHealthcare_Shared.DataStructs;
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

        public List<SharedPatient> GetAllPatients()
        {
            List<SharedPatient> patients = new List<SharedPatient>();

            patients.Add(new SharedPatient("Twan", "Van Noorloos", "1", false, DateTime.Now));

            return patients;
        }

        private void HandleIncomingPatients(JObject data)
        {

        }
    }
}

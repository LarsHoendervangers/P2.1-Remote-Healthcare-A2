using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Dokter.ViewModels
{
    class PatientListViewModel
    {
        private List<User> _PatientsList;
        public List<User> PatientList
        {
            get { return _PatientsList; }
            set { _PatientsList = value; }
        }
    }
}

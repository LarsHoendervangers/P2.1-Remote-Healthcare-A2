using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Dokter.ViewModels
{
    class PatientViewModel
    {
        private List<string> _PatientsList;
        public List<string> PatientList
        {
            get { return _PatientsList; }
            set { _PatientsList = value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RemoteHealthcare_Dokter.ViewModels
{
    class PatientHistoryViewModel
    {
        private Window window;
        private User Patient;

        public PatientHistoryViewModel(Window window, User selectedPatient)
        {
            this.window = window;
            this.Patient = selectedPatient;
        }
    }
}

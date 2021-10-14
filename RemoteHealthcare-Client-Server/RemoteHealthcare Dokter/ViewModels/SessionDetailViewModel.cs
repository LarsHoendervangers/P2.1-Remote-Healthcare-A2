using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using LiveCharts.Geared;
using RemoteHealthcare_Dokter.BackEnd;
using RemoteHealthcare_Shared.DataStructs;

namespace RemoteHealthcare_Dokter.ViewModels
{
    class SessionDetailViewModel
    {
        private Window window;
        private SharedPatient Patient;
        private SessionManager manager;

        public SessionDetailViewModel(Window window, SharedPatient patient)
        {
            this.window = window;
            this.Patient = patient;

            this.manager = new SessionManager();
        }

        private SharedPatient _SelectedSessionPatient;
        public SharedPatient SelectedSessionPatient
        {
            get { return _SelectedSessionPatient; }
            set
            {
                _SelectedSessionPatient = value;
                HandleSessionPatientClicked();
            }
        }

        private void HandleSessionPatientClicked()
        {
            this.window.Content = new SessionDetailViewModel(this.window, SelectedSessionPatient);
        }
    }
}

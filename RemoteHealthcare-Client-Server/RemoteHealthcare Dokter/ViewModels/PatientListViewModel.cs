using RemoteHealthcare_Client;
using RemoteHealthcare_Dokter.BackEnd;
using RemoteHealthcare_Shared.DataStructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RemoteHealthcare_Dokter.ViewModels
{
    class PatientListViewModel
    {
        private Window window;
        private PatientManager manager;

        public PatientListViewModel(Window window)
        {
            this.window = window;
            this.manager = new PatientManager(this);

            Application.Current.Dispatcher.Invoke(() =>
            {
                this.PatientList = this.manager.GetAllPatients();
            });   
        }

        private List<SharedPatient> _PatientsList;
        public List<SharedPatient> PatientList
        {
            get { return _PatientsList; }
            set { _PatientsList = value; }
        }

        private ICommand _SwitchToDashboardView;
        public ICommand SwitchToDashboardView
        {
            get
            {
                if (_SwitchToDashboardView == null)
                {
                    _SwitchToDashboardView = new GeneralCommand(
                        param => SwitchView()
                        );
                }
                return _SwitchToDashboardView;
            }

        }

        private void SwitchView()
        {
            this.window.Content = new DashboardViewModel(window);
        }

        private SharedPatient _SelectedUser;
        public SharedPatient SelectedPatient
        {
            get { return _SelectedUser; }
            set
            {
                _SelectedUser = value;
                OpenHistoryWindow();
            }
        }

        private void OpenHistoryWindow()
        {
            this.window.Content = new PatientHistoryViewModel(this.window, SelectedPatient);
        }
    }
}

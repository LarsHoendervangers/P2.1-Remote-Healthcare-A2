using RemoteHealthcare_Client;
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

        public PatientListViewModel(Window window)
        {
            this.window = window;
        }

        private List<User> _PatientsList;
        public List<User> PatientList
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
    }
}

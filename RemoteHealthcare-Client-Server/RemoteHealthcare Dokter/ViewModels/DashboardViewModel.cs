using Prism.Commands;
using RemoteHealthcare_Client;
using RemoteHealthcare_Dokter.BackEnd;
using RemoteHealthcare_Server;
using RemoteHealthcare_Shared.DataStructs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RemoteHealthcare_Dokter.ViewModels
{
    class DashboardViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Window window;
        private DashboardManager manager;

        public DashboardViewModel(Window window)
        {
            this.window = window;

            this.manager = new DashboardManager();
            this.manager.OnPatientUpdated += (s, d) =>
            {
                List<SharedPatient> ActiveSessionPatients = new List<SharedPatient>();
                List<SharedPatient> AllPatients = new List<SharedPatient>();

                foreach (SharedPatient p in d)
                {
                    if (p.InSession)
                    {
                        ActiveSessionPatients.Add(p);
                    }

                    AllPatients.Add(p);
                }

                this.AllPatients = AllPatients;
                this.InSessionPatients = ActiveSessionPatients;
            };
        }

        private ICommand _AddSessionCommand;
        public ICommand AddSessionCommand
        {
            get
            {
                if (_AddSessionCommand == null)
                {
                    _AddSessionCommand = new GeneralCommand(
                        param => ShowPopUp()
                        );
                }
                return _AddSessionCommand;
            }

        }

        private void ShowPopUp()
        {
            SessionPopUp s = new SessionPopUp();
            s.Show();
        }

        private ICommand _SendMessageCommand;
        public ICommand SendMessageCommand
        {
            get
            {
                if (_SendMessageCommand == null)
                {
                    _SendMessageCommand = new GeneralCommand(
                        param => SendMessage()
                        );
                }
                return _SendMessageCommand;
            }

        }

        private void SendMessage()
        {
            MessageBox.Show("Ja haai");
        }


        private List<SharedPatient> _Patients;
        public List<SharedPatient> AllPatients
        {
            get { return _Patients; }
            set
            {
                _Patients = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AllPatients"));
            }
        }

        private List<SharedPatient> _InSessionPatients;
        public List<SharedPatient> InSessionPatients
        {
            get { return _InSessionPatients; }
            set
            {
                _InSessionPatients = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("InSessionPatients"));
            }
        }

        private ICommand _SwitchToPatientView;
        public ICommand SwitchToPatientView
        {
            get
            {
                if (_SwitchToPatientView == null)
                {
                    _SwitchToPatientView = new GeneralCommand(
                        param => SwitchView()
                        );
                }
                return _SwitchToPatientView;
            }

        }

        private void SwitchView()
        {
            this.window.Content = new PatientListViewModel(this.window);
        }
    }
}

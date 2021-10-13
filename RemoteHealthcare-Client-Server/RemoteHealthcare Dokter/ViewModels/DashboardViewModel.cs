using Prism.Commands;
using RemoteHealthcare_Client;
using RemoteHealthcare_Dokter.BackEnd;
using RemoteHealthcare_Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RemoteHealthcare_Dokter.ViewModels
{
    class DashboardViewModel
    {
        private Window window;

        public DashboardViewModel(Window window)
        {
            this.window = window;
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


        private List<Session> _SessionsList;
        public List<Session> SessionsList
        {
            get { return _SessionsList; }
            set { _SessionsList = value; }
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
            this.window.Content = new PatientListViewModel();
        }
    }
}

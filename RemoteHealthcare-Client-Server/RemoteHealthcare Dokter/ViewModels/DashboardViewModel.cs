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


        private List<string> _SessionsList;
        public List<string> SessionsList
        {
            get { return _SessionsList; }
            set { _SessionsList = value; }
        }
    }
}

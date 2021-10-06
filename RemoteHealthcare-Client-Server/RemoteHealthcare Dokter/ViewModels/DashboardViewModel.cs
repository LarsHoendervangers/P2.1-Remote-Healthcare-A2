using Prism.Commands;
using RemoteHealthcare_Client;
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

        /// <summary>
        /// Calls the Loader class to setup the connection to the servers/
        /// </summary>
        private void ShowPopUp()
        {
            SessionPopUp s = new SessionPopUp();
            s.Show();
        }

        private List<Session> _SessionsList;
        public List<Session> SessionsList
        {
            get { return _SessionsList; }
            set { _SessionsList = value; }
        }
    }
}

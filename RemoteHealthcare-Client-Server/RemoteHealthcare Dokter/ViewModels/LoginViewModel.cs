using RemoteHealthcare_Client;
using RemoteHealthcare_Dokter.BackEnd;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RemoteHealthcare_Dokter.ViewModels
{
    class LoginViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public LoginManager manager;

        public LoginViewModel()
        {
            this.manager = new LoginManager();
        }

        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set
            {
                Trace.WriteLine(value);
                _UserName = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Username"));
            }
        }

        private string _Password;
        public string Password
        {
            get { return _Password; }
            set
            {
                Trace.WriteLine(value);
                _Password = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Password"));
            }
        }

        private ICommand _SendLoginCommand;
        public ICommand SendLoginCommand
        {
            get
            {
                if (_SendLoginCommand == null)
                {
                    _SendLoginCommand = new GeneralCommand(
                        param => SendMessage(UserName, Password)
                        );
                }
                return _SendLoginCommand;
            }

        }

        private void SendMessage(string UserName, string Password)
        {
            string message = "User " + UserName + " Pass " + Password;
            MessageBox.Show(message);

            this.manager.SendLogin(UserName, Password);
        }
    }
}

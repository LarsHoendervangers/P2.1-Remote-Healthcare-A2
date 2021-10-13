using GalaSoft.MvvmLight.Command;
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

        public event EventHandler CloseRequest;
        public RelayCommand<Window> CloswWindowCommand { get; private set; }

        private Window window;

        public LoginViewModel()
        {
            this.CloswWindowCommand = new RelayCommand<Window>(this.OnButtonClick);

            this.manager = new LoginManager();


            this.manager.OnLoginResponseReceived += (s, d) =>
            {
                if (!d)
                {
                    MessageBox.Show("Username or password invalid!");
                    return;
                } 

                StartDokterGUI();
            };
        }

        private void StartDokterGUI()
        {
            Application.Current.Dispatcher?.Invoke(() =>
            {               
                var win = new Window();
                win.Content = new MainWindowViewModel();
                win.Show();

                this.window.Close();
            });
        }

        private void OnButtonClick(Window window)
        {
            this.window = window;
            Trace.WriteLine("Piew");
            SendMessage(UserName, Password);
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

        /*
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
        */

        private void SendMessage(string UserName, string Password)
        {
            string message = "User " + UserName + " Pass " + Password;

            this.manager.SendLogin(UserName, Password);
        }
    }
}

﻿using GalaSoft.MvvmLight.Command;
using RemoteHealthcare_Client;
using RemoteHealthcare_Dokter.BackEnd;
using RemoteHealthcare_Dokter.Views;
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

            App.Current.MainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            App.Current.MainWindow.Width = 350;
            App.Current.MainWindow.Height = 250;
            App.Current.MainWindow.Title = "Login";

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

        /// <summary>
        /// Method which opens a new window with a DashboardViewModel and closes the current window, all on a different thread than the WPF
        /// </summary>
        private void StartDokterGUI()
        {
            Application.Current.Dispatcher?.Invoke(() =>
            {               
                var win = new Window();
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Width = 1780;
                win.Height = 1000;
                win.Content = new DashboardViewModel(win);
                win.Closed += (s, e) => Environment.Exit(Environment.ExitCode);
                win.Show();

                LoginView view = ((this.window) as LoginView);
                if(view != null)
                    view.Shutdown = false;

                this.window.Close();
            });
        }

        /// <summary>
        /// Method which gets the current window as a paramter and sends the Login message to the manager
        /// </summary>
        /// <param name="window"></param>
        private void OnButtonClick(Window window)
        {
            this.window = window;
            SendMessage(UserName, Password);
        }

        private void SendMessage(string UserName, string Password)
        {
            this.manager.SendLogin(UserName, Password);
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
                _Password = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Password"));
            }
        }

        
    }
}

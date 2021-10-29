using RemoteHealthcare_Client;
using RemoteHealthcare_Dokter.BackEnd;
using RemoteHealthcare_Server;
using RemoteHealthcare_Shared.DataStructs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RemoteHealthcare_Dokter.ViewModels
{
    class PatientListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Window window;
        private PatientManager manager;

        public PatientListViewModel(Window window)
        {
            this.window = window;
            this.manager = new PatientManager();

            this.manager.OnPatientsReceived += (s, d) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    //Update the PatientList to a new ObservableCollection of SharedPatient
                    this.PatientList = new ObservableCollection<SharedPatient>(d);
                });
            };

            this.manager.OnSessionReceived += (s, d) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    //Update the Session to a new ObservableCollection of SessionWrap
                    this.SessionList = new ObservableCollection<SessionWrap>(d);
                });
            };
        }

        #region ListViews data

        private ObservableCollection<SharedPatient> _PatientsList;
        public ObservableCollection<SharedPatient> PatientList
        {
            get { return _PatientsList; }
            set 
            { 
                _PatientsList = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PatientList"));
            }
        }

        private SharedPatient _SelectedPatient;
        public SharedPatient SelectedPatient
        {
            get { return _SelectedPatient; }
            set
            {
                _SelectedPatient = value;
                GetSessionsWithPatient();
            }
        }

        /// <summary>
        /// Method which aks to get all the sessions from a certain patient
        /// </summary>
        private void GetSessionsWithPatient()
        {
            this.manager.GetSessions(SelectedPatient.ID);
        }

        private ObservableCollection<SessionWrap> _SessionList;
        public ObservableCollection<SessionWrap> SessionList
        {
            get { return _SessionList; }
            set
            {
                _SessionList = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SessionList"));
            }
        }

        private SessionWrap _SelectedSession;
        public SessionWrap SelectedSession
        {
            get { return _SelectedSession; }
            set
            {
                _SelectedSession = value;
                OpenHistoryWindow();
            }
        }

        #endregion

        #region SwitchingViews

        /// <summary>
        /// Command which is called on when the Dashboard button has been clicked
        /// </summary>
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

        /// <summary>
        /// Method which switches the content of the current window back to a DasboardViewModel
        /// </summary>
        private void SwitchView()
        {
            this.window.Content = new DashboardViewModel(window);
        }

        
        /// <summary>
        /// Method which sets the content of the current window to a PatientHistoryViewModel and deletes this current manager from the managers list
        /// </summary>
        private void OpenHistoryWindow()
        {
            this.window.Content = new PatientHistoryViewModel(this.window, SelectedPatient, SelectedSession);
            this.manager.DeleteManager(this.manager);
        }

        #endregion
    }
}

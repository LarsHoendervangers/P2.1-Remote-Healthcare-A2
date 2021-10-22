using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using LiveCharts.Geared;
using RemoteHealthcare_Client;
using RemoteHealthcare_Dokter.BackEnd;
using RemoteHealthcare_Shared.DataStructs;

namespace RemoteHealthcare_Dokter.ViewModels
{
    class SessionDetailViewModel : INotifyPropertyChanged
    {
        private Window window;
        private SharedPatient Patient;
        private SessionManager manager;

        public event PropertyChangedEventHandler PropertyChanged;

        public SessionDetailViewModel(Window window, SharedPatient patient)
        {
            this.window = window;
            this.Patient = patient;

            this.manager = new SessionManager(patient);

            this.FullName = this.Patient.FirstName + " " + this.Patient.LastName;
            this.Age = "Leeftijd:\t\t" + CalculateAge();
            this.ID = "ID persoon:\t" + this.Patient.ID;

            this.Speed = "Snelheid: ";
            this.TotalW = "Totaal: ";
            this.CurrentW = "Huidig: ";
            this.Distance = "Afstand: ";
            this.RPM = "RPM: ";
            this.BPM = "BPM: ";

            this.manager.NewDataTriggered += (s, d) =>
            {
                Application.Current?.Dispatcher.Invoke(() =>
                {
                    int BikeIndex = this.manager.BikeMeasurements.Count - 1;
                    int HeartIndex = this.manager.HRMeasurements.Count - 1;

                    if (BikeIndex >= 0)
                    {
                        this.Speed = "Snelheid: " + this.manager.BikeMeasurements[BikeIndex].CurrentSpeed;
                        this.TotalW = "Totaal: " + this.manager.BikeMeasurements[BikeIndex].CurrentTotalWattage;
                        this.CurrentW = "Huidig: " + this.manager.BikeMeasurements[BikeIndex].CurrentWattage;
                        this.Distance = "Afstand: " + this.manager.BikeMeasurements[BikeIndex].CurrentTotalDistance;
                        this.RPM = "RPM: " + this.manager.BikeMeasurements[BikeIndex].CurrentRPM;

                    }

                    if (HeartIndex >= 0)
                    {
                        this.BPM = "BPM: " + this.manager.HRMeasurements[HeartIndex].CurrentHeartrate;
                    }

                    
                });
            };
            this.manager = new SessionManager(patient);


            Application.Current.Dispatcher.Invoke(() =>
            {
                this.MessageList = new ObservableCollection<string>();
            });
        }

        private SharedPatient _SelectedSessionPatient;
        public SharedPatient SelectedSessionPatient
        {
            get { return _SelectedSessionPatient; }
            set
            {
                _SelectedSessionPatient = value;
                HandleSessionPatientClicked();
            }
        }

        private void HandleSessionPatientClicked()
        {
            this.window.Content = new SessionDetailViewModel(this.window, SelectedSessionPatient);
        }

        private string _FullName;
        public string FullName
        {
            get { return _FullName; }
            set
            {
                _FullName = value;
            }
        }

        private string _Age;
        public string Age
        {
            get { return _Age; }
            set
            {

                _Age = value;
            }
        }

        private int CalculateAge()
        {
            int years = DateTime.Now.Year - this.Patient.DateOfBirth.Year;
            if (DateTime.Now.Month > this.Patient.DateOfBirth.Month)
            {
                return years;
            } else if (DateTime.Now.Month < this.Patient.DateOfBirth.Month)
            {
                return years - 1;
            } else
            {
                if (DateTime.Now.Day >= this.Patient.DateOfBirth.Day)
                {
                    return years;
                }

                return years - 1;
            }
        }

        private string _ID;
        public string ID
        {
            get { return _ID; }
            set
            {
                _ID = value;
            }
        }

        private string _RPM;
        public string RPM
        {
            get { return _RPM; }
            set
            {
                _RPM = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RPM"));
            }
        }

        private string _BPM;
        public string BPM
        {
            get { return _BPM; }
            set
            {
                _BPM = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BPM"));
            }
        }

        private string _Speed;
        public string Speed
        {
            get { return _Speed; }
            set
            {
                _Speed = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Speed)));
            }
        }

        private string _Distance;
        public string Distance
        {
            get { return _Distance; }
            set
            {
                _Distance = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Distance"));
            }
        }

        private string _CurrentW;
        public string CurrentW
        {
            get { return _CurrentW; }
            set
            {
                _CurrentW = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentW"));
            }
        }

        private string _TotalW;
        public string TotalW
        {
            get { return _TotalW; }
            set
            {
                _TotalW = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TotalW"));
            }
        }

        private ICommand _AbortCommand;
        public ICommand AbortCommand
        {
            get
            {
                if (_AbortCommand == null)
                {
                    _AbortCommand = new GeneralCommand(
                        param => SendAbort()
                        ); ;
                }
                return _AbortCommand;
            }

        }

        private void SendAbort()
        {
            this.manager.AbortSession();
            this.window.Content = new DashboardViewModel(this.window);
        }

        private ICommand _SendMessage;
        public ICommand SendMessage
        {
            get
            {
                if (_SendMessage == null)
                {
                    _SendMessage = new GeneralCommand(
                        param => SendPersonalMessage()
                        ); ;
                }
                return _SendMessage;
            }

        }

        private string _Message;
        public string Message
        {
            get { return _Message; }
            set
            {
                _Message = value;
            }
        }

        private void SendPersonalMessage()
        {
            this.manager.PersonalMessage(Message);
            UpdateListView();
        }

        private ObservableCollection<string> _MessageList;
        public ObservableCollection<string> MessageList
        {
            get { return _MessageList; }
            set
            {
                _MessageList = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MessageList"));
            }
        }

        private void UpdateListView()
        {
            ObservableCollection<string> TempList = MessageList;

            TempList.Add(Message);

            MessageList = new ObservableCollection<string>(TempList);
        }

        private int _ResistanceValue;
        public int ResistanceValue
        {
            get { return _ResistanceValue; }
            set
            {
                _ResistanceValue = value;
                UpdateResistance();
            }
        }
        

        private void UpdateResistance()
        {
            this.manager.SetResistance(ResistanceValue);
        }


        private ICommand _CloseDetailCommand;
        public ICommand CloseDetailCommand
        {
            get
            {
                if (_CloseDetailCommand == null)
                {
                    _CloseDetailCommand = new GeneralCommand(
                        param => CloseDetail()
                        ); ; ;
                }
                return _CloseDetailCommand;
            }

        }

        private void CloseDetail()
        {
            // Telling the manager this window is closing
            this.manager.CloseManager();

            // Switching from active window
            this.window.Content = new DashboardViewModel(this.window);
        }
    }
}

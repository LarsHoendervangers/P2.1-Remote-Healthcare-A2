using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;
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
            this.MessageList = new ObservableCollection<string>();

            this.FullName = this.Patient.FirstName + " " + this.Patient.LastName;
            this.Age = "Leeftijd:\t\t" + CalculateAge();
            this.ID = "ID persoon:\t" + this.Patient.ID;

            this.Speed = "Snelheid: -- km/h";
            this.TotalW = "Totaal: -- kW";
            this.CurrentW = "Huidig: -- Watt";
            this.Distance = "Afstand: -- m";
            this.RPM = "RPM: --";
            this.BPM = "BPM: --";

            this.manager.NewDataTriggered += SetNewPatientData;

            setupGraph();
        }

        #region Binded Attributes

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

        #endregion

        #region Graphs

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        private void setupGraph()
        {
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double> { 4, 6, 5, 2 ,4 }
                },
                new LineSeries
                {
                    Title = "Series 2",
                    Values = new ChartValues<double> { 6, 7, 3, 4 ,6 },
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Series 3",
                    Values = new ChartValues<double> { 4,2,7,2,7 },
                    PointGeometry = DefaultGeometries.Square,
                    PointGeometrySize = 15
                }
            };

            Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            YFormatter = value => value.ToString("C");

            //modifying the series collection will animate and update the chart
            SeriesCollection.Add(new LineSeries
            {
                Title = "Series 4",
                Values = new ChartValues<double> { 5, 3, 2, 4 },
                LineSmoothness = 0, //0: straight lines, 1: really smooth lines
                PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
                PointGeometrySize = 50,
                PointForeground = Brushes.Gray
            });

            //modifying any series values will also animate and update the chart
            SeriesCollection[3].Values.Add(5d);

        }

        #endregion

        private void SetNewPatientData(object sender, object data)
        {
            Application.Current?.Dispatcher.Invoke(() =>
            {
                int BikeIndex = this.manager.BikeMeasurements.Count - 1;
                int HeartIndex = this.manager.HRMeasurements.Count - 1;

                if (BikeIndex >= 0)
                {
                    this.Speed = $"Snelheid: {this.manager.BikeMeasurements[BikeIndex].CurrentSpeed} km/h";
                    this.TotalW = $"Totaal: {this.manager.BikeMeasurements[BikeIndex].CurrentTotalWattage / 1000f} kW";
                    this.CurrentW = $"Huidig: {this.manager.BikeMeasurements[BikeIndex].CurrentWattage} Watt";
                    this.Distance = $"Afstand: {this.manager.BikeMeasurements[BikeIndex].CurrentTotalDistance} m";
                    this.RPM = "RPM: " + this.manager.BikeMeasurements[BikeIndex].CurrentRPM;

                }

                if (HeartIndex >= 0)
                {
                    this.BPM = "BPM: " + this.manager.HRMeasurements[HeartIndex].CurrentHeartrate;
                }


            });
        }

        private void HandleSessionPatientClicked()
        {
            this.window.Content = new SessionDetailViewModel(this.window, SelectedSessionPatient);
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

        private void SendAbort()
        {
            this.manager.AbortSession();
            this.window.Content = new DashboardViewModel(this.window);
        }

        private void SendPersonalMessage()
        {
            this.manager.PersonalMessage(Message);
            UpdateListView();
        }

        private void UpdateListView()
        {
            ObservableCollection<string> TempList = MessageList;

            TempList.Add(Message);

            MessageList = new ObservableCollection<string>(TempList);
        }
        
        private void UpdateResistance()
        {
            this.manager.SetResistance(ResistanceValue);
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

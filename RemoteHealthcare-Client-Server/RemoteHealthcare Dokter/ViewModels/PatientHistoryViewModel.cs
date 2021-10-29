using LiveCharts;
using LiveCharts.Wpf;
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
using System.Windows.Media;

namespace RemoteHealthcare_Dokter.ViewModels
{
    class PatientHistoryViewModel : INotifyPropertyChanged
    {
        private Window window;
        private SharedPatient Patient;
        private SessionWrap SessionWrap;
        private PatientHisoryManager manager;

        public event PropertyChangedEventHandler PropertyChanged;

        public PatientHistoryViewModel(Window window, SharedPatient selectedPatient, SessionWrap selectedSession)
        {
            this.window = window;
            this.Patient = selectedPatient;
            this.SessionWrap = selectedSession;
            this.manager = new PatientHisoryManager(selectedSession, selectedPatient.ID);

            this.BPMOpacity = 100;
            this.RPMOpacity = 100;
            this.SpeedOpacity = 100;
            this.WattageOpacity = 100;

            this.manager.OnSessionUpdate += (s, d) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    // object types are of object, because the chars library only supports List<object>
                    List<object> HRMeasurements = new List<object>();
                    List<object> RPMMeasurements = new List<object>();
                    List<object> CurrentWMeasurements = new List<object>();
                    List<object> SpeedMeasurements = new List<object>();

                    foreach (HRMeasurement m in d.HRMeasurements)
                    {
                        HRMeasurements.Add(m.CurrentHeartrate);
                    }

                    foreach (BikeMeasurement m in d.BikeMeasurements)
                    {
                        RPMMeasurements.Add(m.CurrentRPM);
                        CurrentWMeasurements.Add(m.CurrentWattage);
                        SpeedMeasurements.Add(m.CurrentSpeed);
                    }

                    this.WattageTotal = this.SessionWrap.BikeMeasurements[this.SessionWrap.BikeMeasurements.Count - 1].CurrentTotalWattage / 1000f + " kW";
                    this.DistanceTotal = this.SessionWrap.BikeMeasurements[this.SessionWrap.BikeMeasurements.Count - 1].CurrentTotalDistance / 1000f + " km";
                    this.Duration = $"{this.SessionWrap.Enddate.Subtract(this.SessionWrap.Startdate).ToString("mm")}:{this.SessionWrap.Enddate.Subtract(this.SessionWrap.Startdate).ToString("ss")} minuten";

                    this.BPMCollection[0].Values.AddRange(HRMeasurements.AsEnumerable<object>());
                    this.BPMOpacity = 0;

                    this.RPMCollection[0].Values.AddRange(RPMMeasurements.AsEnumerable<object>());
                    this.RPMOpacity = 0;

                    this.SpeedCollection[0].Values.AddRange(SpeedMeasurements.AsEnumerable<object>());
                    this.SpeedOpacity = 0;

                    this.CurrentWCollection[0].Values.AddRange(CurrentWMeasurements.AsEnumerable<object>());
                    this.WattageOpacity = 0;
                });
            };

            SetupGraphs();

            this.FullName = this.Patient.FirstName + " " + this.Patient.LastName;
            this.Age = "Leeftijd:\t\t" + CalculateAge();
            this.ID = "ID persoon:\t" + this.Patient.ID;
            this.Birthday = "Geboortedatum:\t" + this.Patient.DateOfBirth.ToString("dd MMMM yyyy");
        }

        #region SessionDetails

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
            }
            else if (DateTime.Now.Month < this.Patient.DateOfBirth.Month)
            {
                return years - 1;
            }
            else
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

        private string _Birthday;
        public string Birthday
        {
            get { return _Birthday; }
            set
            {
                _Birthday = value;
            }
        }

        public string StartDate
        {
            get { return this.SessionWrap.Startdate.ToString("dddd dd MMMM yyyy") + " om " + this.SessionWrap.Startdate.ToString("HH:mm"); }
        }

        public string EndDate
        {
            get { return this.SessionWrap.Enddate.ToString("dddd dd MMMM yyyy") + " om " + this.SessionWrap.Enddate.ToString("HH:mm"); }
        }

        private string _Duration;
        public string Duration
        {
            get { return _Duration; }
            set
            {
                _Duration = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Duration"));
            }
        }

        private string _WattageTotal;
        public string WattageTotal
        {
            get { return _WattageTotal; }
            set
            {
                _WattageTotal = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("WattageTotal"));
            }
        }

        private string _DistanceTotal;
        public string DistanceTotal
        {
            get { return _DistanceTotal; }
            set
            {
                _DistanceTotal = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DistanceTotal"));
            }
        }

        #endregion

        #region SwitchingViews

        private ICommand _CloseHistoryCommand;
        public ICommand CloseHistoryCommand
        {
            get
            {
                if (_CloseHistoryCommand == null)
                {
                    _CloseHistoryCommand = new GeneralCommand(
                        param => CloseHistory()
                        ); ; ;
                }
                return _CloseHistoryCommand;
            }

        }

        private void CloseHistory()
        {
            this.window.Content = new PatientListViewModel(this.window);
            this.manager.DeleteManager(this.manager);
        }

        #endregion

        #region GraphsRegion

        public SeriesCollection BPMCollection { get; set; }
        public SeriesCollection SpeedCollection { get; set; }
        public SeriesCollection RPMCollection { get; set; }
        public SeriesCollection CurrentWCollection { get; set; }

        public List<string> BPMLabels { get; set; }
        public List<string> SpeedLabels { get; set; }
        public List<string> RPMLabels { get; set; }
        public List<string> CurrentWLabels { get; set; }

        public Func<int, string> YFormatter { get; set; }


        private void SetupGraphs()
        {
            // Setting up the BPM Graph
            BPMCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "BPM",
                    Values = new ChartValues<int>() { },
                    PointGeometry = null,
                    LineSmoothness = 10,
                    Fill = new SolidColorBrush(Color.FromScRgb(0.5f, 1f, 0f, 0f)),
                    Stroke = Brushes.Red
                }
            };
            BPMLabels = new List<string>();


            // Setting up the Speed / RPM graph
            SpeedCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Speed",
                    Values = new ChartValues<double>() { },
                    PointGeometry = null,
                    LineSmoothness = 10,
                    Fill = new SolidColorBrush(Color.FromScRgb(0.5f, 0f, 0f, 1f)),
                    Stroke = Brushes.Blue
                }
            };
            SpeedLabels = new List<string>();

            RPMCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "RPM",
                    Values = new ChartValues<int>() { },
                    PointGeometry = null,
                    LineSmoothness = 10,
                    Fill = new SolidColorBrush(Color.FromScRgb(0.5f, 0f, 0.5f, 0f)),
                    Stroke = Brushes.Green
                }
            };
            RPMLabels = new List<string>();

            CurrentWCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "CurrentW",
                    Values = new ChartValues<double>() { },
                    PointGeometry = null,
                    LineSmoothness = 10,
                    Fill = new SolidColorBrush(Color.FromScRgb(0.5f, 0.5f, 0.5f, 0f)),
                    Stroke = Brushes.Yellow
                }
            };
            CurrentWLabels = new List<string>();

            YFormatter = value => value.ToString();
        }

        #endregion

        #region OpacityForLoadingLabels

        private int _WattageOpacity;
        public int WattageOpacity
        {
            get { return _WattageOpacity; }
            set 
            { 
                _WattageOpacity = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("WattageOpacity"));
            }
        }

        private int _SpeedOpacity;
        public int SpeedOpacity
        {
            get { return _SpeedOpacity; }
            set
            {
                _SpeedOpacity = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SpeedOpacity"));
            }
        }

        private int _BPMOpacity;
        public int BPMOpacity
        {
            get { return _BPMOpacity; }
            set
            {
                _BPMOpacity = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BPMOpacity"));
            }
        }

        private int _RPMOpacity;
        public int RPMOpacity
        {
            get { return _RPMOpacity; }
            set
            {
                _RPMOpacity = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RPMOpacity"));
            }
        }

        #endregion
    }
}

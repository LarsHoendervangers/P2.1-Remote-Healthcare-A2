using LiveCharts;
using LiveCharts.Wpf;
using RemoteHealthcare_Client;
using RemoteHealthcare_Dokter.BackEnd;
using RemoteHealthcare_Server;
using RemoteHealthcare_Shared.DataStructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace RemoteHealthcare_Dokter.ViewModels
{
    class PatientHistoryViewModel
    {
        private Window window;
        private SharedPatient Patient;
        private SessionWrap SessionWrap;
        private PatientHisoryManager manager;

        public PatientHistoryViewModel(Window window, SharedPatient selectedPatient, SessionWrap selectedSession)
        {
            this.window = window;
            this.Patient = selectedPatient;
            this.SessionWrap = selectedSession;

            Application.Current.Dispatcher.Invoke(() =>
            {
                this.HRValues = new List<int>();
                this.RPMValues = new List<int>();
                this.SpeedValues = new List<double>();
                this.CurrentWValues = new List<double>();

                foreach (HRMeasurement m in this.SessionWrap.HRMeasurements)
                {
                    this.HRValues.Add(m.CurrentHeartrate);
                }

                foreach (BikeMeasurement m in this.SessionWrap.BikeMeasurements)
                {
                    this.RPMValues.Add(m.CurrentRPM);
                    this.CurrentWValues.Add(m.CurrentWattage);
                    this.SpeedValues.Add(m.CurrentSpeed);
                }
            });

            this.FullName = this.Patient.FirstName + " " + this.Patient.LastName;
            this.Age = "Leeftijd:\t\t" + CalculateAge();
            this.ID = "ID persoon:\t" + this.Patient.ID;
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
        }

        private List<int> _HRValues;
        public List<int> HRValues
        {
            get { return _HRValues; }
            set
            {
                _HRValues = value;
            }
        }

        private List<int> _RPMValues;
        public List<int> RPMValues
        {
            get { return _RPMValues; }
            set
            {
                _RPMValues = value;
            }
        }

        private List<double> _SpeedValues;
        public List<double> SpeedValues
        {
            get { return _SpeedValues; }
            set
            {
                _SpeedValues = value;
            }
        }

        private List<double> _CurrentWValues;
        public List<double> CurrentWValues
        {
            get { return _CurrentWValues; }
            set
            {
                _CurrentWValues = value;
            }
        }

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
                    Values = (IChartValues)HRValues,
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
                    Values = (IChartValues)SpeedValues,
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
                    Values = (IChartValues)RPMValues,
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
                    Values = (IChartValues)CurrentWValues,
                    PointGeometry = null,
                    LineSmoothness = 10,
                    Fill = new SolidColorBrush(Color.FromScRgb(0.5f, 0.5f, 0.5f, 0f)),
                    Stroke = Brushes.Yellow
                }
            };
            CurrentWLabels = new List<string>();

            YFormatter = value => value.ToString();
        }
    }
}

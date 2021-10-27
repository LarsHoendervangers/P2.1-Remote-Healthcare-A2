using LiveCharts;
using LiveCharts.Wpf;
using RemoteHealthcare_Client;
using RemoteHealthcare_Dokter.BackEnd;
using RemoteHealthcare_Server;
using RemoteHealthcare_Shared.DataStructs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            this.manager = new PatientHisoryManager(selectedSession, selectedPatient.ID);

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

                    this.BPMCollection[0].Values.AddRange(HRMeasurements.AsEnumerable<object>());
                    this.RPMCollection[0].Values.AddRange(RPMMeasurements.AsEnumerable<object>());
                    this.SpeedCollection[0].Values.AddRange(SpeedMeasurements.AsEnumerable<object>());
                    this.CurrentWCollection[0].Values.AddRange(CurrentWMeasurements.AsEnumerable<object>());
                });
            };

            SetupGraphs();

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
            this.manager.DeleteManager(this.manager);
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
    }
}

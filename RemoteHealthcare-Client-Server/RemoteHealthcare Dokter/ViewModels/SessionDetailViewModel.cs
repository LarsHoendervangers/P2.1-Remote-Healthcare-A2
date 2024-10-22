﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using RemoteHealthcare_Client;
using RemoteHealthcare_Dokter.BackEnd;
using RemoteHealthcare_Shared.DataStructs;

namespace RemoteHealthcare_Dokter.ViewModels
{
    class SessionDetailViewModel : INotifyPropertyChanged
    {
        private readonly int MAX_GRAPH_LENGHT = 50;

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

            SetupGraphs();
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

        //Method which returns the age of the person
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

        private string _Message;
        public string Message
        {
            get { return _Message; }
            set
            {
                _Message = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Message"));
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

        private double _SpeedxMax = 150;
        public double SpeedxMax
        {
            get { return _SpeedxMax; }
            set
            {
                _SpeedxMax = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SpeedxMax"));
            }
        }

        private double _SpeedxMin = 0;
        public double SpeedxMin
        {
            get { return _SpeedxMin; }
            set
            {
                _SpeedxMin = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SpeedxMin"));
            }
        }

        private double _BPMxMax = 150;
        public double BPMxMax
        {
            get { return _BPMxMax; }
            set
            {
                _BPMxMax = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BPMxMax"));
            }
        }

        private double _BPMxMin = 0;
        public double BPMxMin
        {
            get { return _BPMxMin; }
            set
            {
                _BPMxMin = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BPMxMin"));
            }
        }

        private double _RPMxMax = 150;
        public double RPMxMax
        {
            get { return _RPMxMax; }
            set
            {
                _RPMxMax = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RPMxMax"));
            }
        }

        private double _RPMxMin = 0;
        public double RPMxMin
        {
            get { return _RPMxMin; }
            set
            {
                _RPMxMin = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RPMxMin"));
            }
        }

        private double _DistancexMax = 150;
        public double DistancexMax
        {
            get { return _DistancexMax; }
            set
            {
                _DistancexMax = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DistancexMax"));
            }
        }

        private double _DistancexMin = 0;
        public double DistancexMin
        {
            get { return _DistancexMin; }
            set
            {
                _DistancexMin = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DistancexMin"));
            }
        }

        private double _CurrentWxMax = 150;
        public double CurrentWxMax
        {
            get { return _CurrentWxMax; }
            set
            {
                _CurrentWxMax = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentWxMax"));
            }
        }

        private double _CurrentWxMin = 0;
        public double CurrentWxMin
        {
            get { return _CurrentWxMin; }
            set
            {
                _CurrentWxMin = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentWxMin"));
            }
        }

        #endregion

        #region Graphs

        public SeriesCollection BPMCollection { get; set; }
        public SeriesCollection SpeedCollection { get; set; }
        public SeriesCollection RPMCollection { get; set; }
        public SeriesCollection CurrentWCollection { get; set; }

        private List<int> BPMList { get; set; }
        private List<double> SpeedList { get; set; }
        private List<int> RPMList { get; set; }
        private List<double> CurrentWList { get; set; }

        public List<string> BPMLabels { get; set; }
        public List<string> SpeedLabels { get; set; }
        public List<string> RPMLabels { get; set; }
        public List<string> CurrentWLabels { get; set; }

        public Func<int, string> YFormatter { get; set; }

        
        private void SetupGraphs()
        {
            this.BPMList = new List<int>();
            this.SpeedList = new List<double>();
            this.RPMList = new List<int>();
            this.CurrentWList = new List<double>();

            // Setting up the BPM Graph
            BPMCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "BPM",
                    Values = new ChartValues<int> {},
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
                    Values = new ChartValues<double> {},
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
                    Values = new ChartValues<int> {},
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
                    Values = new ChartValues<double> {},
                    PointGeometry = null,
                    LineSmoothness = 10,
                    Fill = new SolidColorBrush(Color.FromScRgb(0.5f, 0.5f, 0.5f, 0f)),
                    Stroke = Brushes.Yellow
                }
            };
            CurrentWLabels = new List<string>();

            YFormatter = value => value.ToString();
        }

        private void UpdateSpeedGraph(double value, DateTime time)
        {
            var list = SpeedCollection[0].Values;

            // checking if the list is grader than the given max
            // if so removing oldest item
            if (this.SpeedList.Count >= MAX_GRAPH_LENGHT)
                this.SpeedList.RemoveAt(0);

            // Checking the list size, if to large unused data is deleted
            if (list.Count >= MAX_GRAPH_LENGHT * 3)
            {
                list.Clear();
                list = new ChartValues<double>(this.SpeedList);
            }

            this.SpeedxMax = list.Count;

            // Checking if the offet of the list is greater that 0
            int minOffset = list.Count - MAX_GRAPH_LENGHT;
            this.SpeedxMin = minOffset < 0 ? 0 : minOffset;

            list.Add(value);
            this.SpeedList.Add(value);
            SpeedLabels.Add(time.ToString("HH:mm:ss"));
        }

        private void UpdateBPMGraph(int value, DateTime time)
        {
            var list = BPMCollection[0].Values;

           // checking if the list is grader than the given max
            // if so removing oldest item
            if (this.BPMList.Count >= MAX_GRAPH_LENGHT)
                this.BPMList.RemoveAt(0);

            // Checking the list size, if to large unused data is deleted
            if (list.Count >= MAX_GRAPH_LENGHT * 3)
            {
                list.Clear();
                list = new ChartValues<int>(this.BPMList);
            }

            this.BPMxMax = list.Count;

            // Checking if the offet of the list is greater that 0
            int minOffset = list.Count - MAX_GRAPH_LENGHT;
            this.BPMxMin = minOffset < 0 ? 0 : minOffset;

            list.Add(value);
            this.BPMList.Add(value);
            BPMLabels.Add(time.ToString("HH:mm:ss"));
        }

        private void UpdateRPMGraph(int value, DateTime time)
        {
            var list = RPMCollection[0].Values;

            // checking if the list is grader than the given max
            // if so removing oldest item
            if (this.RPMList.Count >= MAX_GRAPH_LENGHT)
                this.RPMList.RemoveAt(0);

            // Checking the list size, if to large unused data is deleted
            if (list.Count >= MAX_GRAPH_LENGHT * 3)
            {
                list.Clear();
                list = new ChartValues<int>(this.RPMList);
            }
                

            this.RPMxMax = list.Count;

            // Checking if the offet of the list is greater that 0
            int minOffset = list.Count - MAX_GRAPH_LENGHT;
            this.RPMxMin = minOffset < 0 ? 0 : minOffset;

            list.Add(value);
            this.RPMList.Add(value);
            RPMLabels.Add(time.ToString("HH:mm:ss"));
        }

        private void UpdateCurrentWGraph(double value, DateTime time)
        {
            var list = CurrentWCollection[0].Values;

            // checking if the list is grader than the given max
            // if so removing oldest item
            if (this.CurrentWList.Count >= MAX_GRAPH_LENGHT)
                this.CurrentWList.RemoveAt(0);

            // Checking the list size, if to large unused data is deleted
            if (list.Count >= MAX_GRAPH_LENGHT * 3)
            {
                list.Clear();
                list = new ChartValues<double>(this.CurrentWList);
            }
            
            
            this.RPMxMax = list.Count;
   

            // Checking if the offet of the list is greater that 0
            int minOffset = list.Count - MAX_GRAPH_LENGHT;
            this.RPMxMin = minOffset < 0 ? 0 : minOffset;

            list.Add(value);
            this.CurrentWList.Add(value);
            RPMLabels.Add(time.ToString("HH:mm:ss"));
        }

        #endregion

        #region MethodsAndCommands
        /// <summary>
        /// Method which Sets all the new data for the graphs and the labels showing the values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        private void SetNewPatientData(object sender, object data)
        {
            Application.Current?.Dispatcher.Invoke(() =>
            {

                int BikeIndex = this.manager.BikeMeasurements.Count - 1;
                int HeartIndex = this.manager.HRMeasurements.Count - 1;

                if (BikeIndex >= 0)
                {
                    // Getting the data
                    double speed = this.manager.BikeMeasurements[BikeIndex].CurrentSpeed;
                    int rpm = this.manager.BikeMeasurements[BikeIndex].CurrentRPM;
                    double distance = Math.Round(this.manager.BikeMeasurements[BikeIndex].CurrentTotalDistance / 1000f, 2);
                    double CurrentWattage = this.manager.BikeMeasurements[BikeIndex].CurrentWattage;
                    double TotalWattage = Math.Round(this.manager.BikeMeasurements[BikeIndex].CurrentTotalWattage / 1000f, 2);

                    // setting the labels
                    this.Speed = $"{speed} km/h";
                    this.TotalW = $"{TotalWattage} kW";
                    this.CurrentW = $"{CurrentWattage} W";
                    this.Distance = $"{distance} km";
                    this.RPM = $"{rpm}";


                    // updating the graphs
                    UpdateSpeedGraph(speed, this.manager.BikeMeasurements[BikeIndex].MeasurementTime);
                    UpdateRPMGraph(rpm, this.manager.BikeMeasurements[BikeIndex].MeasurementTime);
                    UpdateCurrentWGraph(CurrentWattage, this.manager.BikeMeasurements[BikeIndex].MeasurementTime);
                }

                if (HeartIndex >= 0)
                {
                    // Getting the data
                    int BPM = this.manager.HRMeasurements[HeartIndex].CurrentHeartrate;

                    // Setting the labels
                    this.BPM = "" + BPM;

                    // Updating the graphs
                    UpdateBPMGraph(BPM, this.manager.HRMeasurements[HeartIndex].MeasurementTime);
                }


            });
        }

        /// <summary>
        /// Method which sets the content of the current window to a SessionDetailViewModel
        /// </summary>
        private void HandleSessionPatientClicked()
        {
            this.window.Content = new SessionDetailViewModel(this.window, SelectedSessionPatient);
        }

        /// <summary>
        /// Method which send an abort to the manager and sets the content of the current window to a DasboardViewModel
        /// </summary>
        private void SendAbort()
        {
            this.manager.AbortSession();
            this.window.Content = new DashboardViewModel(this.window);
        }

        /// <summary>
        /// Method which sends a personal message to the manager, updates the listview of the messages and clears the textbox
        /// </summary>
        private void SendPersonalMessage()
        {
            this.manager.PersonalMessage(Message);
            UpdateListView();
            this.Message = "";
        }

        /// <summary>
        /// Method which creates a temproary list, adds a new value to this list and this list is assigned to the MessagesList, updating the ListView
        /// </summary>
        private void UpdateListView()
        {
            ObservableCollection<string> TempList = MessageList;

            TempList.Add(Message);

            MessageList = new ObservableCollection<string>(TempList);
        }
        
        /// <summary>
        /// Method sends the resistance to the manager
        /// </summary>
        private void UpdateResistance()
        {
            this.manager.SetResistance(ResistanceValue);
        }

        /// <summary>
        /// Method which removes the current manager from the managers list, sets the content of the
        /// current window to a DashboardViewModel and unsubscribes to a certain patient
        /// </summary>
        private void CloseDetail()
        {
            // Telling the manager this window is closing
            this.manager.DeleteManager(this.manager);

            // Switching from active window
            this.window.Content = new DashboardViewModel(this.window);

            this.manager.SubscribeToPatient(this.Patient, false);
        }

        /// <summary>
        /// Command which is called when the StopSession button has been clicked
        /// </summary>
        private ICommand _StopSessionCommand;
        public ICommand StopSessionCommand
        {
            get
            {
                if (_StopSessionCommand == null)
                {
                    _StopSessionCommand = new GeneralCommand(
                        param => SendStopSession()
                        );
                }
                return _StopSessionCommand;
            }

        }

        //method which stops a session and sets the content of the current window to a new DashboardViewModel
        private void SendStopSession()
        {
            this.manager.StopSession(this.Patient);
            this.window.Content = new DashboardViewModel(this.window);
        }

        /// <summary>
        /// Command which is called on when the abort button has been clicked
        /// </summary>
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

        /// <summary>
        /// Command which is called ono when the Send message button has been clicked
        /// </summary>
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

        /// <summary>
        /// COmmand which is called on when the x in the top right corner of the screen has been clicked
        /// </summary>
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

        #endregion
    }
}

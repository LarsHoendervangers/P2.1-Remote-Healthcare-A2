using Newtonsoft.Json;
using RemoteHealthcare_Server.Data.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace RemoteHealthcare_Server
{
    public class Session
    {
        [JsonIgnore]
        public List<Doctor> Subscribers { get; set; }

        public Session(Patient patient)
        {
            Patient = patient;
            Subscribers = new List<Doctor>();
            this.StartTime = DateTime.Now;
            this.HRMeasurements = new List<HRMeasurement>();
            this.BikeMeasurements = new List<BikeMeasurement>();
        }

        /// <summary>
        /// List of HRMeasurements in this current session
        /// </summary>
        public List<HRMeasurement> HRMeasurements
        {
            get ;
            set;
        }

        /// <summary>
        /// List of BikeMeasurements in this current session
        /// </summary>
        public List<BikeMeasurement> BikeMeasurements
        {
            get;
            set;
        }

        public int SessionID
        {
            get => default;
            set
            {
            }
        }

        [JsonIgnore]
        public Patient Patient
        {
            get;
            set;
        }

        public DateTime StartTime
        {
            get;
            set;
        }

        public DateTime EndTime
        {
            get; 
            set;
        }

        public void SetEndTime()
        {
            EndTime = DateTime.Now;
        }
    }
}
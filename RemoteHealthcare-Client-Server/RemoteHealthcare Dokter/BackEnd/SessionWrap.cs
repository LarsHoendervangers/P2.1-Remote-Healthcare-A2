﻿using RemoteHealthcare_Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Dokter.BackEnd
{
    public class SessionWrap
    {

        public List<HRMeasurement> HRMeasurements;
        public List<BikeMeasurement> BikeMeasurements;

        public DateTime Startdate { get; set; }
        public DateTime Enddate { get; set; }

        public SessionWrap(List<HRMeasurement> hRMeasurements, List<BikeMeasurement> bikeMeasurements, DateTime startdate, DateTime enddate)
        {
            HRMeasurements = hRMeasurements;
            BikeMeasurements = bikeMeasurements;
            Startdate = startdate;
            Enddate = enddate;
        }

        public override string ToString()
        {
            return $"{this.Startdate.ToString("dddd d MMMM yyyy HH:mm:ss")} - {this.Enddate.Subtract(this.Startdate).Minutes} minuten";
        }
    }
}

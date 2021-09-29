using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteHealthcare_Server
{
    public class HRMeasurement
    {
        public DateTime MeasurementTime
        {
            get => default;
            set
            {
            }
        }

        public int CurrentHeartrate
        {
            get => default;
            set
            {
            }
        }

        public HRMeasurement(DateTime measurementTime, int currentHeartrate)
        {
            MeasurementTime = measurementTime;
            CurrentHeartrate = currentHeartrate;
        }
    }
}
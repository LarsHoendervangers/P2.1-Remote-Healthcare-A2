﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteHealthcare_Server
{
    public class BikeMeasurement
    {
        public DateTime MeasurementTime
        {
            get; set;
        }

        public int CurrentRPM
        {
            get; set;
        }

        public double CurrentSpeed
        {
            get; set;
        }

        public double CurrentWattage
        {
            get; set;
        }

        public int CurrentTotalWattage
        {
            get; set;
        }

        public int CurrentTotalDistance
        {
            get; set;
        }

        public BikeMeasurement(DateTime measurementTime, int currentRPM, double currentSpeed, double currentWattage, int currentTotalWattage, int currentTotalDistance)
        {
            MeasurementTime = measurementTime;
            CurrentRPM = currentRPM;
            CurrentSpeed = currentSpeed;
            CurrentWattage = currentWattage;
            CurrentTotalWattage = currentTotalWattage;
            CurrentTotalDistance = currentTotalDistance;
        }
    }
}
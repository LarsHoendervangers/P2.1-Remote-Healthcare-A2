using RemoteHealthcare_Server.Data.User;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Server.Data.Logic.SubManagers
{
    partial class SessionSubmanager 
    {

        private UserManagement management;

        public SessionSubmanager(UserManagement management)
        {
            this.management = management;
        }


        /// <summary>
        /// Updates the bike data
        /// </summary>
        /// <param name="rpm"></param>
        /// <param name="speed"></param>
        /// <param name="dist"></param>
        /// <param name="pow"></param>
        /// <param name="accpow"></param>
        /// <param name="time"></param>
        /// <param name="user"></param>
        public Session SessionUpdateBike(int rpm, int speed, int dist, int pow, int accpow, DateTime time, IUser user)
        {
            lock (this)
            {
                foreach (Session s in UserManagement.activeSessions)
                {
                    if (s.Patient == (Patient)user)
                    {
                        Server.PrintToGUI("Added new measurement");
                        s.BikeMeasurements.Add(new BikeMeasurement(time, rpm, speed, pow, accpow, dist));


                        return s;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Updates heart data
        /// </summary>
        /// <param name="time"></param>
        /// <param name="bpm"></param>
        /// <param name="user"></param>
        public Session SessionUpdateHRM(DateTime time, int bpm, IUser user)
        {
            lock (this)
            {
                foreach (Session s in UserManagement.activeSessions)
                {
                    if (s.Patient == (Patient)user)
                    {
                        Server.PrintToGUI("Added new measurement");
                        s.HRMeasurements.Add(new HRMeasurement(time, bpm));
                        return s;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Subscibe to a patient as doctor
        /// </summary>
        /// <param name="d"></param>
        /// <param name="patientIDS"></param>
        public void Subscribe(Doctor d, List<string> patientIDS)
        {
            lock (this)
            {
                foreach (Session s in UserManagement.activeSessions)
                {
                    foreach (string id in patientIDS)
                    {
                        if (s.Patient.PatientID == id)
                        {
                            s.Subscribers.Add(d);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// UNsub to a patient as docotr
        /// </summary>
        /// <param name="d"></param>
        /// <param name="patientIDS"></param>
        public void Unsubscribe(Doctor d, List<string> patientIDS)
        {
            lock (this)
            {
                foreach (Session s in UserManagement.activeSessions)
                {
                    foreach (string id in patientIDS)
                    {
                        if (s.Patient.PatientID == id)
                        {
                            s.Subscribers.Remove(d);
                        }
                    }
                }
            }
        }



        /// <summary>
        /// Starts a session
        /// </summary>
        /// <param name="user"></param>
        public void SessionStart(IUser user)
        {
            lock (this)
            {
                if (user.getUserType() == UserTypes.Patient)
                {
                    UserManagement.activeSessions.Add(new Session((Patient)user));
                }
            }
        }

        /// <summary>
        /// Stops a session
        /// </summary>
        /// <param name="user"></param>
        public void SessionEnd(IUser user)
        {
            lock (this)
            {
                if (user != null && user.getUserType() == UserTypes.Patient)
                {
                    foreach (Session s in UserManagement.activeSessions)
                    {
                        if (s.Patient == (Patient)user)
                        {
                            UserManagement.activeSessions.Remove(s);
                            return;
                        }
                    }
                }
            }
        }
    }
}

using CommClass;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RemoteHealthcare_Server.Data.User;
using RemoteHealthcare_Shared;
using RemoteHealthcare_Shared.DataStructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Server
{
    class JSONWriter
    {
        /// <summary>
        /// Writes if an action was succesfull or not..
        /// </summary>
        /// <param name="succes">Boolean that indicates the state</param>
        /// <param name="sender">Sender for the selected target</param>
        public static void LoginWrite(bool succes, ISender sender)
        {
            //Response..
            string state = succes ? "succesfull connect" : "failed connect";

            //Selecting object
            object o;
            o = new
            {
                command = "message",
                data = state,
                flag = 1
            };

            //Writing answer...
            sender.SendMessage(JsonConvert.SerializeObject(o));
        }

        /// <summary>
        /// Writes a message to the host that is selected...
        /// </summary>
        /// <param name="msg">Message to client</param>
        /// <param name="sender"></param>
        public static void MessageWrite(string msg, ISender sender)
        {
            //Sending over message to client
            object o = new
            {
                command = "message",
                data = msg,
                flag = 2
            };

            sender.SendMessage(JsonConvert.SerializeObject(o));
        }

        /// <summary>
        /// Writes the resitance to a host that is selected
        /// </summary>
        /// <param name="resistance"></param>
        /// <param name="sender"></param>
        public static void ResistanceWrite(int resistance, ISender sender)
        {
            object o = new
            {
                command = "setresist",
                data = resistance
            };


            sender.SendMessage(JsonConvert.SerializeObject(o));
        }

        /// <summary>
        /// Writes an aborot to a host that is selected
        /// </summary>
        /// <param name="sender"></param>
        public static void AbortWrite(ISender sender)
        {
            object o = new
            {
                command = "abort",
                data = new { }
            };

            sender.SendMessage(JsonConvert.SerializeObject(o));
        }

        /// <summary>
        /// Writes all patietns to a selected host..
        /// </summary>
        /// <param name="AllPatients"></param>
        /// <param name="sender"></param>
        public static void AllPatientWrite(List<string> AllPatients, ISender sender)
        {
            object o = new
            {
                command = "getallpatients",
                data = AllPatients
            };

            sender.SendMessage(JsonConvert.SerializeObject(o));
        }


        /// <summary>
        /// Writes active patients to a slected host..
        /// </summary>
        /// <param name="AllPatients"></param>
        /// <param name="sender"></param>
        public static void ActivePatientWrite(List<string> AllPatients, ISender sender)
        {
            object o = new
            {
                command = "getactivepatients",
                data = AllPatients
            };

            sender.SendMessage(JsonConvert.SerializeObject(o));
        }


        /// <summary>
        /// Writes an 
        /// </summary>
        /// <param name="h"></param>
        /// <param name="s"></param>
        public static void DoctorSubWriter(Host h, Session s, string id, ISender sender, bool type)
        {
            //Getting latest measurment
            object measurement = null;
            if (type && s.BikeMeasurements.Count > 0) measurement = s.BikeMeasurements.Last();
            else if (!type && s.HRMeasurements.Count > 0) measurement =  s.HRMeasurements.Last();

            //Parsing it to an object.
            object o = new
            {
                command = "livepatientdata",
                data = new
                {
                    id = id,
                    data = measurement
                }
            };

            //Sending it over.
            sender.SendMessage(JsonConvert.SerializeObject(o));
        }

        /// <summary>
        /// Sends the detailed patiens over...
        /// </summary>
        /// <param name="patients"></param>
        /// <param name="sender"></param>
        public static void SendDetails(List<SharedPatient> patients, ISender sender)
        {
            object o = new
            {
                command = "detaildata",
                data = patients
                
            };
            sender.SendMessage(JsonConvert.SerializeObject(o));
        }

        /// <summary>
        /// Writes the history to the doctor from one patient for one session
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="sessoins"></param>
        /// <param name="patientID"></param>
        public static void HistoryWrite(ISender sender, Session session, string patientID)
        {
            object o = new
            {
                command = "getsessionsdetails",
                data = new
                {
                    patientid = patientID,
                    session = new
                    {
                        startdate = session.StartTime,
                        enddate = session.EndTime,
                        hrdata = session.HRMeasurements,
                        bikedata = session.BikeMeasurements
                    }
                }
            };

            sender.SendMessage(JsonConvert.SerializeObject(o));
        }

        /// <summary>
        /// Writes all the dates...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="sessions"></param>
        /// <param name="patientID"></param>
        public static void HistoryDates(ISender sender, List<Session> sessions, string patientID)
        {
            //Getting dates.
            List<DateTime> startdates = new List<DateTime>();
            List<DateTime> enddates = new List<DateTime>();
            foreach (Session s in sessions)
            {
                startdates.Add(s.StartTime);
                enddates.Add(s.EndTime);   
            }

            object o = new
            {
                command = "getsessions",
                data = new
                {
                    patientid = patientID,
                    startdates = startdates,
                    enddates = enddates
                }
            };

            sender.SendMessage(JsonConvert.SerializeObject(o));

        }

        public static void WriteMessage(string message, List<Host> activeHosts)
        {
            //Message
            object o = new
            {
                command = "message",
                flag = "2",
                data = message

            };

            //Sending
            foreach (Host h in activeHosts)
            {
                if (h != null)
                {
                    h.GetSender().SendMessage(JsonConvert.SerializeObject(o));
                }
            }

        }
    }
}

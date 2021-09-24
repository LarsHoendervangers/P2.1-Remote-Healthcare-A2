using CommClass;
using Newtonsoft.Json.Linq;
using RemoteHealthcare_Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace RemoteHealthcare_Server
{


    //NOTE to future self (Luuk) also do this with try parse.............
    public class JSONReader
    {

        //Switch case for inputs
        public void DecodeJsonObject(JObject jObject, EncryptedSender sender, int accessLevel, object o, Usermanagement managemet)
        {
            string command = jObject.GetValue("command").ToString();

            switch (accessLevel)
            {
                case 0:
                    if (command == "ergometer") JSONPatient.ReceiveMeasurement(jObject, sender, (Patient)o);
                    break;
                case 1:
                    if (command == "abort") JSONDoctor.AbortingClient(jObject, sender);
                    if (command == "setresist") JSONDoctor.SettingErgometer(jObject, sender);
                    if (command == "getallclients") JSONDoctor.GetAllClients(jObject, sender);
                    if (command == "subtopatient") JSONDoctor.SubscribeToLiveSession(jObject, sender);
                    if (command == "getsession") JSONDoctor.GetHistoricSession(jObject, sender);
                    if (command == "newsession") JSONDoctor.StartNewSession(jObject, sender, managemet);
                    break;

                case 2:
                    //Can have all admin features you want
                    break;

            }
        }



        /// <summary>
        /// This class contains all the methods with acces level 0 so being it patients
        /// </summary>
        partial class JSONPatient
        {
            public static void ReceiveMeasurement(JObject Jobject, EncryptedSender sender, Patient p)
            {
                if (p.session != null)
                {
                    //Bike
                    JToken rpm = Jobject.SelectToken("data.rpm");
                    JToken speed = Jobject.SelectToken("data.speed");
                    JToken dist = Jobject.SelectToken("data.dist");
                    JToken pow = Jobject.SelectToken("data.pow");
                    JToken accpow = Jobject.SelectToken("data.accpw");

                    //Heart
                    JToken bpm = Jobject.SelectToken("data.bpm");

                    //All
                    JToken time = Jobject.SelectToken("data.time");

                    //Checks
                    if (rpm != null)
                    {
                        p.session.BikeMeasurements.Add(
                        new BikeMeasurement(DateTime.Parse(time.ToString())
                        , int.Parse(rpm.ToString()), int.Parse(speed.ToString())
                        , double.Parse(pow.ToString()), int.Parse(accpow.ToString())
                        , int.Parse(dist.ToString())));
                    }
                    else if (bpm != null)
                    {
                        p.session.HRMeasurements.Add(new HRMeasurement(
                      DateTime.Parse(time.ToString()), int.Parse(bpm.ToString())));
                    }

                    Server.PrintToGUI("Received data");
                }
            }


        }

        /// <summary>
        /// This class contains all the methods with acces level 1 so being it doctors
        /// </summary>
        partial class JSONDoctor
        {
            public static void SettingErgometer(JObject jObject, EncryptedSender sender)
            {
                throw new NotImplementedException();
            }

            public static void AbortingClient(JObject jObject, EncryptedSender sender)
            {
                throw new NotImplementedException();
            }

            internal static void GetAllClients(JObject jObject, EncryptedSender sender)
            {
                throw new NotImplementedException();
            }

            internal static void SubscribeToLiveSession(JObject jObject, EncryptedSender sender)
            {
                throw new NotImplementedException();
            }

            internal static void GetHistoricSession(JObject jObject, EncryptedSender sender)
            {
                throw new NotImplementedException();
            }

            internal static void StartNewSession(JObject jObject, EncryptedSender sender, Usermanagement management)
            {
                JObject data = (JObject)jObject.GetValue("data");
                string patientID = data.GetValue("patientid").ToString();
                bool sessionState = bool.Parse(data.GetValue("state").ToString());

                if (sessionState)
                {
                    management.StartSession(patientID).session = new Session();
                } else
                {
                    FileProcessing.SaveSession(management.StartSession(patientID));
                    management.StartSession(patientID).session = null;
                }

                Server.PrintToGUI("Stared a new session");
            }

          
        }


        /// <summary>
        /// This class contains all the methods with acces level 2 so being it admin
        /// </summary>
        partial class JSONAdmin
        {






        }
    }
  
}
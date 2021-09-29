using CommClass;
using Newtonsoft.Json.Linq;
using RemoteHealthcare_Server.Data;
using RemoteHealthcare_Server.Data.User;
using RemoteHealthcare_Shared;
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
        public void DecodeJsonObject(JObject jObject, ISender sender, IUser user, Usermanagement managemet)
        {
            string command = jObject.GetValue("command").ToString();

            //Availbe receiving functions
            switch (user.getUserType())
            {
                case UserTypes.Patient:
                    if (command == "ergometer") JSONPatient.ReceiveMeasurement(jObject, sender, user, managemet);
                    break;
                case UserTypes.Doctor:
                    if (command == "abort") JSONDoctor.AbortingClient(jObject, sender);
                    if (command == "setresist") JSONDoctor.SettingErgometer(jObject, sender);
                    if (command == "getallclients") JSONDoctor.GetAllClients(jObject, sender);
                    if (command == "subtopatient") JSONDoctor.SubscribeToLiveSession(jObject, sender);
                    if (command == "getsession") JSONDoctor.GetHistoricSession(jObject, sender);
                    if (command == "newsession") JSONDoctor.StartNewSession(jObject, sender, managemet, user);
                    break;
                case UserTypes.Admin:
                    //Can have all admin features you want
                    break;

            }
        }



        /// <summary>
        /// This class contains all the methods with acces level 0 so being it patients
        /// </summary>
        partial class JSONPatient
        {
            public static void ReceiveMeasurement(JObject Jobject, ISender sender, IUser user, Usermanagement usermanagement)
            {
                if (user != null)
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
                        usermanagement.SessionUpdateBike(int.Parse(rpm.ToString()),
                            int.Parse(speed.ToString()), int.Parse(dist.ToString()), int.Parse(pow.ToString()),
                            int.Parse(accpow.ToString()), DateTime.Parse(time.ToString()), user);
                    }
                    else if (bpm != null)
                    {
                        usermanagement.SessionUpdateHRM(DateTime.Parse(time.ToString()), int.Parse(bpm.ToString()), user);
                    }

                    //Server.PrintToGUI("Received data");
                }
            }


        }

        /// <summary>
        /// This class contains all the methods with acces level 1 so being it doctors
        /// </summary>
        partial class JSONDoctor
        {
            public static void SettingErgometer(JObject jObject, ISender sender)
            {
                throw new NotImplementedException();
            }

            public static void AbortingClient(JObject jObject, ISender sender)
            {
                throw new NotImplementedException();
            }

            internal static void GetAllClients(JObject jObject, ISender sender)
            {
                throw new NotImplementedException();
            }

            internal static void SubscribeToLiveSession(JObject jObject, ISender sender)
            {
                throw new NotImplementedException();
            }

            internal static void GetHistoricSession(JObject jObject, ISender sender)
            {
                throw new NotImplementedException();
            }

            internal static void StartNewSession(JObject jObject, ISender sender, Usermanagement management, IUser user)
            {

                //Logic for parsing still needs to be made which user it is and if its on or of...
                management.SessionStart(user);
                management.SessionEnd(user);
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
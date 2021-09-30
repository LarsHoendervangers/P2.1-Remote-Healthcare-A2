using CommClass;
using Newtonsoft.Json.Linq;
using RemoteHealthcare_Server.Data;
using RemoteHealthcare_Server.Data.User;
using RemoteHealthcare_Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace RemoteHealthcare_Server
{

    public class JSONReader
    {
        public void DecodeJsonObject(JObject jObject, ISender sender, IUser user, Usermanagement managemet)
        {
            string command = jObject.GetValue("command").ToString();

            MethodInfo[] methods = typeof(JSONReader).GetMethods(BindingFlags.Static | BindingFlags.NonPublic);
            foreach (MethodInfo method in methods)
            {
               
              
                if (method.GetCustomAttribute<AccesManagerAttribute>().GetCommand() == command
                    && method.GetCustomAttribute<AccesManagerAttribute>().GetUserType() == user.getUserType())
                {
                    Server.PrintToGUI(method.Name);
                    method.Invoke(this, new object[] { jObject, sender, user, managemet });
                }
            }
        }

        [AccesManager("ergodata", UserTypes.Patient)]
        private static void ReceiveMeasurement(JObject Jobject, ISender sender, IUser user, Usermanagement usermanagement)
        {
            Server.PrintToGUI("Got your data");
            if (user != null)
            {
                //Bike
                JToken rpm = Jobject.SelectToken("data.rpm");
                JToken speed = Jobject.SelectToken("data.speed");
                JToken dist = Jobject.SelectToken("data.dist");
                JToken pow = Jobject.SelectToken("data.pow");
                JToken accpow = Jobject.SelectToken("data.accpow");

                //Heart
                JToken bpm = Jobject.SelectToken("data.bpm");

                //All
                JToken time = Jobject.SelectToken("data.time");

                //Checks
                if (rpm != null)
                {
                    usermanagement.SessionUpdateBike(int.Parse(rpm.ToString()),
                        (int)double.Parse(speed.ToString()), (int)double.Parse(dist.ToString()), int.Parse(pow.ToString()),
                        int.Parse(accpow.ToString()), DateTime.Parse(time.ToString()), user);
                }
                else if (bpm != null)
                {
                    usermanagement.SessionUpdateHRM(DateTime.Parse(time.ToString()), int.Parse(bpm.ToString()), user);
                }
            }
        }


        


        [AccesManager("setresist", UserTypes.Doctor)]
        private static void SettingErgometer(JObject jObject, ISender sender, IUser user, Usermanagement managemet)
        {
            throw new NotImplementedException();
        }

        [AccesManager("abort", UserTypes.Doctor)]
        private static void AbortingClient(JObject jObject, ISender sender, IUser user, Usermanagement managemet)
        {
            throw new NotImplementedException();
        }

        [AccesManager("getallclients", UserTypes.Doctor)]
        private static void GetAllClients(JObject jObject, ISender sender, IUser user, Usermanagement managemet)
        {
            throw new NotImplementedException();
        }


        [AccesManager("subtopatient", UserTypes.Doctor)]
        private static void SubscribeToLiveSession(JObject jObject, ISender sender, IUser user, Usermanagement managemet)
        {
            throw new NotImplementedException();
        }

        [AccesManager("getsessions", UserTypes.Doctor)]
        private static void GetHistoricSession(JObject jObject, ISender sender, IUser user, Usermanagement managemet)
        {
            throw new NotImplementedException();
        }
            

        [AccesManager("newsession", UserTypes.Doctor)]
        private static void StartNewSession(JObject jObject, ISender sender, IUser user, Usermanagement management)
        {

            //Logic for parsing still needs to be made which user it is and if its on or of...
            management.SessionStart(user);
            management.SessionEnd(user);
        }



      
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class AccesManagerAttribute : Attribute
    {
        string command;
        UserTypes type;

        public AccesManagerAttribute(string command, UserTypes type)
        {
            this.command = command;
            this.type = type;
        }

        public string GetCommand()
        {
            return command;
        }

        public UserTypes GetUserType()
        {
            return type;
        }
    }
    
  
}
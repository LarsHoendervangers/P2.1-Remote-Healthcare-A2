using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace RemoteHealthcare_Server
{
    public class JSONReader
    {
        //Switch case for inputs
        public static void DecodeJsonObject(JObject jObject, Host host)
        {
            string command = jObject.GetValue("command").ToString();

            switch (command)
            {
                case "login":
                    LoginAction(jObject, host.TcpClient, host.ClientPatient);
                    break;
                case "ergodata":
                    ReceiveMeasurement(jObject, host.ClientPatient);
                    break;
            }
        }


        //TODO making if there is a database
        private static  void LoginAction(JObject Jobject, TcpClient client, Patient patient)
        {
            JObject data = (JObject)Jobject.GetValue("data");
            //There is still no database to check if there are any patients.   
            //if the client is found then it will send a succes else a failed.
            patient = new Patient("Spaggeti", "Pasword", new DateTime(1, 2,3), new Session());

            JSONWriter.LoginWrite(true, client);
        }


        //TODO test
        private static void ReceiveMeasurement(JObject Jobject, Patient patient)
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
                patient.Session.BikeMeasurements.Add(
                    new BikeMeasurement(DateTime.Parse(time.ToString())
                    , int.Parse(rpm.ToString()), int.Parse(speed.ToString())
                    , double.Parse(pow.ToString()), int.Parse(accpow.ToString())
                    , int.Parse(dist.ToString())));
            } else if (bpm != null)
            {
                patient.Session.HRMeasurements.Add(new HRMeasurement(
                    DateTime.Parse(time.ToString()), int.Parse(bpm.ToString())));
            }
        }

       
    }
}
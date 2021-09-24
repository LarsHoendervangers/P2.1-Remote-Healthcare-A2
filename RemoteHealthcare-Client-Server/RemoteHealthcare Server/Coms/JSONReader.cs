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
    public class JSONReader
    {

        //Switch case for inputs
        public static void DecodeJsonObject(JObject jObject, EncryptedSender sender)
        {
            string command = jObject.GetValue("command").ToString();

            switch (command)
            {
               /* case "login":
                    LoginAction(jObject, sender);
                    break;*/
                case "ergodata":
                    ReceiveMeasurement(jObject, sender);
                    break;
            }
        }


        //TODO making if there is a database
  

        //TODO test
        private static void ReceiveMeasurement(JObject Jobject, EncryptedSender sender)
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
               // patient.Session.BikeMeasurements.Add(
          /*          new BikeMeasurement(DateTime.Parse(time.ToString())
                    , int.Parse(rpm.ToString()), int.Parse(speed.ToString())
                    , double.Parse(pow.ToString()), int.Parse(accpow.ToString())
                    , int.Parse(dist.ToString())));*/
            }
            else if (bpm != null)
            {
              //  patient.Session.HRMeasurements.Add(new HRMeasurement(
                 //   DateTime.Parse(time.ToString()), int.Parse(bpm.ToString())));
            }
        }


    }
}
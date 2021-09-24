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
                case "login":
                    LoginAction(jObject, sender);
                    break;
                case "ergodata":
                    ReceiveMeasurement(jObject, sender);
                    break;
            }
        }


        //TODO making if there is a database
        private static void LoginAction(JObject Jobject, EncryptedSender sender)
        {
            //Getting alle the amazing data
            JObject data = (JObject)Jobject.GetValue("data");
            string username = data.GetValue("us").ToString();
            string password = data.GetValue("pass").ToString();
            int flag = int.Parse(data.GetValue("flag").ToString());


            //Issue still to fix to send it up to the specfice class im thinking of making an object of this class....
         /*   switch (flag)
            {
                case 0:
                    host.user = usermanagement.CheckPatientCredentials(username, password);
                    break;
                case 1:
                    host.user = usermanagement.CheckDoctorCredentials(username, password);
                    break;
                case 2:
                    host.user = usermanagement.CheckAdminCredentials(username, password);
                    break;
            }
*/

            JSONWriter.LoginWrite(true, sender);
        }


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
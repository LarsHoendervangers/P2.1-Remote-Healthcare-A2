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

        public static void DecodeJsonObject(JObject jObject, TcpClient client)
        {
            string command = jObject.GetValue("command").ToString();

            switch (command)
            {
                case "login":
                    LoginAction(jObject, client);
                    break;
                case "message":
                    // code block
                    break;

                case "ergodata":
                    // code block
                    break;

         



                default:

                    //code block
                    break;
            }

        }


        //TODO making if there is a database
        private static  void LoginAction(JObject Jobject, TcpClient client)
        {
            JObject data = (JObject)Jobject.GetValue("data");
            //There is still no database to check if there are any patients.   
            //if the client is found then it will send a succes else a failed.
            JSONWrite.LoginWrite(true, client);
        }

        //private static void ReceiveMeasurement(JObject J)

       
    }
}
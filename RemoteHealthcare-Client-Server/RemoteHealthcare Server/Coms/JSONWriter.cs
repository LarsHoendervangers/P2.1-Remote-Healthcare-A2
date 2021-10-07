using CommClass;
using Newtonsoft.Json;
using RemoteHealthcare_Server.Data.User;
using RemoteHealthcare_Shared;
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
        public static void LoginWrite(bool succes, ISender sender)
        {
            //Selecting object
            object o;
            if (succes)
            {
                o = new
                {
                    command = "message",
                    data = "succesfull connect",
                    flag = 1
                };
            } else
            {
                o = new
                {
                    command = "message",
                    data = "failed connect",
                    flag = 1
            };
            }

            //Writing answer...
            sender.SendMessage(JsonConvert.SerializeObject(o));
        }


        public static void MessageWrite(string msg, ISender sender)
        {
            object o = new
            {
                command = "message",
                data = msg,
                flag = 2
            };

            sender.SendMessage(JsonConvert.SerializeObject(o));
        }

        public static void ResistanceWrite(int resistance, ISender sender)
        {
            object o = new
            {
                command = "setresist",
                data = resistance
            };

            
            sender.SendMessage(JsonConvert.SerializeObject(o));
        }

        public static void AbortWrite(ISender sender)
        {
            object o = new
            {
                command = "abort",
                data = new { }
            };

            sender.SendMessage(JsonConvert.SerializeObject(o));
        }

        public static void AllPatientWrite(List<string> AllPatients, ISender sender)
        {
            object o = new
            {
                command = "getallpatients",
                data = AllPatients
            };

            sender.SendMessage(JsonConvert.SerializeObject(o));
        }

        public static void ActivePatientWrite(List<string> AllPatients, ISender sender)
        {
            object o = new
            {
                command = "getactivepatients",
                data = AllPatients
            };

            sender.SendMessage(JsonConvert.SerializeObject(o));
        }

        public static void DoctorSubWriter(Host h, Session s)
        {
            
        }
    }
}

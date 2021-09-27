﻿using CommClass;
using Newtonsoft.Json.Linq;
using RemoteHealthcare_Server.Data;
using RemoteHealthcare_Server.Data.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Server.Coms
{
    class JSONLogin
    {

        //Note all needs to be made safe with trys but not done yet kind regards luuk ******************************

        public IUser LoginAction(JObject Jobject, PlaneTextSender sender, Usermanagement management)
        {
            //Checking op login string
            string command = Jobject.GetValue("command").ToString();
            if (command == "login")
            {
                //Getting alle the amazing data
                JObject data = (JObject)Jobject.GetValue("data");
                string username = data.GetValue("us").ToString();
                string password = data.GetValue("pass").ToString();
                int flag = int.Parse(data.GetValue("flag").ToString());


                //Statement for cases for input
                if (flag == 0)
                {
                    IUser user = management.CheckPatientCredentials(username, password);
                    if (user != null)
                    {
                        JSONWriter.LoginWrite(true, sender);
                        Server.PrintToGUI("Authenticated....");
                        return user;
                    }
                } else if (flag == 1)
                {
                    IUser user = management.CheckDoctorCredentials(username, password);
                    if (user != null)
                    {
                        JSONWriter.LoginWrite(true, sender);
                        Server.PrintToGUI("Authenticated....");
                        return user;
                    }

                } else if (flag == 2)
                {
                    IUser user  = management.CheckAdminCredentials(username, password);
                    if (user != null)
                    {
                        JSONWriter.LoginWrite(true, sender);
                        Server.PrintToGUI("Authenticated....");
                        return user;
                    }
                }
                JSONWriter.LoginWrite(false, sender);

                Server.PrintToGUI("Not a user....");
            }


            //Not valid as command
            return null;
        }

    }
}
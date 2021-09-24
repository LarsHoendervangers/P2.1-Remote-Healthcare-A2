using CommClass;
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

        public (int, object) LoginAction(JObject Jobject, EncryptedSender sender, Usermanagement management)
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
                    object o = management.CheckPatientCredentials(username, password);
                    if (o != null)
                    {
                        JSONWriter.LoginWrite(true, sender);
                        return (0, o);
                    }
                } else if (flag == 1)
                {
                    object o = management.CheckDoctorCredentials(username, password);
                    if (o != null) { JSONWriter.LoginWrite(true, sender);
                        return (1, o);
                    }

                } else if (flag == 2)
                {
                    object o = management.CheckAdminCredentials(username, password);
                    if (o != null)
                    {
                        JSONWriter.LoginWrite(true, sender);
                        return (2, o);
                    }
                }
                JSONWriter.LoginWrite(false, sender);
                return (-1, null);
            }


            //Not valid as command
            return (-1, null);
        }

    }
}

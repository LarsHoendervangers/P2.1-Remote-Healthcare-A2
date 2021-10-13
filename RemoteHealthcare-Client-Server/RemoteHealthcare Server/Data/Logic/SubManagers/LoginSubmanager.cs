using RemoteHealthcare_Server.Data.Processing;
using RemoteHealthcare_Server.Data.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Server.Data.Logic
{
    partial class LoginSubmanager
    {
        private UserManagement management;

        public LoginSubmanager(UserManagement management)
        {
            this.management = management;
        }

        /// <summary>
        /// Checks the authenication
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public IUser Credentials(string username, string password, int flag)
        {
            //Finding user
            foreach (IUser user in UserManagement.users)
            {
                if (user.getUserType() == UserTypes.Patient && flag == 0)
                {

                    Patient p = (Patient)user;
                    if (p.Password == HashProcessing.HashString(password) && p.Username == username)
                    {
                        return user;
                    }
                }
                else if (user.getUserType() == UserTypes.Doctor && flag == 1)
                {
                    Doctor d = (Doctor)user;
                    if (d.Password == HashProcessing.HashString(password) && d.Username == username)
                    {
                        return user;
                    }
                }
                else if (user.getUserType() == UserTypes.Admin && flag == 2)
                {
                    Admin a = (Admin)user;
                    Console.WriteLine(a.Password);
                    if (a.Password == HashProcessing.HashString(password) && a.Username == username)
                    {
                        return user;
                    }
                }
            }

            //Patient not found
            return null;
        }
    }
}

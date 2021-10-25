using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Server.Data.User
{
    public enum UserTypes
    {
        Patient, Doctor, Admin, Unkown
    }

    public class UserTypesUtil
    {
        public static UserTypes? Parse(int number)
        {
            if (number == 0)
            {
                return UserTypes.Patient;
            } else if (number == 1)
            {
                return UserTypes.Doctor;
            } else if (number == 2)
            {
                return UserTypes.Admin;
            } else
            {
                return null;
            }
        }
    }
}

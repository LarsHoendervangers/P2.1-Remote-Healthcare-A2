using RemoteHealthcare_Server.Data.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Server.Data.User
{
    public class Admin : IUser
    {
        public string Username { get; set; }

        public string Password{ get; set;}

        public readonly UserTypes type;

        public Admin(string username, string password)
        {
            Username = username;
            Password = HashProcessing.HashString(password);
            type = UserTypes.Admin;
        }


        public UserTypes getUserType()
        {
            return type;
        }
    }
}

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

        public string Password { get; set; }

        public Admin(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public Type getType()
        {
            throw new NotImplementedException();
        }

        int IUser.getType()
        {
            return 2;
        }

        public void sessionSetter(Session s)
        {
           //Not to be implemented
        }

        public Session sessionGetter()
        {
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Server.Data.User
{
    public interface IUser
    {
        public int getType();
        public void sessionSetter(Session s);
        public Session sessionGetter();
    }
}

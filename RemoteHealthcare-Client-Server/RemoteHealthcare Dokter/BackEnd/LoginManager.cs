using Newtonsoft.Json.Linq;
using RemoteHealthcare_Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Dokter.BackEnd
{
    class LoginManager : DataManager
    {
        public override void ReceivedData(JObject data)
        {
            throw new NotImplementedException();
        }

        private void HandleIncoming(JObject data)
        {

        }

        public void SendLogin(string username, string password)
        {

        }

        private void HandleLoginResponse(JObject data)
        {

        }
    }
}

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteHealthcare_Client
{
    public interface IDataManager
    {

        void ReceivedData(JObject data);
    }
}
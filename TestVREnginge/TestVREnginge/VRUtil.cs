using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TestVREngine
{
    class VRUTil
    {
        public static string GetId(string returnedData)
        {
            var data = JsonConvert.DeserializeObject<Id>(returnedData);
            Console.WriteLine(data.data.uuid);
            return data.data.uuid;
        }
      
     
    }

    class Id
    {
        public Data data
        {
            get;
            set;
        }
    }
    class Data
    {
        public string uuid
        {
            get;
            set;
        }
    }
}

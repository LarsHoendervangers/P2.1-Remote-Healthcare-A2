using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TestVREngine
{
    class VRUTil
    {
        public static string GetId(string returnedData)
        {
            Console.WriteLine(returnedData);
            JObject data =(JObject) JsonConvert.DeserializeObject(returnedData);
            string uuID = data.SelectToken("data.data.data.uuid").ToString(); 
            Console.WriteLine(uuID);
            return uuID;
        }
      
     
    }


}

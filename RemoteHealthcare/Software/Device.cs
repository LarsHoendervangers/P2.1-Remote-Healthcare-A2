using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare.Software
{
    abstract class Device
    {
        public DateTime StartTime { get; set; }

        public Device()
        {
           
        }

        public abstract void onHeartBeatReceived(Object sender, Byte[] data);
        public abstract void onBikeReceived(Object sender, Byte[] data);
    }
}

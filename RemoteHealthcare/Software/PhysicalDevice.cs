using RemoteHealthcare.Hardware;
using RemoteHealthcare.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare.Software
{
    class PhysicalDevice : Device
    {
        private HRBLE HRMonitor { get; set; }
        private BikeBLE Bike{ get; set; }

        public PhysicalDevice(string BikeName, string HRName) : base()
        {
            Bike = new BikeBLE(BikeName, this);
            HRMonitor = new HRBLE(HRName, this);

            HRMonitor.onHRData += onHeartBeatReceived;
        }

        public override void onHeartBeatReceived(object sender, byte[] data)
        {
            Console.WriteLine(ProtocolConverter.ReadByte(data, 1));
        }

        public override void onBikeReceived(object sender, byte[] data)
        {
            
        }
    }
}

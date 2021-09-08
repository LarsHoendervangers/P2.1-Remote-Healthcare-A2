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

        public override event EventHandler<double> onSpeed;
        public override event EventHandler<int> onRPM;
        public override event EventHandler<int> onHeartrate;
        public override event EventHandler<double> onDistance;

        public PhysicalDevice(string BikeName, string HRName) : base()
        {
            Bike = new BikeBLE(BikeName, this);
            HRMonitor = new HRBLE(HRName, this);

            HRMonitor.onHRData += OnHeartBeatReceived;
            Bike.onBikeData += OnBikeReceived;
        }

        public override void OnHeartBeatReceived(object sender, byte[] data)
        {
            int heartbeat = ProtocolConverter.ReadByte(data, 1);
            onHeartrate?.Invoke(this, heartbeat);
        }

        public override void OnBikeReceived(object sender, byte[] data)
        {
            // transform the given data to a usefull payload
            Byte[] payload = ProtocolConverter.DataToPayload(data);

            // Check for the pagenumber that 
            if(ProtocolConverter.PageChecker(payload) == 0x10)
            {
                double speed = ProtocolConverter.ReadDataSet(payload, 0x10, true, 4, 5);
                speed = ProtocolConverter.toKMH(speed);
                onSpeed?.Invoke(this, speed);

                double distance = ProtocolConverter.ReadDataSet(payload, 0x10, false, 3);
                distance = ProtocolConverter.rollOver((int)distance, ref prevDistance, ref rollDistance);
                Console.WriteLine(distance);

                double elapsedTime = ProtocolConverter.ReadDataSet(payload, 0x10, false, 2); 

                Console.WriteLine($"Speed: {speed}");
            }

            if (ProtocolConverter.PageChecker(payload) == 0x19)
            {
                int RPM = ProtocolConverter.ReadDataSet(payload, 0x19, false, 2);
                Console.WriteLine($"RPM: {RPM}");

                onRPM?.Invoke(this, RPM);
            }
            

        }
    }
}

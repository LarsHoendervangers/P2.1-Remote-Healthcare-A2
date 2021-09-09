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
        public override event EventHandler<double> onElapsedTime;

        public PhysicalDevice(string BikeName, string HRName) : base()
        {
            Bike = new BikeBLE(BikeName, this);
            HRMonitor = new HRBLE(HRName, this);

            HRMonitor.onHRData += OnHeartBeatReceived;
            Bike.OnBikeData += OnBikeReceived;
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
                // Getting the speed from the bike data
                double speed = ProtocolConverter.ReadDataSet(payload, 0x10, true, 4, 5);
                speed = ProtocolConverter.toKMH(speed);
                onSpeed?.Invoke(this, speed);

                // Getting the distance value from the data
                double distance = ProtocolConverter.ReadDataSet(payload, 0x10, false, 3);
                distance = ProtocolConverter.rollOver((int)distance, ref prevDistance, ref rollDistance);
                onDistance?.Invoke(this, distance);

                // Getting the elapsed time value from the data
                double elapsedTime = ProtocolConverter.ReadDataSet(payload, 0x10, false, 2);
                elapsedTime = (int)(ProtocolConverter.rollOver((int)elapsedTime, ref prevTime, ref rollTime) * 0.25);
                onElapsedTime?.Invoke(this, elapsedTime);
            }

            // When the page is 0x19 these values are read;
            if (ProtocolConverter.PageChecker(payload) == 0x19)
            {
                // Transforming the RPM from the bike
                int RPM = ProtocolConverter.ReadDataSet(payload, 0x19, false, 2);
                onRPM?.Invoke(this, RPM);
            }
        }

        public override void OnResistanceCall(object sender, int data)
        {
            throw new NotImplementedException();
        }
    }
}

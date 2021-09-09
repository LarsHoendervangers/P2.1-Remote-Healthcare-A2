using RemoteHealthcare.Graphics;
using RemoteHealthcare.Hardware;
using RemoteHealthcare.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avans.TI.BLE;

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
        public override event EventHandler<int> onTotalPower;
        public override event EventHandler<int> onCurrentPower;

        public async Task Initialize(int errorcode, int connectionAttempts, string deviceName, string serviceName, string characteristic, BLE Device, IBLEDevice IDevice)
        {
            // Open the correct device, when connection failed it retries to connect
            while (errorcode != 0)
            {
                connectionAttempts += 1;
                errorcode = await Device.OpenDevice(deviceName);
                if (errorcode == 0) continue;
            }

            // Try to set the required service to heartRate
            errorcode = await Device.SetService(serviceName);

            // Set the method called on data receive to onHeartRate()
            Device.SubscriptionValueChanged += IDevice.OnDataReceived;
            errorcode = await Device.SubscribeToCharacteristic(characteristic);
        }

        public PhysicalDevice(string BikeName, string HRName) : base()
        {
            Bike = new BikeBLE(BikeName, this);
            HRMonitor = new HRBLE(HRName, this);

            HRMonitor.onHRData += OnHeartBeatReceived;
            Bike.OnBikeData += OnBikeReceived;

            List<string> list = Bike.ListDevices();
            foreach (string l in list)
            {
                DataGUI.AddDeviceToList(l);
            }
        }

        public void OnHeartBeatReceived(object sender, byte[] data)
        {
            int heartbeat = ProtocolConverter.ReadByte(data, 1);
            onHeartrate?.Invoke(this, heartbeat);
        }

        public void OnBikeReceived(object sender, byte[] data)
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

                // Transforming the totalWattage from the bike
                int totalWattage = ProtocolConverter.ReadDataSet(payload, 0x19, true, 3, 4);
                totalWattage = (int)(ProtocolConverter.rollOverTotalPower(totalWattage, ref prevTotalPower, ref rollTotalPower));
                onTotalPower?.Invoke(this, totalWattage);

                // Transforming the currentWattage from the bike
                payload[6] = (byte)(payload[6] & 0b00001111);
                int currentWattage = ProtocolConverter.ReadDataSet(payload, 0x19, true, 5, 6);
                onCurrentPower?.Invoke(this, currentWattage);
            }
        }

        public override void OnResistanceCall(object sender, int data)
        {
            Bike.ChangeResistance(data);
        }
    }
}

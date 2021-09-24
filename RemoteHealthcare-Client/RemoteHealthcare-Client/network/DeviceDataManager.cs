using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RemoteHealthcare.Ergometer.Software;
using RemoteHealthcare_Client.Ergometer.Software;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace RemoteHealthcare_Client
{
    public class DeviceDataManager : DataManager
    {

        private Device Device { get; set; }

        public DeviceDataManager()
        {
            this.Device = new SimulatedDevice();
            Setup();
        }

        public DeviceDataManager(string bikeName, string HRName)
        {
            if (bikeName.ToLower() == "simulator")
                this.Device = new SimulatedDevice();
            else
                this.Device = new PhysicalDevice(bikeName, HRName);

            Setup();
        }

        private void Setup()
        {
            this.Device.OnSpeed += OnIncomingSpeed;
            this.Device.OnRPM += OnIncomingRPM;
            this.Device.OnHeartrate += OnIncomingHR;
            this.Device.OnCurrentPower += OnIncomingCurPower;
            this.Device.OnTotalPower += OnIncomingTotalPower;
            this.Device.OnDistance += OnIncomingDistance;
            this.Device.OnElapsedTime += OnIncomingTime;


            // TODO implement buffer system so the data to server is not overused
        }

        public void OnIncomingSpeed(object sender, double speed)
        {
            JObject wrappedCommand = JObject.FromObject(PrepareDeviceData(speed, "speed"));

            this.ServerDataManager.ReceivedData(wrappedCommand);
            this.VRDataManager.ReceivedData(wrappedCommand);
        }

        public void OnIncomingRPM(object sender, int speed)
        {
            //JObject wrappedCommand = JObject.FromObject(PrepareDeviceData(speed, "rpm"));

            //this.ServerDataManager.ReceivedData(wrappedCommand);
        }

        public void OnIncomingHR(object sender, int heartrate)
        {

        }

        public void OnIncomingCurPower(object sender, int power)
        {

        }

        public void OnIncomingTotalPower(object sender, int power)
        {

        }

        public void OnIncomingDistance(object sender, double distance)
        {

        }

        public void OnIncomingTime(object sender, double time)
        {

        }

        public override void ReceivedData(JObject data)
        {
            // (See dataprotocol) receivedData wil allways be set resistance

            // command value always gives the action 
            JToken value;

            bool correctCommand = data.TryGetValue("command", StringComparison.InvariantCulture, out value);

            if (!correctCommand)
            {
                Trace.WriteLine("No valid JSON was received to DeviceDataManager");
                return;
            }

            // Looking at the command and switching what behaviour is required
            switch (value.ToString())
            {

                case "setresist":
                    this.Device.OnResistanceCall(this, (int)data.GetValue("data"));
                    break;
                default:
                    // TODO HANDLE NOT SUPPORTER
                    Trace.WriteLine("Error in DeviceDataManager, data received does not meet spec");
                    break;

            }
        }


        
        private object PrepareDeviceData(double value, string key)
        {
            return new
            {
                command = "ergodata",
                data = new
                {
                    time = DateTime.Now.ToString(),
                    speed = value
                }
            };
        }
    }
}
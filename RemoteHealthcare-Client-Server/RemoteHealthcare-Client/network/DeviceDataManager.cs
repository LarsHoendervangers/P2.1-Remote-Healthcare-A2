using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RemoteHealthcare.Ergometer.Software;
using RemoteHealthcare_Client.Ergometer.Software;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace RemoteHealthcare_Client
{
    public class DeviceDataManager : DataManager
    {
        private bool sending;

        private Device Device { get; set; }
        private Dictionary<string, dynamic> SendingDictionary { get; set; }

        public DeviceDataManager(string bikeName, string HRName)
        {
            if (bikeName.ToLower() == "simulator")
                this.Device = new SimulatedDevice();
            else
                this.Device = new PhysicalDevice(bikeName, HRName);
            this.SendingDictionary = new Dictionary<string, dynamic>();
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

            this.sending = true;
            Trace.WriteLine("Started listener thread...");
            new Thread(
                (() =>
                {
                    Trace.WriteLine("Started thread" + Thread.CurrentThread.ManagedThreadId);
                    while (this.sending)
                    {
                        Trace.WriteLine("Sending bikedata");
                        JObject wrappedCommand = JObject.FromObject(PrepareDeviceDataNewton());

                        this.SendToManagers(wrappedCommand);
                        Thread.Sleep(500);

                    }
                })).Start();
        }

        public void OnIncomingSpeed(object sender, double speed)
        {
            //JObject wrappedCommand = JObject.FromObject(PrepareDeviceData(speed, "speed"));

            //this.ServerDataManager.ReceivedData(wrappedCommand);
            //this.VRDataManager.ReceivedData(wrappedCommand);

            //Replace old speed with new speed in dictionary
            ReplaceInDictionary("speed", speed);
        }

        

        public void OnIncomingRPM(object sender, int rpm)
        {
            //JObject wrappedCommand = JObject.FromObject(PrepareDeviceData(speed, "rpm"));

            //this.ServerDataManager.ReceivedData(wrappedCommand);
            ReplaceInDictionary("rpm", rpm);
        }
        
        public void OnIncomingHR(object sender, int heartrate)
        {
            ReplaceInDictionary("bpm", heartrate);
        }

        public void OnIncomingCurPower(object sender, int power)
        {
            ReplaceInDictionary("pow", power);
        }

        public void OnIncomingTotalPower(object sender, int power)
        {
            ReplaceInDictionary("accpow", power);
        }

        public void OnIncomingDistance(object sender, double distance)
        {
            ReplaceInDictionary("dist", distance);
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


        // deprecated 
        private object PrepareDeviceData()
        {
            this.SendingDictionary.TryGetValue("speed", out var speed);
            this.SendingDictionary.TryGetValue("rpm", out var rpm);
            this.SendingDictionary.TryGetValue("bpm", out var heartrate);
            this.SendingDictionary.TryGetValue("pow", out var curpower);
            this.SendingDictionary.TryGetValue("accpow", out var accpower);
            this.SendingDictionary.TryGetValue("dist", out var distance);
            return new
            {
                command = "ergodata",
                data = new
                {
                    time = DateTime.Now.ToString(),
                    rpm = rpm,
                    bpm = heartrate,
                    speed = speed,
                    dist = distance,
                    pow = curpower,
                    accpow = accpower
                }
            };
        }

        private object PrepareDeviceDataNewton()
        {
            JObject ergoObject = new JObject();

            ergoObject.Add("command", "ergodata");

            JObject data = new JObject();
            data.Add("time", DateTime.Now.ToString());
            if(this.SendingDictionary.TryGetValue("rpm", out var rpm)) data.Add("rpm", rpm);
            if(this.SendingDictionary.TryGetValue("bpm", out var heartrate)) data.Add("bpm", heartrate);
            if (this.SendingDictionary.TryGetValue("speed", out var speed)) data.Add("speed", speed);
            if(this.SendingDictionary.TryGetValue("dist", out var distance)) data.Add("dist", distance);
            if (this.SendingDictionary.TryGetValue("pow", out var curpower)) data.Add("pow", curpower);
            if (this.SendingDictionary.TryGetValue("accpow", out var accpower)) data.Add("accpow", accpower);
            
            ergoObject.Add("data", data);
            return ergoObject;
        }

        private void ReplaceInDictionary(string key, dynamic value)
        {
            if (this.SendingDictionary.Remove(key))
            {
              this.SendingDictionary.Add(key, value);
              return;
            }
            this.SendingDictionary.Add(key, value);
        }
    }
}
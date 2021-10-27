using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RemoteHealthcare.Ergometer.Software;
using RemoteHealthcare_Client.Ergometer.Software;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace RemoteHealthcare_Client
{
    /// <summary>
    /// Implementation for DataManager, handles the dataflow from the device
    /// </summary>
    public class DeviceDataManager : DataManager
    {
        // Magic numbers:
        private static int BufferDelay = 500;

        // Variables
        private bool sending;

        private Device Device { get; set; }
        private Dictionary<string, dynamic> SendingDictionary { get; set; }

        /// <summary>
        /// Constructor for DeciveDataManager
        /// </summary>
        /// <param name="bikeName">The name of the bleutooth bike name</param>
        /// <param name="HRName">The name of the hr monitor</param>
        public DeviceDataManager(string bikeName, string HRName)
        {
            this.SendingDictionary = new Dictionary<string, dynamic>();

            // When the given name is "simulator" the simulator gets started
            if (bikeName.ToLower() == "simulator")

                this.Device = new SimulatedDevice();
            else
                this.Device = new PhysicalDevice(bikeName, HRName);

            Setup();
        }

        /// <summary>
        /// Sets up the DeviceDataManager
        /// </summary>
        private void Setup()
        {
            // Binding all the incoming events (s = sender)
            this.Device.OnSpeed += (s,speed) => ReplaceInDictionary("speed", speed);
            this.Device.OnRPM += (s,rpm) => ReplaceInDictionary("rpm", rpm);
            this.Device.OnHeartrate += (s, bpm) => ReplaceInDictionary("bpm", bpm);
            this.Device.OnCurrentPower += (s, power) => ReplaceInDictionary("pow", power);
            this.Device.OnTotalPower += (s, power) => ReplaceInDictionary("accpow", power);
            this.Device.OnDistance += (s, distance) => ReplaceInDictionary("dist", distance);

            this.sending = true;
            // Starting the thread to buffer the incomming data
            Trace.WriteLine("Started listener thread...");
            new Thread(
                (() =>
                {
                    Trace.WriteLine("Started thread" + Thread.CurrentThread.ManagedThreadId);
                    while (this.sending)
                    {
                        Trace.WriteLine("Sending bikedata");
                        JObject wrappedCommand = JObject.FromObject(PrepareDeviceDataNewton());

                        // Broadcasting this data over the data managers
                        this.SendToManagers(wrappedCommand);

                        Thread.Sleep(DeviceDataManager.BufferDelay);
                    }
                })).Start();
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

            // only two data command comming to the device data manager is 'setresist' and 'abort'
            if (value.ToString() == "setresist")
                this.Device.OnResistanceCall(this, (int)data.GetValue("data"));
            else if (value.ToString() == "abort")
            {
                this.Device.OnResistanceCall(this, 0);
                foreach (var process in Process.GetProcesses())
                {
                    // kills the NetworkEngine and the client application when abort is called 
                    if  (process.ProcessName == "RemoteHealthcare-Client")
                    {
                        process.Kill();
                    }
                }
            }
            else
                Trace.WriteLine("Error in DeviceDataManager, data received does not meet spec");
        }

        /// <summary>
        /// Wrapping the buffered data to the correct ergo-data command
        /// </summary>
        /// <returns></returns>
        private object PrepareDeviceDataNewton()
        {
            JObject ergoObject = new JObject();

            ergoObject.Add("command", "ergodata");

            JObject data = new JObject();
            data.Add("time", DateTime.Now.ToString());
            if (this.SendingDictionary.TryGetValue("rpm", out var rpm)) data.Add("rpm", rpm);
            if (this.SendingDictionary.TryGetValue("bpm", out var heartrate)) data.Add("bpm", heartrate);
            if (this.SendingDictionary.TryGetValue("speed", out var speed)) data.Add("speed", speed);
            if (this.SendingDictionary.TryGetValue("dist", out var distance)) data.Add("dist", distance);
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
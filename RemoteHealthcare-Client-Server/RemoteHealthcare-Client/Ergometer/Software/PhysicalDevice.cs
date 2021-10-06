using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avans.TI.BLE;
using RemoteHealthcare_Client.Ergometer.Tools;
using RemoteHealthcare_Client.Ergometer.Software;
using RemoteHealthcare_Client.Ergometer.Hardware;
using RemoteHealthcare_Client.Ergometer.Graphics;
using System.Threading;

namespace RemoteHealthcare.Ergometer.Software
{
    class PhysicalDevice : Device
    {

        public static List<string> ReadAllDevices()
        {
            BLE blDevice = new BLE();

            //Make async or multithreaded
            Thread.Sleep(100);

            return blDevice.ListDevices().FindAll((s)=> s.StartsWith("Tacx Flux"));
        }

        private HRBLE HRMonitor { get; set; }
        private BikeBLE Bike { get; set; }

        // Event handler attributes
        public override event EventHandler<double> OnSpeed;
        public override event EventHandler<int> OnRPM;
        public override event EventHandler<int> OnHeartrate;
        public override event EventHandler<double> OnDistance;
        public override event EventHandler<double> OnElapsedTime;
        public override event EventHandler<int> OnTotalPower;
        public override event EventHandler<int> OnCurrentPower;

        /// <summary>
        /// Starts the connection to the BT devices
        /// </summary>
        /// <param name="deviceName">The name of the device</param>
        /// <param name="serviceName">The service to connect to</param>
        /// <param name="characteristic">The characteristic to subscribe to</param>
        /// <param name="Device">The BLE device created</param>
        /// <param name="IDevice">The IDevice create</param>
        /// <returns>Task</returns>
        public async Task Initialize(string deviceName, string serviceName, string characteristic, BLE Device, IBLEDevice IDevice)
        {
            // Open the correct device, when connection failed it retries to connect
            int errorcode = IDevice.GetErrorCode();
            while (errorcode != 0)
            {
                int connectionAttempts = IDevice.GetConnectionAttempts();
                connectionAttempts += 1;
                IDevice.SetConnectionAttempts(connectionAttempts);
                errorcode = IDevice.GetErrorCode();
                errorcode = await Device.OpenDevice(deviceName);
                IDevice.SetErrorCode(errorcode);
                if (errorcode == 0)
                {
                    //DataGUI.SetMessage($"Succesfully connected to device {deviceName}");
                    Trace.WriteLine($"Succesfully connected to device {deviceName}");
                    continue;
                }
                //DataGUI.SetMessage($"Connection attempts from device {deviceName} is {connectionAttempts}");
                Trace.WriteLine($"Connection attempts from device {deviceName} is {connectionAttempts}");
                Thread.Sleep(500);
            }

            // Try to set the required service to devices' servicename
            errorcode = await Device.SetService(serviceName);

            // Set the method called on data receive to eventhandler
            Device.SubscriptionValueChanged += IDevice.OnDataReceived;
            errorcode = await Device.SubscribeToCharacteristic(characteristic);
        }


        // Value's to handle rollover and start value
        private double initialValueDistance = -1;
        private double initialValueTime = -1;
        private double initialValueWatt = -1;

        public int rollDistance = 0;
        public int prevDistance = 0;

        public int rollTime = 0;
        public int prevTime = 0;

        public int rollTotalPower = 0;
        public int prevTotalPower = 0;

        public int rollCurrentPower = 0;
        public int prevCurrentPower = 0;



        /// <summary>
        /// Constructor for PhysicalDevice, taking the names of the devices to connect to
        /// </summary>
        /// <param name="BikeName">The name of the BT bike</param>
        /// <param name="HRName">The name of the BT HR monitor</param>
        public PhysicalDevice(string BikeName, string HRName) : base()
        {
            Trace.WriteLine("Made new bike");
            Bike = new BikeBLE(BikeName, this);
            HRMonitor = new HRBLE(HRName, this);

            HRMonitor.onHRData += OnHeartBeatReceived;
            Bike.OnBikeData += OnBikeReceived;
        }


        /// <summary>
        /// Event call that handles the translation of the data from the heartbeat monitor
        /// </summary>
        /// <param name="sender">The object that called the event</param>
        /// <param name="data">THe data from the event</param>
        public void OnHeartBeatReceived(object sender, byte[] data)
        {
            int heartbeat = ProtocolConverter.ReadByte(data, 1);
            OnHeartrate?.Invoke(this, heartbeat);
        }

        /// <summary>
        /// Event call that handles the translation of the data from the bike
        /// </summary>
        /// <param name="sender">The object that called the event</param>
        /// <param name="data">The data received through the event</param>
        public void OnBikeReceived(object sender, byte[] data)
        {
            // transform the given data to a usefull payload
            byte[] payload = ProtocolConverter.DataToPayload(data);

            // Check for the pagenumber that 
            if (ProtocolConverter.PageChecker(payload) == 0x10)
            {
                // Getting the speed from the bike data
                double speed = ProtocolConverter.ReadDataSet(payload, 0x10, true, 4, 5);
                speed = ProtocolConverter.TransformtoKMH(speed);
                OnSpeed?.Invoke(this, speed);

                // Getting the distance value from the data
                double distance = ProtocolConverter.ReadDataSet(payload, 0x10, false, 3);
                distance = ProtocolConverter.rollOver((int)distance, ref prevDistance, ref rollDistance);
                distance = InitialValueComp(distance, ref initialValueDistance);
                OnDistance?.Invoke(this, distance);

                // Getting the elapsed time value from the data
                double elapsedTime = ProtocolConverter.ReadDataSet(payload, 0x10, false, 2);
                elapsedTime = (int)(ProtocolConverter.rollOver((int)elapsedTime, ref prevTime, ref rollTime) * 0.25);
                elapsedTime = InitialValueComp(elapsedTime, ref initialValueTime);
                OnElapsedTime?.Invoke(this, elapsedTime);
            }

            // When the page is 0x19 these values are read;
            if (ProtocolConverter.PageChecker(payload) == 0x19)
            {
                // Transforming the RPM from the bike
                int RPM = ProtocolConverter.ReadDataSet(payload, 0x19, false, 2);
                OnRPM?.Invoke(this, RPM);

                // Transforming the totalWattage from the bike
                int totalWattage = ProtocolConverter.ReadDataSet(payload, 0x19, true, 3, 4);
                totalWattage = ProtocolConverter.rollOverTotalPower(totalWattage, ref prevTotalPower, ref rollTotalPower);
                totalWattage = (int)InitialValueComp(totalWattage, ref initialValueWatt);
                OnTotalPower?.Invoke(this, totalWattage);

                // Transforming the currentWattage from the bike
                payload[6] = (byte)(payload[6] & 0b00001111);
                int currentWattage = ProtocolConverter.ReadDataSet(payload, 0x19, true, 5, 6);
                OnCurrentPower?.Invoke(this, currentWattage);
            }

        }

        /// <summary>
        /// Event method for setting the resistance on the bike 
        /// </summary>
        /// <param name="sender">The object that called the event</param>
        /// <param name="data">The data given in the event</param>
        public override void OnResistanceCall(object sender, int data)
        {
            Bike.ChangeResistance(data); // Telling the bike to change the resistance
        }

        /// <summary>
        /// Method that handles the initial value's
        /// </summary>
        /// <param name="value">The value to be checked</param>
        /// <param name="initialValue">Reference to double where in initial is stored</param>
        /// <returns>The adjusted value</returns>
        double InitialValueComp(double value, ref double initialValue)
        {
            if (initialValue == -1)
                initialValue = value;

            return value - initialValue;
        }
    }
}

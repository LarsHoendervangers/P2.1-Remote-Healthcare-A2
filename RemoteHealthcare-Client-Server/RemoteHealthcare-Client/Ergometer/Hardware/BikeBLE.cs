using Avans.TI.BLE;
using RemoteHealthcare.Ergometer.Software;
using RemoteHealthcare_Client.Ergometer.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RemoteHealthcare_Client.Ergometer.Hardware
{
    class BikeBLE : BLE, IBLEDevice
    {
        public event EventHandler<byte[]> OnBikeData;
        private readonly PhysicalDevice device;
        private readonly string bikeName;
        private int errorcode = 1;
        private int connectionAttempts = 0;

        public BikeBLE(string BikeName, PhysicalDevice device) : base()
        {
            this.device = device;
            bikeName = BikeName;
            // Waiting beforeinitializing
            Thread.Sleep(100);

            Console.WriteLine("Initializing...");
            Task task = device.Initialize(bikeName,
                "6e40fec1-b5a3-f393-e0a9-e50e24dcca9e", "6e40fec2-b5a3-f393-e0a9-e50e24dcca9e", this, this);
        }

        /*
         * Starts the connection to the bike and subscribes to the correct value's
         */
        private async Task Initialize()
        {
            // Open the correct device, when connection failed it retries to connect
            while (errorcode != 0)
            {
                connectionAttempts += 1;
                errorcode = await OpenDevice(bikeName);
                if (errorcode == 0) continue;
            }

            // Try to set the required service to heartRate
            errorcode = await SetService("6e40fec1-b5a3-f393-e0a9-e50e24dcca9e");

            // Set the method called on data receive to onHeartRate()
            SubscriptionValueChanged += OnDataReceived;
            errorcode = await SubscribeToCharacteristic("6e40fec2-b5a3-f393-e0a9-e50e24dcca9e");
        }

        /*
         * Event method that is called when the BLE receives data.
         * The method checks if the data is correct and sends it to the device class for decoding.
         */
        public void OnDataReceived(object sender, BLESubscriptionValueChangedEventArgs e)
        {
            // check if the date was received correct, by checking the checksum
            if (ProtocolConverter.MichaelChecksum(e.Data))
            {
                //Invoking the the event to sent the data to the Device class
                OnBikeData?.Invoke(this, e.Data);
            }
        }

        //Changes the resistance of the bike to the given value / 2.
        public void ChangeResistance(int resistance)
        {
            // Setting the byte array needed to the correct value's
            byte[] data = new byte[13] { 0xA4, 0x09, 0x4E, 0x05, 0x30, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, (byte)resistance, 0 };

            // calculating the checksum 
            data[12] = (byte)ProtocolConverter.CalculateChecksum(data);
            WriteCharacteristic("6e40fec3-b5a3-f393-e0a9-e50e24dcca9e", data);
        }

        /// <summary>
        /// Sets this.errorcode the the given errorcode
        /// </summary>
        /// <param name="errorcode">the errorcode to set this.errorcode to</param>
        public void SetErrorCode(int errorcode)
        {
            this.errorcode = errorcode;
        }

        /// <summary>
        /// Sets the amount of connection attempts to the given value
        /// </summary>
        /// <param name="connectionAttempts">The connection attempt to set this.connectionAttemps</param>
        public void SetConnectionAttempts(int connectionAttempts)
        {
            this.connectionAttempts = connectionAttempts;
        }

        /// <summary>
        /// Getter for this.errorcode
        /// </summary>
        /// <returns>this.errorcode</returns>
        public int GetErrorCode()
        {
            return errorcode;
        }

        /// <summary>
        /// Getter for this.connectionAttemps
        /// </summary>
        /// <returns>this.connectionAttemps</returns>
        public int GetConnectionAttempts()
        {
            return connectionAttempts;
        }
    }
}
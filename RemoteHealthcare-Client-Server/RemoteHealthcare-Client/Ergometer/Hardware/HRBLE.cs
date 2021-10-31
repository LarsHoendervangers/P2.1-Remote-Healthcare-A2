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
    /// <summary>
    /// This class reads the data from the hearrate monitor
    /// </summary>
    class HRBLE : BLE, IBLEDevice
    {

        private readonly PhysicalDevice device;
        private readonly string hrMonitorname;
        int errorcode = 1;
        private int connectionAttempts = 0;

        public event EventHandler<byte[]> onHRData;

        public HRBLE(string hrMonitorname, PhysicalDevice device) : base()
        {
            this.device = device;
            this.hrMonitorname = hrMonitorname;
            // Waiting beforeinitializing
            Thread.Sleep(1000);

            Task task = device.Initialize(hrMonitorname, "HeartRate", "HeartRateMeasurement", this, this);
            
        }

        /// <summary>
        /// This mehtod is called to start reading bluetooth data from a Heartrate sensor, it subscribes to the given device
        /// </summary>
        /// <returns></returns>
        private async Task Initialize()
        {
            // Open the correct device
            while (errorcode == 1)
            {
                errorcode = errorcode = await OpenDevice(hrMonitorname);
                connectionAttempts += 1;
                if (errorcode == 0) continue;
            }
            // Try to set the required service to heartRate
            await SetService("HeartRate");

            // Set the method called on data receive to onHeartRate()
            SubscriptionValueChanged += OnDataReceived;
            await SubscribeToCharacteristic("HeartRateMeasurement");
        }

        /// <summary>
        /// Event method that is called when the BLE receives data.
        /// The method checks if the data is correct and sends it to the device class for decoding.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnDataReceived(object sender, BLESubscriptionValueChangedEventArgs e)
        {
            if (ProtocolConverter.ConfirmPageData(e.Data))
                onHRData?.Invoke(this, e.Data);
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

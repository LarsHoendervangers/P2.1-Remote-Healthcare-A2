using Avans.TI.BLE;
using RemoteHealthcare.Software;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RemoteHealthcare.Hardware
{
    class BikeBLE : BLE
    {

        public event EventHandler<Byte[]> onBikeData;
        private readonly PhysicalDevice device;
        private readonly string BikeName;
        int errorcode = 0;

        public BikeBLE(string BikeName, PhysicalDevice device) : base()
        {
            this.device = device;
            this.BikeName = BikeName;
            // Waiting beforeinitializing
            Thread.Sleep(1000);


            // TODO remove
            List<string> list = ListDevices();
            foreach (string l in list)
            {
                Console.WriteLine(l);
            }


            // ignore async problem, function can continue
            Initialize();
        }

        private async Task Initialize()
        {
            // Open the correct device
            this.errorcode = this.errorcode = await OpenDevice(BikeName);

            // Try to set the required service to heartRate
            this.errorcode = await SetService("6e40fec1-b5a3-f393-e0a9-e50e24dcca9e");

            // Set the method called on data receive to onHeartRate()
            SubscriptionValueChanged += onBikeMovement;
            this.errorcode = await SubscribeToCharacteristic("6e40fec2-b5a3-f393-e0a9-e50e24dcca9e");


        }

        private bool checkReceivedBike()
        {
            return true; //TODO not implemented
        }

        private void onBikeMovement(object sender, BLESubscriptionValueChangedEventArgs e)
        {
            this.onBikeData?.Invoke(this, e.Data);
        }

    }
}

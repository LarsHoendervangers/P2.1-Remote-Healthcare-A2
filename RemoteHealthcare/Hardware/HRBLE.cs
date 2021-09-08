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
    class HRBLE : BLE
    {

        private readonly PhysicalDevice device;
        private readonly string hrMonitorname;
        int errorcode = 0;

        public event EventHandler<Byte[]> onHRData;

        public HRBLE(string hrMonitorname, PhysicalDevice device) : base()
        {
            this.device = device;
            this.hrMonitorname = hrMonitorname;
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
            this.errorcode = await OpenDevice(hrMonitorname);

            // Try to set the required service to heartRate
            await SetService("HeartRate");

            // Set the method called on data receive to onHeartRate()
            SubscriptionValueChanged += onHearthRate;
            await SubscribeToCharacteristic("HeartRateMeasurement");

            
        }

        private bool checkReceivedHR() 
        {
            return true; //TODO not implemented
        }

        private void onHearthRate(object sender, BLESubscriptionValueChangedEventArgs e)
        {
            onHRData?.Invoke(this, e.Data);
        }

    }
}

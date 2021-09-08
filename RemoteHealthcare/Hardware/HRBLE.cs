using Avans.TI.BLE;
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

        private readonly string hrMonitorname;
        int errorcode = 0;

        public HRBLE(string hrMonitorname) : base()
        {
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

            Console.WriteLine("Received from {0}: {1}, {2}", e.ServiceName,
                BitConverter.ToString(e.Data).Replace("=", " "),
                Encoding.UTF8.GetString(e.Data));
        }

    }
}

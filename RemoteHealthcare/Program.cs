using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avans.TI.BLE;
using RemoteHealthcare.UI;

namespace RemoteHealthcare
{
    class Program

    {
        static async Task Main(string[] args)
        {
            int errorCode = 0;
            BLE bleBike = new BLE();
            BLE bleHeart = new BLE();

            Thread.Sleep(1000); // We need some time to list available devices

            // List available devices
            List<String> bleBikeList = bleBike.ListDevices();
            //Console.WriteLine("Devices found: ");
            foreach (var name in bleBikeList)
            {
                Console.WriteLine($"Device: {name}");
            }

            // Connecting
            errorCode = errorCode = await bleBike.OpenDevice("Tacx Flux 01249");
            // __TODO__ Error check

            var services = bleBike.GetServices;
            foreach (var service in services)
            {
                Console.WriteLine($"Service: {service}");
            }

            // Set service
            errorCode = await bleBike.SetService("6e40fec1-b5a3-f393-e0a9-e50e24dcca9e");
            // __TODO__ error check

            // Subscribe
            bleBike.SubscriptionValueChanged += BleBike_SubscriptionValueChanged;
            errorCode = await bleBike.SubscribeToCharacteristic("6e40fec2-b5a3-f393-e0a9-e50e24dcca9e");

            // Heart rate
            errorCode = await bleHeart.OpenDevice("Decathlon Dual HR");

            await bleHeart.SetService("HeartRate");

            bleHeart.SubscriptionValueChanged += BleBike_SubscriptionValueChanged;
            await bleHeart.SubscribeToCharacteristic("HeartRateMeasurement");

            Console.Read();
        }
        

        private static void BleBike_SubscriptionValueChanged(object sender, BLESubscriptionValueChangedEventArgs e)
        {
            //Console.WriteLine("Received from {0}: {1}, {2}, {3}", e.ServiceName,
            //   BitConverter.ToString(e.Data).Replace("-", " "),
            //   Encoding.UTF8.GetString(e.Data), ShowValue(DataToPayload(e.Data)
            //   ));

            ShowValue(DataToPayload(e.Data));
        }

        private static String ByteArrayToString(byte[] array)
        {
            String toReturn = "";

            foreach (var name in array)
            {
                toReturn += name;
            }

            return toReturn;
        }

        private static byte PageChecker(byte[] payload)
        {
            //Filter on byte array size, should be redundant
            //if (payload.Length != 8) return (byte)0xFF;

            return payload[0];
        }

        //TODO make method so we can give multiple parameters for pagenumber and payloadnumber 
        private static byte ShowValue(byte[] payload)
        {
            byte pagenumber = PageChecker(payload);

            if (pagenumber == 16)
            {
                  //Console.WriteLine(" {0}", payload[3]);
                Console.WriteLine("Speed: {0} km/h", (double)CombineBits(payload[5], payload[4]) * 0.001 * 3.6);
                return payload[3];
            }

            if (pagenumber == 25)
            {
                //Console.WriteLine("Rpm: {0}", payload[2]);
                return payload[2];
            }

            return (byte)0xFF;
        }

        //Data size is 13, this is the full dataset received by the device
        //Payload size is 8, contains the information obtained by the sensor
        private static byte[] DataToPayload(byte[] data)
        {
            if (data.Length != 13) return new byte[8];

            byte[] toReturn = new byte[8];

            for (int i = 4; i < 12; i++)
            {
                toReturn[i - 4] = data[i];
            }
            return toReturn;
        }

        private static ushort CombineBits(byte byte1, byte byte2)
        {
            //Bit shift first bit 8 to the left
            ushort combined = byte1;
            combined = (ushort)(combined << 8);

            //Or both bytes
            return (ushort)(combined | byte2);
        }

        public void Start()
        {
            ConsoleWindow consoleWindow = new ConsoleWindow();
            consoleWindow.PrintData();
        }
    }
}
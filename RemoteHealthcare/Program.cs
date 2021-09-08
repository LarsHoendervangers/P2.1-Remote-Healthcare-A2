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
        private static ConsoleWindow consoleWindow = new ConsoleWindow();

        private static int distanceCounter = 0;
        private static int previousDistance = 0;
        private static int timeCounter = 0;
        private static int previousTime = 0;

        static async Task Main(string[] args)
        {

            Console.Clear();
            consoleWindow.PrintData();

            int errorCode = 0;
            BLE bleBike = new BLE();
            BLE bleHeart = new BLE();

            Thread.Sleep(1000); // We need some time to list available devices

            // List available devices
            List<String> bleBikeList = bleBike.ListDevices();

            // Connecting
            errorCode = errorCode = await bleBike.OpenDevice("Tacx Flux 01249");
            // __TODO__ Error check

            var services = bleBike.GetServices;

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
            return payload[0];
        }

        //TODO make method so we can give multiple parameters for pagenumber and payloadnumber 
        private static byte ShowValue(byte[] payload)
        {
            
            byte pagenumber = PageChecker(payload);

            if (pagenumber == 16)
            {
                consoleWindow.OnSpeedChanged((double)CombineBits(payload[5], payload[4]) * 0.001 * 3.6);
                if (payload[3] < previousDistance)
                {
                    distanceCounter += 1;
                }
                previousDistance = payload[3];
                consoleWindow.OnDistanceChanged((distanceCounter * 256) + payload[3]);
                
                if (payload[2] < previousTime)
                {
                    timeCounter += 1;
                }
                previousTime = payload[2];
                consoleWindow.OnElapsedTime(((timeCounter * 64) + (payload[2] / 4)));
                return payload[3];
            }

            if (pagenumber == 25)
            {
                consoleWindow.OnRPMChanged(payload[2]);
                return payload[2];
            }

            if (pagenumber == 22)
            {
                consoleWindow.OnHeartBeatChanged(payload[1]);
            }

            return (byte)0xFF;
        }

        //Data size is 13, this is the full dataset received by the device
        //Payload size is 8, contains the information obtained by the sensor
        private static byte[] DataToPayload(byte[] data)
        {
            if (data.Length <= 6)
            {
               return data;
            }

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
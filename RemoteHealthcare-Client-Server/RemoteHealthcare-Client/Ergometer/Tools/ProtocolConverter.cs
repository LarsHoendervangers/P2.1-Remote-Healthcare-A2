using Avans.TI.BLE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Client.Ergometer.Tools
{
    public class ProtocolConverter
    {
        /// <summary>
        /// Converts a bytearray to a string, this can be used for displaying the contents of the array.
        /// </summary>
        public static string ByteArrayToString(byte[] array)
        {
            string toReturn = "";

            foreach (var name in array)
            {
                toReturn += name;
            }
            return toReturn;
        }

        /// <summary>
        /// Returns the pagenumber for a given payload
        /// </summary>
        /// <param name="payload"></param>
        /// <returns>returns the pagenumber as byte</returns>
        public static byte PageChecker(byte[] payload)
        {
            return payload[0];
        }

        /// <summary>
        /// Given a payload the method gives the value for the asked indexes
        /// </summary>
        /// <param name="payload">The payload to extract the data from</param>
        /// <param name="targetPageNumber">The page that needs to be read</param>
        /// <param name="mustCombine">true if the data to be read is greater than 1 byte</param>
        /// <param name="targetIndex">A array of the indexes from the payload to be combined</param>
        /// <returns>Returns the requested data as int</returns>
        public static int ReadDataSet(byte[] payload, byte targetPageNumber, bool mustCombine, params int[] targetIndex)
        {
            //Check if we're reading the correct page
            byte pageNumberReceived = PageChecker(payload);
            if (pageNumberReceived == targetPageNumber)
            {
                //received bits to combine
                if (mustCombine && targetIndex.Length == 2) return CombineBits(payload[targetIndex[1]], payload[targetIndex[0]]);

                //received one bit and returns payload contents
                if (targetIndex.Length == 1) return payload[targetIndex[0]];
            }
            System.Diagnostics.Debug.WriteLine("Could not read dataset {0} from page {1} with first targetbyte {2}", ByteArrayToString(payload), targetPageNumber, targetIndex[0]);
            return -1;
        }

        /// <summary>
        /// Converts the byte to an int.
        /// </summary>
        /// <param name="data">The data to search in.</param>
        /// <param name="targetByte">The position in the data.</param>
        /// <returns>returns the asked byte as int</returns>
        public static int ReadByte(byte[] data, int targetByte)
        {
            if (data.Length > targetByte) return data[targetByte];

            Console.WriteLine("Error in reading from dataset, dataset too small");
            return -1;
        }

        /// <summary>
        /// Data size is 13, this is the full dataset received by the device
        /// Payload size is 8, contains the information obtained by the sensor
        /// </summary>
        /// <param name="data">The data that needs to be converted</param>
        /// <returns>The payload from the data</returns>
        public static byte[] DataToPayload(byte[] data)
        {
            if (data.Length != 13) return new byte[8];

            byte[] toReturn = new byte[8];

            for (int i = 4; i < 12; i++)
            {
                toReturn[i - 4] = data[i];
            }
            return toReturn;
        }

        /// <summary>
        /// Combines two bytes into one byte
        /// </summary>
        /// <param name="byte1"></param>
        /// <param name="byte2"></param>
        /// <returns></returns>
        public static ushort CombineBits(byte byte1, byte byte2)
        {
            //Bit shift first bit 8 to the left
            ushort combined = byte1;
            combined = (ushort)(combined << 8);

            //Or both bytes
            return (ushort)(combined | byte2);
        }

        /// <summary>
        /// Checks the checksum from the given data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool MichaelChecksum(byte[] data)
        {
            if (data.Length == 13)
            {
                byte sendChecksum = data[12];
                int temperoryChecksum = data[0];
                for (int i = 1; i < 12; i++)
                {
                    temperoryChecksum = temperoryChecksum ^ data[i];
                }
                byte calcutedChecksum = (byte)temperoryChecksum;

                return calcutedChecksum == sendChecksum;
            }
            return false;
        }

        //Calculates checksum from given byte array.
        public static int CalculateChecksum(byte[] data)
        {
            int checksum = data[0];

            for (int i = 1; i < 12; i++)
            {
                checksum = checksum ^ data[i];
            }
            return checksum;
        }

        /// <summary>
        /// Method which makes a calculation with the amount of rollovers there have been
        /// since the connection was made so the distance and time have the right values
        /// </summary>
        /// <param name="value"></param>
        /// <param name="oldValue"></param>
        /// <param name="valueCounter"></param>
        /// <returns></returns>
        public static int rollOver(int value, ref int oldValue, ref int valueCounter)
        {
            if (value < oldValue)
            {
                valueCounter++;
            }

            int returnValue = valueCounter * 256 + value;
            oldValue = value;

            return returnValue;
        }

        /// <summary>
        /// Method which makes a calculation with the amount of rollovers there have been
        /// since the connection was made so the total power has the right values
        /// </summary>
        /// <param name="value"></param>
        /// <param name="oldValue"></param>
        /// <param name="valueCounter"></param>
        /// <returns>Int for new value</returns>
        public static int rollOverTotalPower(int value, ref int oldValue, ref int valueCounter)
        {
            if (value < oldValue) valueCounter++;

            int returnValue = valueCounter * 65536 + value;
            oldValue = value;

            return returnValue;
        }

        /// <summary>
        /// Confirms the page number to be working in page 0x16
        /// </summary>
        /// <param name="data">The given payload</param>
        /// <returns>true if number is correct</returns>
        public static bool ConfirmPageData(byte[] data) => data[0] == 0x16;

        /// <summary>
        /// Transforms the given data to km/h
        /// </summary>
        /// <param name="speed">The speed inn 1000th of mp/h</param>
        /// <returns>The speed in km/h</returns>
        public static double TransformtoKMH(double speed)
        {
            return speed * 0.0036;
        }
    }
}

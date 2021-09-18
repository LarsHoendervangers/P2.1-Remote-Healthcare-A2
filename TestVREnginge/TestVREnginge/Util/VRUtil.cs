using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TestVREngine.Util
{
    static class VRUTil
    {
        /// <summary>
        /// Given JSON data returned from the server this method finds the uuid of the data
        /// </summary>
        /// <param name="returnedData">The JSON to be searched</param>
        /// <returns>The uuid in de data</returns>
        public static string GetId(string returnedData)
        {
            Trace.WriteLine($"VRutil: data received: {returnedData}, \n");
            JObject data = (JObject)JsonConvert.DeserializeObject(returnedData);
            string uuID = data.SelectToken("data.data.data.uuid").ToString(); //error handling nog needed, but is prefered TODO

            Trace.WriteLine($"VRutil: found uuID: {uuID} \n");

            return uuID;
        }

        /// <summary>
        /// Given the parameters this method creates a heightmap using perlin noise
        /// </summary>
        /// <param name="width">The width of the heightmap</param>
        /// <param name="height">The height of the heightmap</param>
        /// <param name="range">The range the value's can be between 0-range</param>
        /// <param name="multiplier">Multiplier applied to data</param>
        /// <returns>float array filled with perlin noise</returns>
        public static float[] GenerateTerrain(int width, int height, int range, float multiplier)
        {
            Random random = new Random();
            int randomSeedX = width + random.Next(0, 1000);
            int randomSeedY = height + random.Next(0, 1000);

            float[] terrain = new float[width * height];

            for (int i = randomSeedX; i < randomSeedX + width; i++)
            {
                for (int j = randomSeedY; j < randomSeedY + height; j++)
                {
                    terrain[(i - randomSeedX) * width + (j - randomSeedY)] = SimplexNoise.Noise.CalcPixel2D(i, j, multiplier) / 255f * range;

                }
            }

       



            return terrain;
        }

        // Debug method, prints a float[] to the console in readable data
        public static void PrintMatirx(float[] input, int CutOffPoint)
        {
            for (int i = 0; i < input.Length; i++)
            {
                Console.Write(input[i] + "\t");

                if ((i + 1) % CutOffPoint == 0)
                {
                    Console.Write("\n");
                }

            }

        }


    }


}

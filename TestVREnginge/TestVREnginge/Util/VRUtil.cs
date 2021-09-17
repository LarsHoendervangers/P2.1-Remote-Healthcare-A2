using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TestVREngine.Util
{
    static class VRUTil
    {
        public static string GetId(string returnedData)
        {
            Console.WriteLine(returnedData);
            JObject data = (JObject)JsonConvert.DeserializeObject(returnedData);
            string uuID = data.SelectToken("data.data.data.uuid").ToString();
            Console.WriteLine(uuID);
            return uuID;
        }

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

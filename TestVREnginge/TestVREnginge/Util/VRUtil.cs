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

        public static float[] GenerateTerrain(uint renderresulution, int width, int height, int range, float multiplier)
        {
            Random random = new Random();
            int renderW = (int)(width * renderresulution);
            int renderH =(int) (height * renderresulution);


            int randomSeedX = renderW + random.Next(0, 1000);
            int randomSeedY = renderH + random.Next(0, 1000);

            float[] render = new float[width * height];

            for (int i = randomSeedX; i < randomSeedX + renderW; i++)
            {
                for (int j = randomSeedY; j < randomSeedY + renderH; j++)
                {
                    render[(i - randomSeedX) * renderW + (j - randomSeedY)] = SimplexNoise.Noise.CalcPixel2D(i, j, multiplier) / 255f * range;
                }
            }

            PrintMatirx(render, width);

            float[] terrain = new float[width * height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
              /*      for (int i = 0; i < length; i++)
                    {

                    }
                    terraini[i * width + j] = */
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

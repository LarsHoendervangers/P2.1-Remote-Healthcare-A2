using RemoteHealthcare.Software;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare.Graphics
{
    class DataGUI
    {

        public DataGUI()
        {
            Device device = new PhysicalDevice("Tacx Flux 00438", "Decathlon Dual HR");
            device.onHeartrate += drawHeartrate;
            device.onRPM += drawRPM;
            device.onSpeed += drawSpeed;
            device.onDistance += drawDistance;
            device.onElapsedTime += drawElapsedTime;
        }

        static void Main(string[] args)
        {
            new DataGUI();
            Console.Read();
        }



        private void drawHeartrate(Object sender, int heartrate)
        {
            Console.WriteLine($"GUI BPM: {heartrate}");

        }


        private void drawRPM(object sender, int e)
        {
            
        }
        private void drawSpeed(object sender, double e)
        {
      
        }

        private void drawDistance(object sender, double e)
        {

        }


        private void drawElapsedTime(object sender, int e)
        {
       
        }

    }
}

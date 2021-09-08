using RemoteHealthcare.Hardware;
using RemoteHealthcare.Software;
using RemoteHealthcare.Tools;
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
            device.onHeartrate += DrawHeartrate;
            device.onRPM += DrawRPM;
            device.onSpeed += DrawSpeed;
            device.onDistance += DrawDistance;
            device.onElapsedTime += DrawElapsedTime;
        }

        static void Main(string[] args)
        {
            new DataGUI();


            Console.Read();
        }

        private void DrawRPM(object sender, int e)
        {
            
        }
        private void DrawSpeed(object sender, double e)
        {
      
        }

        public void DrawHeartrate(Object sender, int heartrate)
        {

            Console.WriteLine($"GUI BPM: {heartrate}");
        }


        private void DrawElapsedTime(object sender, int e)
        {
       
        }

        private void DrawDistance(object sender, double e)
        {

        }

    }
}

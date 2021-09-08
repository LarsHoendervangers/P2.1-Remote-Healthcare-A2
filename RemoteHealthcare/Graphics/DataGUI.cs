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
        private static EventHandler<int> drawSpeed;

        public DataGUI()
        {
            Device device = new PhysicalDevice("Tacx Flux 00438", "Decathlon Dual HR");
            device.onHeartrate += drawHeartrate;
        }

        static void Main(string[] args)
        {
            //HRBLE hr = new HRBLE("Decathlon Dual HR");
            //BikeBLE bike = new BikeBLE("Tacx Flux 00438");

            

            new DataGUI();


            Console.Read();
        }



        public void drawHeartrate(Object sender, int heartrate)
        {
            Console.WriteLine($"GUI BPM: {heartrate}");

        }
    }
}

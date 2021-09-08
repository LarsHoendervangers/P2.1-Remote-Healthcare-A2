using RemoteHealthcare.Hardware;
using RemoteHealthcare.Software;
using System;

namespace RemoteHealthcare
{
    class Program

    {
        static void Main(string[] args)
        {
            //HRBLE hr = new HRBLE("Decathlon Dual HR");
            //BikeBLE bike = new BikeBLE("Tacx Flux 00438");

            Device device = new PhysicalDevice("Tacx Flux 00438", "Decathlon Dual HR");

            Console.Read();
        }
    }
}
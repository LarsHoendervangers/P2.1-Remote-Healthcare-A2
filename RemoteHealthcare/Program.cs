using RemoteHealthcare.Hardware;
using System;

namespace RemoteHealthcare
{
    class Program

    {
        static void Main(string[] args)
        {
            //HRBLE hr = new HRBLE("Decathlon Dual HR");
            BikeBLE bike = new BikeBLE("Tacx Flux 00438");

            Console.Read();
        }
    }
}
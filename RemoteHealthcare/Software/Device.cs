using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare.Software
{
    abstract class Device
    {
        public int Heartbeat { get; set; }
        public int RPM { get; set; }
        public double Speed { get; set; }
        public double DistanceTravelled { get; set; }
        public DateTime StartTime { get; set; }

        public Device()
        {

        }
    }
}

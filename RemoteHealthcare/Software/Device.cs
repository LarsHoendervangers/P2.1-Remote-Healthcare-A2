using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace RemoteHealthcare.Software
{
    abstract class Device {

    public abstract event EventHandler<double> OnSpeed;
    public abstract event EventHandler<int> OnRPM;
    public abstract event EventHandler<int> OnHeartrate;
    public abstract event EventHandler<double> OnDistance;
    public abstract event EventHandler<double> OnElapsedTime;
    public abstract event EventHandler<int> OnTotalPower;
    public abstract event EventHandler<int> OnCurrentPower;

    public DateTime StartTime { get; set; }

        public int rollDistance = 0;
        public int prevDistance = 0;

        public int rollTime = 0;
        public int prevTime = 0;

        public int rollTotalPower = 0;
        public int prevTotalPower = 0;

        public int rollCurrentPower = 0;
        public int prevCurrentPower = 0;
        
        // Event that is called by DataGUI when a user enters a resistance value.
        public virtual void OnResistanceCall(Object sender, int data)
        {
            System.Diagnostics.Debug.WriteLine($"Resistance of the bike set to {data}");
        }
    }
}

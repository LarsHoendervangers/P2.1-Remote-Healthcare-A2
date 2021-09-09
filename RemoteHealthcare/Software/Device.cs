using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace RemoteHealthcare.Software
{
    abstract class Device {

    public abstract event EventHandler<double> onSpeed;
    public abstract event EventHandler<int> onRPM;
    public abstract event EventHandler<int> onHeartrate;
    public abstract event EventHandler<double> onDistance;
    public abstract event EventHandler<double> onElapsedTime;
    public abstract event EventHandler<int> onTotalPower;
    public abstract event EventHandler<int> onCurrentPower;

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

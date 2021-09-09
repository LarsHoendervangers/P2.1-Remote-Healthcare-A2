using RemoteHealthcare.Debug;
using RemoteHealthcare.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare.Software
{
    class SimulatedDevice : Device
    {
        private SimDataGenerator Generator{ get; set; }
        private SimGUI User{ get; set; }
        

        public override event EventHandler<double> onSpeed;
        public override event EventHandler<int> onRPM;
        public override event EventHandler<int> onHeartrate;
        public override event EventHandler<double> onDistance;
        public override event EventHandler<double> onElapsedTime;
        public override event EventHandler<int> onTotalPower;
        public override event EventHandler<int> onCurrentPower;

        public SimulatedDevice()
        {
            Generator = new SimDataGenerator();

            Generator.GeneratedDistance += OnGeneratedDistance;
            Generator.GeneratedHeartrate += OnGeneratedHeartRate;
            Generator.GeneratedRPM += OnGeneratedRPM;
            Generator.GeneratedDistance += OnGeneratedDistance;
            Generator.GeneratedTime += OnGenerateTime;
        }

        private void OnGenerateTime(object sender, double e)
        {
            onElapsedTime?.Invoke(this, e);
        }

        private void OnGeneratedRPM(object sender, int e)
        {
            onRPM?.Invoke(this, e);
        }

        private void OnGeneratedHeartRate(object sender, int e)
        {
            onHeartrate?.Invoke(this, e);
        }

        private void OnGeneratedDistance(object sender, double e)
        {
            onDistance?.Invoke(this, e);
        }

      
    }
}

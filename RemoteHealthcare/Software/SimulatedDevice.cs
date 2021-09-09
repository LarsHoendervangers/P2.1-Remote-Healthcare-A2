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

        public SimulatedDevice()
        {
        }

        public override event EventHandler<double> onSpeed;
        public override event EventHandler<int> onRPM;
        public override event EventHandler<int> onHeartrate;
        public override event EventHandler<double> onDistance;
        public override event EventHandler<int> onElapsedTime;






        public override void OnHeartBeatReceived(object sender, byte[] data)
        {
            throw new NotImplementedException();
        }

        public override void OnBikeReceived(object sender, byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}

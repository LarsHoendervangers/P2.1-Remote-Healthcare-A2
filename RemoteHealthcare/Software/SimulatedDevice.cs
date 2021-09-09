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
        

        public override event EventHandler<double> OnSpeed;
        public override event EventHandler<int> OnRPM;
        public override event EventHandler<int> OnHeartrate;
        public override event EventHandler<double> OnDistance;
        public override event EventHandler<double> OnElapsedTime;
        public override event EventHandler<int> OnTotalPower;
        public override event EventHandler<int> OnCurrentPower;

        public SimulatedDevice()
        {
            Generator = new SimDataGenerator();

            Generator.GeneratedDistance += OnGeneratedDistance;
            Generator.GeneratedHeartrate += OnGeneratedHeartRate;
            Generator.GeneratedRPM += OnGeneratedRPM;
            Generator.GeneratedSpeed += OnGeneratedSpeed;
            Generator.GeneratedTime += OnGenerateTime;
            Generator.GeneratedCurrentPower += OnGeneratedCurrentPower;
            Generator.GeneratedTotalPower += OnGeneratedTotalPower;

         
        }

        /// <summary>
        /// Mulitple methods which get updated when data from the
        /// bike or the heart rate monitor has been received
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGeneratedTotalPower(object sender, int e)
        {
            OnTotalPower?.Invoke(this, e);
        }

        private void OnGeneratedCurrentPower(object sender, int e)
        {
            OnCurrentPower?.Invoke(this, e);
        }

        private void OnGeneratedSpeed(object sender, double e)
        {
            OnSpeed?.Invoke(this, e);
        }

        private void OnGenerateTime(object sender, double e)
        {
            OnElapsedTime?.Invoke(this, e);
        }

        private void OnGeneratedRPM(object sender, int e)
        {
            OnRPM?.Invoke(this, e);
        }

        private void OnGeneratedHeartRate(object sender, int e)
        {
            OnHeartrate?.Invoke(this, e);
        }

        private void OnGeneratedDistance(object sender, double e)
        {
            OnDistance?.Invoke(this, e);
        }
        public override void OnResistanceCall(object sender, int data)
        {
            throw new NotImplementedException();
        }
    }
}

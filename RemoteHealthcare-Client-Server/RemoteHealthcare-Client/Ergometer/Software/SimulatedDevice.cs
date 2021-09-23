using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RemoteHealthcare.Debug;


namespace RemoteHealthcare_Client.Ergometer.Software
{
    class SimulatedDevice : Device
    {
        //The generator that makes the data.
        private SimDataGenerator Generator { get; set; }

        //The eventhandelers that send it to the gui
        public override event EventHandler<double> OnSpeed;
        public override event EventHandler<int> OnRPM;
        public override event EventHandler<int> OnHeartrate;
        public override event EventHandler<double> OnDistance;
        public override event EventHandler<double> OnElapsedTime;
        public override event EventHandler<int> OnTotalPower;
        public override event EventHandler<int> OnCurrentPower;


        /// <summary>
        /// Constructor that calls the simdatagenerator constuctor without parameters,
        /// it also connects the eventhandelers to it's functions.
        /// 
        /// </summary>
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
        /// Constructor that calls the simdatagenerator constuctor with parameters,
        /// it also connects the eventhandelers to it's functions.
        /// </summary>
        /// <param name="speedRange"></param>
        /// <param name="rpmRange"></param>
        /// <param name="hearbeatRange"></param>
        /// <param name="powerRange"></param>
        public SimulatedDevice(double[] speedRange, int[] rpmRange, int[] hearbeatRange, int[] powerRange)
        {
            Generator = new SimDataGenerator(speedRange, rpmRange, hearbeatRange, powerRange);

            Generator.GeneratedDistance += OnGeneratedDistance;
            Generator.GeneratedHeartrate += OnGeneratedHeartRate;
            Generator.GeneratedRPM += OnGeneratedRPM;
            Generator.GeneratedSpeed += OnGeneratedSpeed;
            Generator.GeneratedTime += OnGenerateTime;
            Generator.GeneratedCurrentPower += OnGeneratedCurrentPower;
            Generator.GeneratedTotalPower += OnGeneratedTotalPower;
        }

        /*
         * Mulitple methods which get updated when data from the
         * bike or the heart rate monitor has been received.
        */
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

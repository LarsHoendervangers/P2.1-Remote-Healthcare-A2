using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace RemoteHealthcare_Client.Ergometer.Software
{
    public class SimDataGenerator
    {
        //Variables.
        private double speed;
        private int RPM;
        private int heartRate;
        private double distance = 0;
        private double elapsedTime = 0;
        private bool running;
        private Random random;
        private int powerlevel;
        private int totalpower;

        //Ranges for the variables
        private double[] speedRange;
        private int[] rpmRange;
        private int[] heartrateRange;
        private int[] powerlevelRange;

        //Event handlers
        public event EventHandler<double> GeneratedSpeed;
        public event EventHandler<int> GeneratedRPM;
        public event EventHandler<int> GeneratedHeartrate;
        public event EventHandler<double> GeneratedDistance;
        public event EventHandler<double> GeneratedTime;
        public event EventHandler<int> GeneratedCurrentPower;
        public event EventHandler<int> GeneratedTotalPower;

        /// <summary>
        /// This is the constuctor SimDataGenetor with no need for ranges.
        /// 
        /// This constructor sets the ranges and starts a seperate thread that will send events,
        /// so it emulates the real devices it's replacing.
        /// </summary>
        public SimDataGenerator()
        {
            //Variables for working
            running = true;
            random = new Random();

            //Setting up ranges.
            speedRange = new double[] { 0, 50 };
            rpmRange = new int[] { 0, 120 };
            heartrateRange = new int[] { 50, 200 };
            powerlevelRange = new int[] { 150, 350 };

            new Thread(() =>
            {
                //Needs signaling but it also works with a wait i guesss
                //TODO Maybe fix with signaling because it can give some potential issues.
                Thread.Sleep(3000);
                Simulation();
            }).Start();
        }

        /// <summary>
        ///  This is the constuctor SimDataGenetor with parameters for ranges.
        ///  
        /// <param name="speedParam"></param>
        /// <param name="rpmParam"></param>
        /// <param name="heartrateParam"></param>
        /// <param name="powerParam"></param>
        /// 
        /// This constructor sets the ranges and starts a seperate thread that will send events,
        /// so it emulates the real devices it's replacing.
        /// <summery>
        public SimDataGenerator(double[] speedParam, int[] rpmParam, int[] heartrateParam, int[] powerParam)
        {
            //Variables for working
            running = true;
            random = new Random();

            new Thread(() =>
            {
                //Needs signaling but it also works with a wait i guesss
                //TODO Maybe fix with signaling because it can give some potential issues.
                Thread.Sleep(3000);
                Simulation();
            }).Start();
        }

        /// <summary>
        /// This the simulation that sends data to the simulation device.
        /// It does this with Simplex Noise generated values each second.
        /// 
        /// </summary>
        private void Simulation()
        {
            //Random starting point in noise space.
            int randomSpeedSeed = (int)(random.NextDouble() * 100000);
            int randomHeartSeed = (int)(random.NextDouble() * 100000);

            //Loop for generating each value each second
            while (running)
            {
                elapsedTime += 1;
                GeneratedTime?.Invoke(this, elapsedTime);

                speed = speedRange[0] + SimplexNoiseGenerator(randomSpeedSeed, 0.01f) * speedRange[1];
                GeneratedSpeed?.Invoke(this, speed);

                RPM = rpmRange[0] + (int)(SimplexNoiseGenerator(randomSpeedSeed, 0.01f) * rpmRange[1]);
                GeneratedRPM?.Invoke(this, RPM);

                heartRate = (int)(heartrateRange[0] + SimplexNoiseGenerator(randomHeartSeed, 0.1f) * heartrateRange[1]);
                GeneratedHeartrate?.Invoke(this, heartRate);

                distance += speed / 3.6;
                GeneratedDistance?.Invoke(this, distance);

                powerlevel = (int)(powerlevelRange[0] + SimplexNoiseGenerator(randomSpeedSeed, 0.01f) * powerlevelRange[1]);
                GeneratedCurrentPower?.Invoke(this, powerlevel);

                totalpower = totalpower + powerlevel;
                GeneratedTotalPower?.Invoke(this, totalpower);

                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// This the simplex noise generator that use the library from WardBenjamin
        /// It calls the function CalcPixel1D for a value and it needs the parameters scale and x for it,
        /// the x is generated as a seed + the time passed.
        /// 
        /// </summary>
        /// <param name="startingpoint"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        private float SimplexNoiseGenerator(int startingpoint, float scale)
        {
            float output = SimplexNoise.Noise.CalcPixel1D(startingpoint + (int)elapsedTime, scale);
            return output / 255;
        }
    }
}
using System;
using System.Threading;

namespace RemoteHealthcare.Software
{
    internal class SimDataGenerator
    {

        private double speed;
        private int RPM;
        private int heartRate;
        private double distance = 0;
        private double elapsedTime = 0;
        private bool running;
        private Random random;

        public SimDataGenerator()
        {
            this.running = true;
            this.random = new Random();
            Start();
        }

        private void Start()
        {
            while (this.running)
            {
                this.speed = this.random.NextDouble() * 40;
                this.RPM = (int)(this.random.NextDouble() * 120);
                this.heartRate = (int)(50 + this.random.NextDouble() * 200);
                this.distance += this.speed / 3.6;
                this.elapsedTime += 1;

                Thread.Sleep(1000);
            }
        }
    }
}
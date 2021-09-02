using System;

namespace RemoteHealthcare.Simulator
{
    class RandomNoise
    {
        private Random random = new Random();
        private int lastRandomInt = 0;

        private int min;
        private int max;
        private int maxDeviation;

        public RandomNoise(int min, int max, int maxDeviation)
        {
            this.min = min;
            this.max = max;
            this.maxDeviation = maxDeviation;
            this.lastRandomInt = this.random.Next(this.min, this.max);
        }

        // Calculate a new random integer within a certain boundry from the previous value.
        public int Next()
        {
            this.lastRandomInt += this.random.Next(-this.maxDeviation, this.maxDeviation * 2);

            if (this.lastRandomInt < this.min)
            {
                this.lastRandomInt = this.min;
            }

            if (this.lastRandomInt > this.max)
            {
                this.lastRandomInt = this.max;
            }

            return this.lastRandomInt;
        }
    }
}

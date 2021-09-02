using System;
using System.Threading;
using RemoteHealthcare.UI.Interfaces;

namespace RemoteHealthcare.Simulator
{
    class DataSimulator
    {
        private IHeartbeatListener heartBeatListener;
        private ISpeedListener speedListener;
        private IRPMListener rpmListener;
        private IResistanceListener resistanceListener;
        private IDistanceListener distanceListener;

        private Random random = new Random();
        private RandomNoise randomHeart = new RandomNoise(70, 250, 2);
        private RandomNoise randomSpeed = new RandomNoise(0, 40, 5);
        private RandomNoise randomRPM = new RandomNoise(0, 400, 5);
        private RandomNoise randomResistance = new RandomNoise(0, 100, 2);

        public DataSimulator()
        {
        }

        public void Start()
        {
            for (int i = 0; i < 50; i++)
            {
                if (this.heartBeatListener != null)
                {
                    this.heartBeatListener.OnHeartBeatChanged(this.randomHeart.Next());
                }
                
                if (this.speedListener != null)
                {
                    this.speedListener.OnSpeedChanged(this.randomSpeed.Next());
                }

                if (this.rpmListener != null)
                {
                    this.rpmListener.OnRPMChanged(this.randomRPM.Next());
                }

                if (this.resistanceListener != null)
                {
                    this.resistanceListener.OnResistanceChanged(this.randomResistance.Next());
                }

                if (this.distanceListener != null)
                {
                    this.distanceListener.OnDistanceChanged(i);
                }

                Thread.Sleep(500);
            }
        }

        public void SetHeartBeatListener(IHeartbeatListener listener)
        {
            this.heartBeatListener = listener;
        }

        public void SetSpeedListener(ISpeedListener listener)
        {
            this.speedListener = listener;
        }

        public void SetRPMListener(IRPMListener listener)
        {
            this.rpmListener = listener;
        }

        public void SetResitanceListener(IResistanceListener listener)
        {
            this.resistanceListener = listener;
        }

        public void SetDistanceListener(IDistanceListener listener)
        {
            this.distanceListener = listener;
        }
    }
}

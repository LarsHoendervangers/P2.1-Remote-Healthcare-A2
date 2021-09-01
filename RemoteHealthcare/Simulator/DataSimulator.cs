using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RemoteHealthcare.UI.Interfaces;

namespace RemoteHealthcare.Simulator
{
    class DataSimulator
    {
        private IHeartbeatListener heartBeatListener;
        private ISpeedListener speedListener;
        private IRPMListener rpmListener;

        private Random random = new Random();

        public DataSimulator()
        {

        }

        public void Start()
        {
            for (int i = 0; i < 50; i++)
            {
                if (this.heartBeatListener != null)
                {
                    this.heartBeatListener.OnHeartBeatChanged(this.random.Next(200));
                }
                
                if (this.speedListener != null)
                {
                    this.speedListener.OnSpeedChanged(this.random.Next(60));
                }

                if (this.rpmListener != null)
                {
                    this.rpmListener.OnRPMChanged(this.random.Next(1000));
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
    }
}

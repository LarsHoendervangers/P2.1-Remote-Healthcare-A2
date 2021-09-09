using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RemoteHealthcare.Debug
{

    public class SimDataGenerator
    {
        private double speed;
        private int RPM;
        private int heartRate;
        private double distance = 0;
        private double elapsedTime = 0;
        private bool running;
        private Random random;

        //Event handlers
        public event EventHandler<double> GeneratedSpeed;
        public event EventHandler<int> GeneratedRPM;
        public event EventHandler<int> GeneratedHeartrate;
        public event EventHandler<double> GeneratedDistance;
        public event EventHandler<double> GeneratedTime;

        private object _lock = new object();

        public SimDataGenerator()
        {
            this.running = true;
            this.random = new Random();

            new Thread(() =>
            {
                Simulation();
            }).Start();
            
        }

        private void Simulation()
        {
            while (this.running)
            {
                lock(_lock){
                    this.speed = this.random.NextDouble() * 40;
                    GeneratedSpeed?.Invoke(this, speed);



                    this.RPM = (int)(this.random.NextDouble() * 120);
                  
                        GeneratedRPM?.Invoke(this, RPM);
                    

                    this.heartRate = (int)(50 + this.random.NextDouble() * 200);
                  
                        GeneratedHeartrate?.Invoke(this, heartRate);
                    

                    this.distance += this.speed / 3.6;
                
                        GeneratedDistance?.Invoke(this, distance);
                    

                    this.elapsedTime += 1;

                        GeneratedTime?.Invoke(this, elapsedTime);
                    
                }

                Thread.Sleep(1000);
            }
        }

    }



      

    
    
}
    

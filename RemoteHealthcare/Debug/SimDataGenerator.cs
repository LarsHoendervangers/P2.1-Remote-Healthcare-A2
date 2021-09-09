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
        private int powerlevel;
        private int totalpower;

        //Event handlers
        public event EventHandler<double> GeneratedSpeed;
        public event EventHandler<int> GeneratedRPM;
        public event EventHandler<int> GeneratedHeartrate;
        public event EventHandler<double> GeneratedDistance;
        public event EventHandler<double> GeneratedTime;
        public event EventHandler<int> GeneratedCurrentPower;
        public event EventHandler<int> GeneratedTotalPower;

        private object _lock = new object();

        public SimDataGenerator()
        {
            this.running = true;
            this.random = new Random();

            new Thread(() =>
            {
                //Warning: Needed for the console to run the correct way
                Thread.Sleep(3000);


                Simulation();
             
            }).Start();
            
        }

        /// <summary>
        /// Method which updates the text in the GUI and waits for a second
        /// after checking all the values
        /// </summary>
        private void Simulation()
        {

            while (this.running)
            {
              
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


                this.powerlevel = (int)(150 + this.random.NextDouble() * 200);
                GeneratedCurrentPower?.Invoke(this, powerlevel);

                this.totalpower = totalpower + powerlevel;
                GeneratedTotalPower?.Invoke(this, totalpower);




                Thread.Sleep(1000);
            }
        }

    }



      

    
    
}
    

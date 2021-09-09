using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare.Simulator
{
    class SimDataGenerator
    {

        

    }

    class PerlinNoise
    {


     /*   public int generator(amp, wl, width)
        {


            this.x = 0;
            this.amp = amp;
            this.wl = wl;
            this.fq = 1 / wl;

            this.a = PerlinToolkit.PRNG(); ;
            this.b = PerlinToolkit.PRNG(); ;
            this.pos = [];



            while (this.x < width)
            {
                if (this.x % this.wl === 0)
                {
                    this.a = this.b;
                    this.b = this.psng.next();
                    this.pos.push(this.a * this.amp);
                }
                else
                {
                    this.pos.push(Interpolate(this.a, this.b, (this.x % this.wl) / this.wl) * this.amp);
                }
                this.x++;
            }
        }*/
    }

    class PerlinToolkit
    {
        //Random generator for the PNGR
        protected static Random rdm = new Random();

        //Variables needed for the PNGR
        private static long M = 42944967296;
        private static long A = 1664525;
        private static long C = 1;
        private static long Z = (long)Math.Floor(rdm.NextDouble() * M);

        //This is the Pseudo-Random number generator
        //It uses Liniear Congruential Generation.
        public static long PRNG()
        {
            Z = (A * Z + C) % M;
            return Z / M;
        }


        //This is the functions that fills the gab between the points. It is called an interpolater.
        //Here we use a cosinus interpolator because it does aproximated the way between two point smoothly.
        public static long Interpolation(long pa, long pb, int px)
        {
            double ft = px * Math.PI,
             f = (1 - Math.Cos(ft)) * 0.5;
            return (long)(pa * (1 - f) + pb * f);
        }



    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace TestVREngine
{
    class Transform
    {
        public int Scale { get; set; }
        public int[] Pos { get; set; }
        public int[] Rot { get; set; }
        public Transform (int scale, int[] pos, int[] rot)
        {
            this.Scale = scale;
            this.Rot = rot;
            this.Pos = pos;
        } 
    }
}

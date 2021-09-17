using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace TestVREngine.Util.Structs
{
    public struct Transform
    {
        public int[] position { get; set; }
        public int scale { get; set; }
        public int[] rotation { get; set; }
        public Transform(int scale, int[] pos, int[] rot)
        {
            rotation = rot;
            this.scale = scale;
            position = pos;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare.ClientVREngine.Util.Structs
{
    /// <summary>
    /// Struct used to contain a PositionVector. This is needed to make an array of these PosVectors.
    /// </summary>
    public struct PosVector
    {
        //Attributes NOT capitalized, to correstpond with server commands
        public int[] pos { get; set; }
        public int[] dir { get; set; }

        public PosVector(int[] position, int[] direction)
        {
            pos = position;
            dir = direction;
        }
    }
}

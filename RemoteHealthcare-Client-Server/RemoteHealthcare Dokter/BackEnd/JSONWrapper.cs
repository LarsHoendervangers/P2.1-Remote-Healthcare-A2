using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Dokter.BackEnd
{
    class JSONWrapper
    {
        public static object WrapHeader(string command, object data)
        {
            return new
            {
                command,
                data = new
                {
                    data
                }
            };
        }

        public static object WrapResistance(int id, int resistance)
        {
            return new
            {
                id,
                resistance
            };
        }

        public static object WrapPatient(int id)
        {
            return new
            {
                id
            };
        }

        public static object WrapMessage(int id, string message)
        {
            return new
            {
                id,
                message
            };
        }

        public static object WrapSession(int id, bool state)
        {
            return new
            {
                id,
                state
            };
        }
    }
}

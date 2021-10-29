using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Dokter.BackEnd
{
    class JSONWrapper
    {
        /// <summary>
        /// Method which returns an object with a command and corresponding data
        /// </summary>
        /// <param name="command"></param>
        /// <param name="data"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method which returns an object with an ID and the resistance
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resistance"></param>
        /// <returns></returns>
        public static object WrapResistance(int id, int resistance)
        {
            return new
            {
                id,
                resistance
            };
        }

        /// <summary>
        /// Method which returns an object with an ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static object WrapPatient(int id)
        {
            return new
            {
                id
            };
        }

        /// <summary>
        /// Method which returns an object with an ID and a message
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static object WrapMessage(int id, string message)
        {
            return new
            {
                id,
                message
            };
        }

        /// <summary>
        /// Method which returns an object with an ID and a boolean which states if the session
        /// should be started or ended
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
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

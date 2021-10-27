using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Server.Data.Processing
{
    public class HashProcessing
    {
        private static SHA256 shaM = new SHA256Managed();

        public static string HashString(string data)
        {
            return BitConverter.ToString(shaM.ComputeHash(Encoding.UTF32.GetBytes(data)));
        } 
    }
}

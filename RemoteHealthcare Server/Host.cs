using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteHealthcare_Server
{
    public class Host
    {
        public string ID
        {
            get => default;
            set
            {
            }
        }

        public Patient ClientPatient
        {
            get => default;
            set
            {
            }
        }

        public FileProcessing Database
        {
            get => default;
            set
            {
            }
        }

        public JSONReader JSONReader
        {
            get => default;
            set
            {
            }
        }

        public void ReadData()
        {
            throw new System.NotImplementedException();
        }
    }
}
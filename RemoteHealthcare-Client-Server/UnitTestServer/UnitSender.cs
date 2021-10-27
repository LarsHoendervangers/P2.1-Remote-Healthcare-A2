using RemoteHealthcare_Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestServer 
{
    class UnitSender : ISender
    {
        public event EventHandler<bool> CheckCallback;
        public string ExpectedOutput { get; set; }

        public UnitSender(string expectedOutput)
        {
            ExpectedOutput = expectedOutput;
        }

        public string ReadMessage()
        {
            throw new NotImplementedException();
        }

        public void SendMessage(string message)
        {
            if (ExpectedOutput == message)
                CheckCallback?.Invoke(this, true);
            else
                CheckCallback?.Invoke(this, false);
        }
    }
}

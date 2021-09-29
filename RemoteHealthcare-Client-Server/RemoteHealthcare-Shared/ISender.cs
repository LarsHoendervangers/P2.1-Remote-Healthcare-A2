using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteHealthcare_Shared
{
    public interface ISender
    {
        string ReadMessage();
        void SendMessage(string message);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteHealthcare_Shared
{
    interface ISender
    {
        string ReadMessage();
        void SendMessage(string message);
    }
}

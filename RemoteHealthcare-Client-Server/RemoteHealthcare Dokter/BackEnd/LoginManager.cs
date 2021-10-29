using Newtonsoft.Json.Linq;
using RemoteHealthcare_Client;
using RemoteHealthcare_Dokter.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Dokter.BackEnd
{
    class LoginManager : DataManager
    {
        public event EventHandler<bool> OnLoginResponseReceived;

        public override void ReceivedData(JObject data)
        {
            HandleIncoming(data);
        }

        /// <summary>
        /// Method which checks whether a command is valid and calls the right method belong to the command
        /// </summary>
        /// <param name="data"></param>
        private void HandleIncoming(JObject data)
        {
            JToken value;

            bool correctCommand = data.TryGetValue("command", StringComparison.InvariantCulture, out value);

            if (!correctCommand)
            {
                // todo, log error and handle correctly
                return;
            }

            // Looking at the command and switching what behaviour is required
            if (value.ToString() == "message")
                HandleLoginResponse(data);
        }

        /// <summary>
        /// Method which sends an object to all the managers to Log in to the server
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void SendLogin(string username, string password)
        {
            object o = new
            {
                command = "login",
                data = new
                {
                    us = username,
                    pass = password,
                    flag = 1
                }
            };

            SendToManagers(JObject.FromObject(o));
        }

        /// <summary>
        /// Method which checks wheter the command that was returned from the server is valid and
        /// notifies the OnLoginResponseReceived that there is an response from the server
        /// </summary>
        /// <param name="data"></param>
        private void HandleLoginResponse(JObject data)
        {
            JToken flag;
            int flagValue;

            bool canParse = data.TryGetValue("flag", StringComparison.CurrentCulture, out flag);

            if (!canParse) return;

            if (!int.TryParse(flag.ToString(), out flagValue)) return;

            if (flagValue == 1) 
                this.OnLoginResponseReceived?.Invoke(this, data.GetValue("data").ToString().Contains("succesfull connect"));
        }
    }
}

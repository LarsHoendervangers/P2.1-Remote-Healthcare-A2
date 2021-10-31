using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RemoteHealthcare_Client.TCP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace RemoteHealthcare_Client
{
    /// <summary>
    /// Extention from DataManager,
    /// this class handles the connection to the server and the communication from the server
    /// </summary>
    public class ServerDataManager : DataManager
    {
        // The TCPHandler that handles the 'talking' to the server
        private TCPClientHandler TCPClientHandler { get; set; }

        // Event that is involked when the server anwsers a login request
        public event EventHandler<bool> OnLoginResponseReceived;

        /// <summary>
        /// Constructor for the ServerDataManager
        /// It starts the connection to the server
        /// </summary>
        /// <param name="ip">The IP to connect to</param>
        /// <param name="port">The port number to connect to</param>
        public ServerDataManager(string ip, int port)
        {
            // Creating the TCPClientHandler
            this.TCPClientHandler = new TCPClientHandler(ip, port, true);
            this.TCPClientHandler.SetRunning(true);

            // Setting the OnMessageReceived command to be called when the TCP handler receives a message
            this.TCPClientHandler.OnMessageReceived += OnMessageReceived;
        }

        /// <summary>
        /// This method wil handle the event that is called when this.TCPClientHander receives a message from the server
        /// The method transforms the string message to a JObject if it can, otherwise it ignores the message 
        /// </summary>
        /// <param name="sender">The sender that called the event</param>
        /// <param name="message">The message that was received from the server</param>
        private void OnMessageReceived(object sender, string message)
        {
            //Reading input, null if the parse failed
            JObject jobject = JsonConvert.DeserializeObject(message) as JObject;

            // Checking if the object could be parsed
            if (jobject != null) HandleIncoming(jobject);

            else
            {
                Debug.WriteLine("JObject is null");
                this.TCPClientHandler.SetRunning(false);
            }
        }

        /// <summary>
        /// Method handles the JObjects that are received from the server
        /// It checks the command line of the message, if the command is 'message' then we handles that command
        /// else the command is send to all other handlers
        /// </summary>
        /// <param name="jobject"></param>
        private void HandleIncoming(JObject jobject)
        {
            // command value always gives the action 
            JToken value;

            bool correctCommand = jobject.TryGetValue("command", StringComparison.InvariantCulture, out value);

            // Checking if the command value could be read
            if (!correctCommand) return;

            // Looking at the command and switching what behaviour is required
            switch(value.ToString())
            {
                case "message":
                    // Calling the method to hanled a message command
                    HandleMessageCommand(jobject);
                    break;
                default:
                    // DataManager does not need the command, sending to all others
                    this.SendToManagers(jobject);
                    break;
            }
        }

        /// <summary>
        /// Handles the Message command that the server can send.
        /// flag 1: This is a login message, call the event to handle the login
        /// flag 2: The message needs to be shown in VR, send to the manager
        /// All other flags are ignored
        /// </summary>
        /// <param name="jobject">The object that holds the data of the message command</param>
        private void HandleMessageCommand(JObject jobject)
        {
            //TODO try get value instead of getvalue
            // all message object are required to have flag attribute.
            int flag = (int)jobject.GetValue("flag");

            // Printing the message to the debug file
            Debug.WriteLine($"Message from server: {jobject.GetValue("data")}, with flag: {flag}");
            switch (flag)
            {
                case 1:
                    this.OnLoginResponseReceived?.Invoke(this, jobject.GetValue("data").ToString().Contains("succesfull connect"));
                    break;
                case 2:
                    // Sending the data to the vrmanager, since flag 2 needs to be show in vr
                    this.SendToManagers(jobject);
                    break;
                case 3:
                default:
                    Trace.WriteLine($"Error received from server{jobject.GetValue("data")}");
                    break;
            }
        }

        /// <summary>
        /// This method is called when other managers broadcast a message.
        /// It sends the command to the server
        /// </summary>
        /// <param name="data">The data to be send to the server</param>
        public override void ReceivedData(JObject data)
        {
            // Calling the TCPClient to send the message to the server
            this.TCPClientHandler.WriteMessage(data.ToString());
        }

        /// <summary>
        /// Method wil try to reconnect to the server, this method can be called if the connection failed during use
        /// </summary>
        /// <param name="ip">The IP to connect to</param>
        /// <param name="port">The port number to connection to</param>
        public void ReconnectWithServer(string ip, int port)
        {
            // Stop reading from the server
            this.TCPClientHandler.SetRunning(false);

            // Create a new object to restart the connection to the server
            this.TCPClientHandler = new TCPClientHandler(ip, port, true);
            this.TCPClientHandler.OnMessageReceived += OnMessageReceived;

            // Start the new TCPhandler
            this.TCPClientHandler.SetRunning(true);
        }

        // Getter for the networstream the TCPClient handler uses
        public NetworkStream GetStream()
        {
            return this.TCPClientHandler.stream;
        }
    }
}
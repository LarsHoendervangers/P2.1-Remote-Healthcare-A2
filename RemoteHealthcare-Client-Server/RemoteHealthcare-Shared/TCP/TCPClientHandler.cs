using CommClass;
using RemoteHealthcare_Shared;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;

namespace RemoteHealthcare_Client.TCP
{
    /// <summary>
    /// Class that handles the TCP connection between a other program, taking ip- and portadress
    /// </summary>
    class TCPClientHandler
    {
        public event EventHandler<string> OnMessageReceived;
        private bool running = false;
        public readonly NetworkStream stream;
        private ISender Sender;

        /// <summary>
        /// Constructor for TCPClientHandler
        /// </summary>
        public TCPClientHandler(string ip, int port, bool useEncryption)
        {
            try
            {
                TcpClient client = new TcpClient(ip, port);
                stream = client.GetStream();

                if (useEncryption) this.Sender = new EncryptedClient(stream);
                else this.Sender = new PlaneTextSender(stream);
            }
            catch (Exception e)
            {
                Trace.WriteLine("Error: " + e.Message);
            }
        }

        /// <summary>
        /// Starts a thread with a loop that receives all the data en envokes it up.
        /// </summary>
        private void HandleIncoming()
        {
            // Starting the reading loop in new thread
            new Thread(
                () =>
                {
                    running = true;

                    while (running)
                    {
                        // Call the event with the message received
                        if (stream != null)
                        {
                            string message = this.Sender?.ReadMessage();
                            OnMessageReceived.Invoke(this, message);
                        } else
                        {
                            break;
                        }
                    }

                    // Shutting down
                    stream?.Close();
                    Debug.WriteLine("Stopped read thread");
                }).Start();
        }

        public string ReadMessage()
        {
            return this.Sender?.ReadMessage();
        }

        /// <summary>
        /// Writes the message as a string as input.
        /// </summary>
        /// <param name="message">the message that is send to the server</param>
        public void WriteMessage(string message)
        {
            this.Sender?.SendMessage(message);
        }

        /// <summary>
        /// Sets the read function on or of
        /// </summary>
        /// <param name="state">boolean to set the state to</param>
        public void SetRunning(bool state)
        {
            if (state)
                // When running tis set true we start the loop for incoming messages
                HandleIncoming();
            else
                Debug.WriteLine("Disabling read");
                running = false;
        }
    }
}

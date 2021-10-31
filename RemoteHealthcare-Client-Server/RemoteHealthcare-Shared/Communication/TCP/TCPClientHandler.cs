using CommClass;
using RemoteHealthcare_Shared;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using System.Windows;

namespace RemoteHealthcare_Client.TCP
{
    /// <summary>
    /// Class that handles the TCP connection between another program.
    /// </summary>
    class TCPClientHandler
    {
        public event EventHandler<string> OnMessageReceived;
        private bool running = false;
        public readonly NetworkStream stream;
        private ISender Sender;

        /// <summary>
        /// Constructor of the TCPClientHandler class.
        /// </summary>
        /// <param name="ip">The IP of the other program you want to connect with.</param>
        /// <param name="port">Port of the other program you want to connect with.</param>
        /// <param name="useEncryption">Decides whether the connection should be encrypted.</param>
        public TCPClientHandler(string ip, int port, bool useEncryption)
        {
            try
            {
                //TcpClient client = new TcpClient(ip, port);

                TcpClient client = new TcpClient();
                if (!client.ConnectAsync(ip, port).Wait(5000))
                {
                    throw new Exception("Failed to connect with server.");
                }

                stream = client.GetStream();

                if (useEncryption) this.Sender = new EncryptedClient(stream);
                else this.Sender = new PlaneTextSender(stream);
            }
            catch (Exception e)
            {
                MessageBox.Show("Er kan geen verbinding worden gemaakt met de server!", "RemoteHealthcare", MessageBoxButton.OK, MessageBoxImage.Error);
                Debug.WriteLine("Error: " + e.Message);
            }
        }

        /// <summary>
        /// Starts a thread with a loop that receives all the data and then invokes the MessageReceived event.
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

        /// <summary>
        /// Can be used to read the stream only once.
        /// </summary>
        /// <returns></returns>
        public string ReadMessage()
        {
            try
            {
                return this.Sender?.ReadMessage();
            } catch (Exception e)
            {
                Debug.WriteLine("TCPClientHandler: " + e.Message);
                return "";
            }
        }

        /// <summary>
        /// Writes a string to the stream.
        /// </summary>
        /// <param name="message">Message that will be send to the other program.</param>
        public void WriteMessage(string message)
        {
            this.Sender?.SendMessage(message);
        }

        /// <summary>
        /// Sets the state of the read loop. You should only set it to true once per instance!
        /// </summary>
        /// <param name="state">State of the read loop.
        /// True: Starts the reading loop.
        /// False: Stops the reading loop.</param>
        public void SetRunning(bool state)
        {
            if (state)
                HandleIncoming();
            else
                Debug.WriteLine("Disabling read");
                running = false;
        }
    }
}

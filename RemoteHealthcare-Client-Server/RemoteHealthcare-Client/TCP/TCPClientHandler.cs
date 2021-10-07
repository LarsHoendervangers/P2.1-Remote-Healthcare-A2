using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RemoteHealthcare_Client.TCP
{
    /// <summary>
    /// Class that handles the TCP connection between a other program, taking ip- and portadress
    /// </summary>
    class TCPClientHandler
    {
        public event EventHandler<string> OnMessageReceived;
        private bool running = false;
        private readonly NetworkStream stream;

        /// <summary>
        /// Constructor for TCPClientHandler
        /// </summary>
        public TCPClientHandler(string ip, int port)
        {
            try
            {
                TcpClient client = new TcpClient(ip, port);
                stream = client.GetStream();

                Trace.WriteLine("connected to server BOOBIES");
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
                            string message = ReadMessage();
                            OnMessageReceived.Invoke(this, message);
                        }
                        
                    }

                    // Shutting down
                    stream?.Close();

                }).Start();
        }

        /// <summary>
        /// Writes the message as a string as input.
        /// </summary>
        /// <param name="message">the message that is send to the server</param>
        public void WriteMessage(string message)
        {
            //Console.WriteLine(message);
            byte[] payload = Encoding.ASCII.GetBytes(message);
            byte[] length = new byte[4];
            length = BitConverter.GetBytes(message.Length);
            byte[] final = Combine(length, payload);

            //Debug print of data that is send
            //Console.WriteLine(BitConverter.ToString(final));
            stream?.Write(final, 0, message.Length + 4);
            stream?.Flush();
        }

        /// <summary>
        /// Reads a message from the TCP connection
        /// </summary>
        /// <returns>The message as a string</returns> 
        public string ReadMessage()
        {
            // 4 bytes leng == 32 bits, always positive unsigned
            byte[] lengthArray = new byte[4];

            //Trying to solve a nullpointer here
            stream?.Read(lengthArray, 0, 4);
            int length = BitConverter.ToInt32(lengthArray, 0);

            if (length <= 0)
            {
                Debug.WriteLine("TCPClientHandler.ReadMessage: we don't have a networkstream");
                return "";
            }

            byte[] buffer = new byte[length];
            int totalRead = 0;

            //read bytes until stream indicates there are no more
            while (totalRead < length)
            {
                int read = stream.Read(buffer, totalRead, buffer.Length - totalRead);
                totalRead += read;
                //Console.WriteLine("ReadMessage: " + read);
            }

            

            return Encoding.ASCII.GetString(buffer, 0, totalRead);
        }

        /// <summary>
        /// Combines two byte[] together
        /// </summary>
        /// <param name="first"></param> first byte[]
        /// <param name="second"></param> second byte[]
        /// <returns>The byte array with both bytes added together</returns>
        private static byte[] Combine(byte[] first, byte[] second)
        {
            byte[] bytes = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, bytes, 0, first.Length);
            Buffer.BlockCopy(second, 0, bytes, first.Length, second.Length);
            return bytes;
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
                running = false;
        }

    }
}

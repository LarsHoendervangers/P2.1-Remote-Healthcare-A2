using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestVREngine.Tunnel.TCP
{
    class TCPClientHandler
    {
        public event EventHandler<string> OnMessageReceived;
        private bool running = false;
        private readonly NetworkStream stream;

        public TCPClientHandler()
        {
            TcpClient client = new TcpClient("145.48.6.10", 6666);
            stream = client.GetStream();

        }

        /// <summary>
        /// Starts a thread with a loop that receives all the data en envokes it up.
        /// </summary>
        private void HandleIncoming()
        {
            new Thread(
                () =>
                {

                    running = true;

                    while (running)
                    {
                        string message = ReadMessage();
                        OnMessageReceived.Invoke(this, message);
                    }

                    stream.Close();

                }).Start();
        }

        /// <summary>
        /// Writes the message as a string as input.
        /// </summary>
        /// <param name="message"></param> the message that is send to the server
        public void WriteMessage(string message)
        {
            //Console.WriteLine(message);
            byte[] payload = Encoding.ASCII.GetBytes(message);
            byte[] lenght = new byte[4];
            lenght = BitConverter.GetBytes(message.Length);
            byte[] final = Combine(lenght, payload);

            //Debug print of data that is send
            //Console.WriteLine(BitConverter.ToString(final));
            stream.Write(final, 0, message.Length + 4);
            stream.Flush();
        }

        /// <summary>
        /// Reads the message once.
        /// </summary>
        /// <returns></returns> the message as a string
        public string ReadMessage()
        {
            // 4 bytes lenght == 32 bits, always positive unsigned
            byte[] lenghtArray = new byte[4];

            stream.Read(lenghtArray, 0, 4);
            int lenght = BitConverter.ToInt32(lenghtArray, 0);

            //Console.WriteLine(lenght);

            byte[] buffer = new byte[lenght];
            int totalRead = 0;

            //read bytes until stream indicates there are no more
            while (totalRead < lenght)
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
        /// <returns></returns>
        private byte[] Combine(byte[] first, byte[] second)
        {
            byte[] bytes = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, bytes, 0, first.Length);
            Buffer.BlockCopy(second, 0, bytes, first.Length, second.Length);
            return bytes;
        }

        /// <summary>
        /// Sets the read function on or of
        /// </summary>
        /// <param name="state"></param> boolean for state.
        public void SetRunning(bool state)
        {
            if (state)
                HandleIncoming();
            else
                running = false;
        }

    }
}

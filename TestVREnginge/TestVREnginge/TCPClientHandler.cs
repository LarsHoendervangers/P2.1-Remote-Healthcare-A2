using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestVREnginge.TCP
{
    class TCPClientHandler
    {
        
        public event EventHandler<string> OnMessageReceived;
        private bool running = false;
        private NetworkStream stream;

        public TCPClientHandler()
        {
            TcpClient client = new TcpClient("145.48.6.10", 6666);
            stream = client.GetStream();


            
        }

        public void startIncoming()
        {
            Thread listener = new Thread(
                () => HandleIncoming(stream));
            listener.Start();
        }

        private void HandleIncoming(NetworkStream incomingStream)
        {
            running = true;
           
            while (running)
            {
                string message = ReadMessage(incomingStream);
                this.OnMessageReceived.Invoke(this, message);
            }
        }

        public void WriteMessage(string message)
        {
            //Console.WriteLine(message);
            byte[] payload = Encoding.ASCII.GetBytes(message);
            byte[] lenght = new byte[4];
            lenght = BitConverter.GetBytes(message.Length);
            byte[] final = Combine(lenght, payload);

            //Debug print of data that is send
            //Console.WriteLine(BitConverter.ToString(final));
            this.stream.Write(final, 0, message.Length + 4);
            this.stream.Flush();
        }

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

        public byte[] Combine(byte[] first, byte[] second)
        {
            byte[] bytes = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, bytes, 0, first.Length);
            Buffer.BlockCopy(second, 0, bytes, first.Length, second.Length);
            return bytes;
        }

        public void close()
        {
            this.running = false;
        }

    }
}

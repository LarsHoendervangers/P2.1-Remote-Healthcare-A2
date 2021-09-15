using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using TestVREnginge.TCP;

namespace TestVREnginge
{
    class Program
    {
        static void Main(string[] args)
        {
            TCPClientHandler client = new TCPClientHandler(); 
            /*TcpClient client = new TcpClient("145.48.6.10", 6666);
            NetworkStream stream = client.GetStream();

            string startingCode = "{\r\n\"id\" : \"session/list\"\r\n}";
            Communication.WriteMessage(stream, startingCode);


            //Reading
            String jsonString = Communication.ReadMessage(stream);
            JObject jsonData = (JObject)JsonConvert.DeserializeObject(jsonString);

            List<string> availableComputers = new List<string>();
            Console.WriteLine("Available Computers: ");

            JArray computers = (JArray)jsonData.GetValue("data");
            int selection = 1;
            foreach (JObject objects in computers)
            {
                string id = objects.GetValue("id").ToString();
                JObject clientinfo = (JObject)objects.GetValue("clientinfo");
                string pcName = clientinfo.GetValue("host").ToString();
                string pcUser = clientinfo.GetValue("user").ToString();
                string pcGPU = clientinfo.GetValue("renderer").ToString();

                Console.WriteLine($"Selection: {selection}, ID: {id}, PC: {pcName}, User: {pcUser}, GPU: {pcGPU}");

                
                availableComputers.Add(id);
                selection++;
            }

            int userinput = 0;
            while (userinput < 1 || userinput > availableComputers.Count)
            {
                Console.WriteLine("\nGive a selection number for a tunnel: ");
                userinput = int.Parse(Console.ReadLine());
            }

            string requestingCode = "{\r\n\"id\" : \"tunnel/create\",\r\n\"data\" :\r\n	{\r\n\"session\" : \""+ availableComputers[userinput-1]+ "\"\r\n}\r\n}";
            Communication.WriteMessage(stream, requestingCode);


            Console.WriteLine(Communication.ReadMessage(stream));

            //Shutting down
            stream.Close();
            client.Close();*/
        }




    }
}

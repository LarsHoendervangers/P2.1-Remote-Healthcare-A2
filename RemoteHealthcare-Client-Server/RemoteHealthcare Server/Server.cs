using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;

namespace RemoteHealthcare_Server
{
    public class Server
    {
        private MainWindow window;

        public IPAddress Ip { get; set; }
        public int Port { get; set; }

        /// <summary>
        /// List of all the clients connected to the server
        /// </summary>
        public List<Host> Clients { get; set; }

        public Server(MainWindow window, IPAddress ip, int port)
        {
            this.window = window;
            this.Ip = ip;
            this.Port = port;
        }

        /// <summary>
        /// Metod which starts the server
        /// </summary>
        public void StartServer()
        {
            PrintToGUI($"Server started on {this.Ip}:{this.Port}.");
        }

        /// <summary>
        /// Method which end the server
        /// </summary>
        public void StopServer()
        {
            PrintToGUI("Server stopped.");
        }

        /// <summary>
        /// Method which is fired when client is connected
        /// </summary>
        public void OnConnect()
        {
            PrintToGUI("Someone connected.");
        }

        /// <summary>
        /// Method which is fired when a client disconnects
        /// </summary>
        public void OnDisconnect()
        {
            PrintToGUI("Someone disconnected.");
        }

        public void PrintToGUI(string msg)
        {
            this.window.debugTextBlock.Dispatcher.Invoke(() => window.debugTextBlock.Text += ("\n" + msg));
        }
    }
}
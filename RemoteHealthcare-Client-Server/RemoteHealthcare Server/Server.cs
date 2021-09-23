using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace RemoteHealthcare_Server
{
    public class Server
    {
        private MainWindow window;

        /// <summary>
        /// The IP address that the server is running on.
        /// </summary>
        public IPAddress Ip { get; set; }
        /// <summary>
        /// The port that the server is running on.
        /// </summary>
        public int Port { get; set; }

        private TcpListener tcpListener;

        /// <summary>
        /// List of all the clients connected to the server.
        /// </summary>
        public List<Host> Hosts { get; set; }

        public Server(MainWindow window, IPAddress ip, int port)
        {
            this.window = window;
            this.Ip = ip;
            this.Port = port;
            this.Hosts = new List<Host>();
            this.tcpListener = new TcpListener(this.Ip, this.Port);
        }

        /// <summary>
        /// Method which starts the server.
        /// </summary>
        public void StartServer()
        {
            PrintToGUI($"Server started on {this.Ip}:{this.Port}.");
            this.tcpListener.Start();
            RunServer();
        }

        private void RunServer()
        {
            this.tcpListener.BeginAcceptTcpClient(new AsyncCallback(HandleClient), null);
        }

        public void HandleClient(IAsyncResult ar)
        {
            TcpClient tcpClient = this.tcpListener.EndAcceptTcpClient(ar);
            Host host = new Host("id", null, null, null, tcpClient);

            OnConnect(host);
            this.tcpListener.BeginAcceptTcpClient(new AsyncCallback(HandleClient), null);
        }

        /// <summary>
        /// Method which ends the server.
        /// </summary>
        public void StopServer()
        {
            PrintToGUI("Server stopped.");
        }

        /// <summary>
        /// Method which is fired when client is connected.
        /// </summary>
        public void OnConnect(Host host)
        {
            PrintToGUI($"{host.tcpClient.Client.RemoteEndPoint} connected.");
            this.Hosts.Add(host);
        }

        /// <summary>
        /// Method which is fired when a client disconnects.
        /// </summary>
        public void OnDisconnect(Host host)
        {
            PrintToGUI($"{host.ClientPatient.Username} disconnected.");
        }

        /// <summary>
        /// This method allows outputting to a text block on the GUI.
        /// </summary>
        public void PrintToGUI(string msg)
        {
            this.window.debugTextBlock.Dispatcher.Invoke(() => window.debugTextBlock.Text += ("\n" + msg));
        }
    }
}
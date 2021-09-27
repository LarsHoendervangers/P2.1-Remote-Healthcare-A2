using RemoteHealthcare_Server.Data;
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

        private Usermanagement users;

        private static MainWindow window;

        public IPAddress Ip { get; set; }

        public int Port { get; set; }

        private TcpListener tcpListener;

        public List<Host> Hosts { get; set; }

        public Server(MainWindow windows, IPAddress ip, int port)
        {
            window = windows;
            this.Ip = ip;
            this.Port = port;
            this.Hosts = new List<Host>();
            this.tcpListener = new TcpListener(this.Ip, this.Port);
            this.users = new Usermanagement(); 
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

        /// <summary>
        /// Method which will be called by StartServer().
        /// </summary>
        private void RunServer()
        {
            this.tcpListener.BeginAcceptTcpClient(new AsyncCallback(HandleClient), null);
        }

        /// <summary>
        /// This method will create a new Host object and call the OnConnect event.
        /// </summary>
        /// <param name="ar"></param>
        public void HandleClient(IAsyncResult ar)
        {
            try
            {
                TcpClient tcpClient = this.tcpListener.EndAcceptTcpClient(ar);
                Host host = new Host(tcpClient, users);

                OnConnect(host);
                this.tcpListener.BeginAcceptTcpClient(new AsyncCallback(HandleClient), null);
            } catch
            {
                OnDisconnect(null);
            }
        }

        /// <summary>
        /// Method which ends the server.
        /// </summary>
        public void StopServer()
        {
            for (int i = this.Hosts.Count - 1; i >= 0; i--)
            {
                if (this.Hosts.Count > i)
                {
                    Host host = this.Hosts[i];
                    OnDisconnect(host);
                } else
                {
                    break;
                }
            }
            this.tcpListener.Stop();
            PrintToGUI("Server stopped.");
        }

        /// <summary>
        /// Method which is fired when client is connected.
        /// This method will add the Host object to the list.
        /// </summary>
        /// <param name="host">Host object that will be added.</param>
        public void OnConnect(Host host)
        {
            PrintToGUI($"{host.tcpclient.Client.RemoteEndPoint} connected. ");
            this.Hosts.Add(host);
        }

        /// <summary>
        /// Method which is fired when a client disconnects. 
        /// This method will stop each connected Host object and remove them from the list.
        /// </summary>
        /// <param name="host">Host object to remove.</param>
        public void OnDisconnect(Host host)
        {
            if (host != null)
            {
                //PrintToGUI($"{host.client.Client.RemoteEndPoint} disconnected.");
                host.Stop();
                this.Hosts.Remove(host);
            }
        }

        /// <summary>
        /// This method allows outputting to a text block on the GUI.
        /// </summary>
        /// <param name="msg">Message to print on the GUI</param>
        public static void PrintToGUI(string msg)
        {
            if (window != null)
            {
                window.debugTextBlock.Dispatcher.Invoke(() => window.debugTextBlock.Text += ("\n" + msg));
            }
        }

        /// <summary>
        /// This method will send a message to each connected Host object.
        /// This will also print the message on to the GUI.
        /// </summary>
        /// <param name="msg">Message to send</param>
        public void Broadcast(string msg)
        {
            for (int i = this.Hosts.Count - 1; i >= 0; i--)
            {
                if (this.Hosts.Count > i)
                {
                    Host host = this.Hosts[i];
                    //JSONWriter.MessageWrite(msg, host.client);
                } else
                {
                    break;
                }
            }
            PrintToGUI($"Server: {msg}");
        }
    }
}
using RemoteHealthcare_Server.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using CommClass;

namespace RemoteHealthcare_Server
{
    public class Server
    {
        private UserManagement userManagement;

        private static MainWindow window;

        public IPAddress Ip { get; set; }

        public int Port { get; set; }

        private TcpListener tcpListener;

        public Server(MainWindow windows, IPAddress ip, int port)
        {
            window = windows;
            this.Ip = ip;
            this.Port = port;
            this.tcpListener = new TcpListener(this.Ip, this.Port);
            this.userManagement = new UserManagement(); 
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
                Host host = new Host(tcpClient, userManagement);

                host.Disconnecting += OnDisconnect;

                OnConnect(host);
                this.tcpListener.BeginAcceptTcpClient(new AsyncCallback(HandleClient), null);
            } catch
            {
                OnDisconnect(null);
            }
        }

        /// <summary>
        /// Method which ends the server.
        /// It also ends the active sessions and closes the connections with each user.
        /// </summary>
        public void StopServer()
        {
            for (int i = this.userManagement.activeHosts.Count - 1; i >= 0; i--)
            {
                if (this.userManagement.activeHosts.Count > i)
                {
                    Host host = this.userManagement.activeHosts[i];
                    host.FireDisconnectingEvent();
                } else
                {
                    break;
                }
            }
            this.userManagement.OnDestroy();
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
            PrintToGUI($"{host.tcpclient.Client.RemoteEndPoint} connected.");
            this.userManagement.activeHosts.Add(host);
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
                PrintToGUI($"{host.tcpclient.Client.RemoteEndPoint} disconnected.");
                this.userManagement.activeHosts.Remove(host);
                host.tcpclient.Close();
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
                try
                {
                    window.debugTextBlock.Dispatcher.Invoke(() => window.debugTextBlock.Text += ("\n" + msg));
                } catch (Exception e)
                {
                    Debug.WriteLine($"Error: {e.Message}. The server was probably closing.");
                }
            }
        }

        /// <summary>
        /// This method will send a message to each connected Host object.
        /// This will also print the message on to the GUI.
        /// </summary>
        /// <param name="msg">Message to send</param>
        public void Broadcast(string msg)
        {
            for (int i = this.userManagement.activeHosts.Count - 1; i >= 0; i--)
            {
                if (this.userManagement.activeHosts.Count > i)
                {
                    Host host = this.userManagement.activeHosts[i];
                    JSONWriter.MessageWrite(msg, host.GetSender());
                } else
                {
                    break;
                }
            }
            PrintToGUI($"Server: {msg}");
        }
    }
}
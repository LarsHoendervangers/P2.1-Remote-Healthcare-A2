using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RemoteHealthcare.ClientVREngine.Util.Structs;
using RemoteHealthcare.Ergometer.Software;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Windows;
using RemoteHealthcare_Client.ClientVREngine.Scene;
using RemoteHealthcare_Shared.Settings;

namespace RemoteHealthcare_Client
{
    /// <summary>
    /// Class handles all the setup that is needed for the client to start
    /// This includes:
    /// - Starting the connection to the server
    /// - Login with the server
    /// - Starting the connection to the Ergo device
    /// - Starting the connection to the VR-server
    /// </summary>
    public class StartupLoader
    {
        // Getting the IP and Port from the serversettings
        private string ip = ServerSettings.IP;
        private int port = ServerSettings.Port;

        private DataManager serverDataManager;
        private DataManager deviceDataManager;
        private DataManager vrDataManager;

        // Events
        public event EventHandler<List<ClientData>> OnVRConnectionsReceived;
        public event EventHandler<List<string>> OnBLEDeviceReceived;
        public event EventHandler<bool> OnLoginResponseReceived;

        /// <summary>
        /// Method is called on startup of the application
        /// - Loads all Bike bluetooth devices
        /// - Loads all available VR-servers
        /// - Start connection with the server
        /// </summary>
        public void Init()
        {
            GetAvailableVRConnections();
            GetAvailableBLEDevices();

            // Logging in debug when login is received
            this.OnLoginResponseReceived += ((s, d) =>
            {
                Debug.WriteLine("OnLoginResponseReceived fired");
            });

            // starting up the connection to the server
            this.serverDataManager = new ServerDataManager(this.ip, this.port);
        }

        /// <summary>
        /// This method handles starting the client application
        /// This is done after a device and Vr-server and scene is selected
        /// </summary>
        /// <param name="device">The device that the user selected</param>
        /// <param name="vrServerID">The Vr-server that the user selected</param>
        /// <param name="generalScene">The scene the user selected</param>
        public void Start(string device, string vrServerID, GeneralScene generalScene)
        {
            this.deviceDataManager = new DeviceDataManager(device, "Decathlon Dual HR");

            (this.vrDataManager as VRDataManager).Scene = generalScene;
            (this.vrDataManager as VRDataManager)?.Start(vrServerID);
        }

        /// <summary>
        /// Method gets all the VR-server that are running, it calls the event when the data is received
        /// </summary>
        public void GetAvailableVRConnections()
        {
            // The gui needs all the available vr servers to connect to
            // To get this list it is needed to start up the vrDataManager
            VRDataManager dataManager = new VRDataManager();
            List<ClientData> clientVREngines = dataManager.VRTunnelHandler.GetAvailableClients();
            if (clientVREngines == null) return;
            clientVREngines.Reverse();

            this.vrDataManager = dataManager;

            // Invoking the event when the data is received
            this.OnVRConnectionsReceived?.Invoke(this, clientVREngines);
        }

        /// <summary>
        /// Gets all the bluetooth devices that are available
        /// Calls the event when the deta is received
        /// </summary>
        public void GetAvailableBLEDevices()
        {
            // Gets all the bluetooth devices available
            List<string> blDevices = PhysicalDevice.ReadAllDevices();
            blDevices.Add("Simulator");
            
            // Invoking the event
            this.OnBLEDeviceReceived?.Invoke(this, blDevices);

            // Starts the refresh system to get new bl devices
            new Thread(PhysicalDevice.ReadAllDevicesTask).Start();
        }

        [Obsolete] //This was the old, ugly way of starting up
        public void SetupServerConnection(string ip, int port, string device, string vrServerID, string username, string password)
        {
            // Setting op serverDataManager, it creates the connection to the server
            this.serverDataManager = new ServerDataManager(ip, port);

            this.deviceDataManager = new DeviceDataManager(device, "Decathlon Dual HR");

            (this.vrDataManager as VRDataManager)?.Start(vrServerID);

            object o = new
            {
                command = "login",
                data = new
                {
                    us = username,
                    pass = password,
                    flag = 0
                }
            };
            this.serverDataManager.ReceivedData(JObject.FromObject(o));
        }

        /// <summary>
        /// Handles the login command between the user and the server
        /// </summary>
        /// <param name="userName">The username the user entered</param>
        /// <param name="password">The password to log in with</param>
        public void Login(string userName, string password)
        {
            // Reconnecting to the server if it failed the first time
            if ((this.serverDataManager as ServerDataManager).GetStream() == null)
            {
                Debug.WriteLine("Reconnecting");
                (this.serverDataManager as ServerDataManager).ReconnectWithServer(this.ip, this.port);
            }

            // Setting the callback event for the login
            (this.serverDataManager as ServerDataManager).OnLoginResponseReceived += (s, d) => OnLoginResponseReceived?.Invoke(this, d);

            // Building the json command to log in
            JObject loginCommand = JObject.FromObject(
                new
                {
                    command = "login",
                    data = new
                    {
                        us = userName,
                        pass = password,
                        flag = 0
                    }
                });

            // Sending the login data to the server
            this.serverDataManager?.ReceivedData(loginCommand);
        }

        [Obsolete] //This was the old, ugly way of refreshing the VR-servers
        private void UpdateVRServers()
        {
            while (true)
            {
                Application.Current.Dispatcher.Invoke(GetAvailableVRConnections);
                Thread.Sleep(5000);
            }
        }

        [Obsolete] //This was the old, ugly way of refreshing the BL devices
        private void UpdateBLEDevices()
        {
            while (true)
            {
                Application.Current.Dispatcher.Invoke(GetAvailableBLEDevices);
                Thread.Sleep(5000);
            }
        }
    }
}
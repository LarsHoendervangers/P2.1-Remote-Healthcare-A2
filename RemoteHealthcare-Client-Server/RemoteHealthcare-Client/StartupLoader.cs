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

namespace RemoteHealthcare_Client
{
    public class StartupLoader
    {
        private string ip = "127.0.0.1";
        private int port = 6969;

        private DataManager serverDataManager;
        private DataManager deviceDataManager;
        private DataManager vrDataManager;

        public event EventHandler<List<ClientData>> OnVRConnectionsReceived;
        public event EventHandler<List<string>> OnBLEDeviceReceived;
        public event EventHandler<bool> OnLoginResponseReceived;

        public void Init()
        {
            GetAvailableVRConnections();
            GetAvailableBLEDevices();

            this.OnLoginResponseReceived += ((s, d) =>
            {
                Debug.WriteLine("OnLoginResponseReceived fired");
            });

            // starting up the connection to the server
            this.serverDataManager = new ServerDataManager(this.ip, this.port);
        }

        public void Start(string device, string vrServerID, GeneralScene generalScene)
        {
            this.deviceDataManager = new DeviceDataManager(device, "Decathlon Dual HR");

            (this.vrDataManager as VRDataManager).Scene = generalScene;
            (this.vrDataManager as VRDataManager)?.Start(vrServerID);
        }

        public void GetAvailableVRConnections()
        {
            // The gui needs all the available vr servers to connect to
            // To get this list it is needed to start up the vrDataManager
            VRDataManager dataManager = new VRDataManager();
            List<ClientData> clientVREngines =  dataManager.VRTunnelHandler.GetAvailableClients();
            if (clientVREngines == null) return;
            clientVREngines.Reverse();

            this.vrDataManager = dataManager;

            this.OnVRConnectionsReceived?.Invoke(this, clientVREngines);
        }

        public void GetAvailableBLEDevices()
        {
            // Gets all the bluetooth devices available
            List<string> blDevices = PhysicalDevice.ReadAllDevices();
            blDevices.Add("Simulator");
            
            this.OnBLEDeviceReceived?.Invoke(this, blDevices);
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

        public void Login(string userName, string password)
        {
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

        private void UpdateVRServers()
        {
            while (true)
            {
                Application.Current.Dispatcher.Invoke(GetAvailableVRConnections);
                Thread.Sleep(5000);
            }

        }
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
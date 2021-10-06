using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RemoteHealthcare.ClientVREngine.Util.Structs;
using RemoteHealthcare.Ergometer.Software;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RemoteHealthcare_Client
{
    public class StartupLoader
    {

        private DataManager serverDataManager;
        private DataManager deviceDataManager;
        private DataManager vrDataManager;

        public event EventHandler<List<ClientData>> OnVRConnectionsReceived;
        public event EventHandler<List<string>> OnBLEDeviceReceived;
        public event EventHandler<bool> OnLoginResponseReceived;

        public void Start()
        {
            GetAvailableVRConnections();
            GetAvailableBLEDevices();

            // starting up the connection to the server
            this.serverDataManager = new ServerDataManager("127.0.0.1", 6969);
        }

        public void GetAvailableVRConnections()
        {
            // The gui needs all the available vr servers to connect to
            // To get this list it is needed to start up the vrDataManager
            VRDataManager dataManager = new VRDataManager();
            List<ClientData> clientVREngines =  dataManager.VRTunnelHandler.GetAvailableClients();

            this.vrDataManager = dataManager;

            this.OnVRConnectionsReceived?.Invoke(this, clientVREngines);
        }

        public void GetAvailableBLEDevices()
        {

            // TODO !! blocking call
            // Gets all the bleutooth devices available
            List<string> blDevices = PhysicalDevice.ReadAllDevices();
            blDevices.Add("Simulator");

            this.OnBLEDeviceReceived.Invoke(this, blDevices);
        }

        public void SetupServerConnection(string ip, int port, string device, string vrServerID, string username, string password)
        {

            // Setting op serverDataManager, it creates the connection to the server
            this.serverDataManager = new ServerDataManager(ip, port);

            this.deviceDataManager = new DeviceDataManager(device, "Decathlon Dual HR");

            this.serverDataManager.NetworkManagers.Add(deviceDataManager);
            this.serverDataManager.NetworkManagers.Add(vrDataManager);

            this.vrDataManager.NetworkManagers.Add(serverDataManager);
            this.vrDataManager.NetworkManagers.Add(deviceDataManager);

            this.deviceDataManager.NetworkManagers.Add(serverDataManager);
            this.deviceDataManager.NetworkManagers.Add(vrDataManager);

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

            // Seniding the login data to the server
            this.serverDataManager?.ReceivedData(loginCommand);
        }
    }
}
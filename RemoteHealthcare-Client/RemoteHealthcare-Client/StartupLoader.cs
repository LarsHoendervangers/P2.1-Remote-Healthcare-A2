using RemoteHealthcare.ClientVREngine.Util.Structs;
using RemoteHealthcare.Ergometer.Software;
using System.Collections.Generic;
using System.Diagnostics;

namespace RemoteHealthcare_Client
{
    public class StartupLoader
    {

        private DataManager serverDataManager;
        private DataManager deviceDataManager;
        private DataManager vrDataManager;


        public List<ClientData> GetVRConnections()
        {
            // The gui needs all the available vr servers to connect to
            // To get this list it is needed to start up the vrDataManager
            VRDataManager dataManager = new VRDataManager();
            List<ClientData> clientDatas =  dataManager.VRTunnelHandler.GetAvailableClients();

            this.vrDataManager = dataManager;

            return clientDatas;
        }

        public void SetupServerConnection(string device, string vrServer)
        {
            SetupServerConnection("127.0.0.1", 6969, device, vrServer);
        }

        public void SetupServerConnection(string ip, int port, string device, string vrServerID)
        {

            // Setting op serverDataManager, it creates the connection to the server
            this.serverDataManager = new ServerDataManager(ip, port);

            this.deviceDataManager = new DeviceDataManager(device, "Decathlon Dual HR");

            this.serverDataManager.DeviceDataManager = deviceDataManager;
            this.serverDataManager.VRDataManager = vrDataManager;
            this.deviceDataManager.ServerDataManager = serverDataManager;

            (this.vrDataManager as VRDataManager)?.Start(vrServerID);
        }

    }
}
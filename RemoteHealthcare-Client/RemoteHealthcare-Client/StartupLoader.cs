using RemoteHealthcare.Ergometer.Software;
using System.Diagnostics;

namespace RemoteHealthcare_Client
{
    public class StartupLoader
    {

        private DataManager serverDataManager;
        private DataManager deviceDataManager;
        private DataManager vrDataManager;

        public StartupLoader()
        {

            //SetupServerConnection("127.0.0.1", 6969);
        }

        public void SetupServerConnection(string device, string vrEngine)
        {
            SetupServerConnection("127.0.0.1", 6969, device, vrEngine);
        }

        public void SetupServerConnection(string ip, int port, string device, string vrEngine)
        {
            // Setting op serverDataManager, it creates the connection to the server
            this.serverDataManager = new ServerDataManager(ip, port);

            this.deviceDataManager = new DeviceDataManager(device, "Decathlon Dual HR");

            this.serverDataManager.DeviceDataManager = deviceDataManager;
            this.deviceDataManager.ServerDataManager = serverDataManager;
        }

    }
}
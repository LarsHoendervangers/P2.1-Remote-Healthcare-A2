using RemoteHealthcare.Ergometer.Software;
using System.Diagnostics;

namespace RemoteHealthcare_Client
{
    public class StartupLoader
    {

        private IDataManager serverDataManager;
        private IDataManager devideDataManager;
        private IDataManager vrDataManager;

        public StartupLoader()
        {

            //SetupServerConnection("127.0.0.1", 6969);

            Trace.WriteLine("*************************************");
            Trace.WriteLine(PhysicalDevice.ReadAllDevices().ToArray());
            foreach (string s in PhysicalDevice.ReadAllDevices())
            {
                Trace.WriteLine(s);
            }
        }

        public void SetupServerConnection(string ip, int port)
        {
            // Setting op serverDataManager, it creates the connection to the server
            this.serverDataManager = new ServerDataManager(ip, port);

        }

    }
}
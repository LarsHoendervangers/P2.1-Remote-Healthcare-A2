using Newtonsoft.Json;
using RemoteHealthcare.ClientVREngine.Util.Structs;
using RemoteHealthcare.Ergometer.Software;
using RemoteHealthcare_Client.ClientVREngine.Scene;
using RemoteHealthcare_Client.ClientVREngine.Tunnel;
using RemoteHealthcare_Client.TCP;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RemoteHealthcare_Client
{
    class ClientViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private StartupLoader loader;
        private TCPClientHandler handler;

        public ClientViewModel(StartupLoader loader, TCPClientHandler handler)
        {
            // TODO !! blocking call
            List<string> blDevices = PhysicalDevice.ReadAllDevices();
            blDevices.Add("Simulator");
            this.mBLEDevices = new ObservableCollection<string>(blDevices);

            // Setting all the VRserers list
            // !! Also blocking call
            this.mVRServers = new ObservableCollection<ClientData>(loader.GetVRConnections());

            List<string> scenes = new List<string>();
            scenes.Add(new SimpleScene(new TunnelHandler()).ToString());
            this.mScenes = new ObservableCollection<string>(scenes);

            this.loader = loader;
            this.handler = handler;
        }

        private ObservableCollection<ClientData> mVRServers;
        public ObservableCollection<ClientData> VRServers
        {
            get { return mVRServers; }
            set
            {   

                mVRServers = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("vrServers"));

            }
        }

        private ClientData mSelectedVRServer = new ClientData();
        public ClientData SelectedVRServer
        {
            get { return mSelectedVRServer; }
            set
            {
                mSelectedVRServer = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("selectedVRServer"));
            }
        }

        private ObservableCollection<string> mBLEDevices;
        public ObservableCollection<string> BLEDevices
        {
            get { return mBLEDevices; }
            set
            {

                mBLEDevices = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BLEDevices"));

            }
        }

        private string mSelectedDevice = null;
        public string SelectedDevice
        {
            get { return mSelectedDevice; }
            set
            {
                mSelectedDevice = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("selectedDevice"));
            }
        }

        private ObservableCollection<string> mScenes;
        public ObservableCollection<string> Scenes
        {
            get { return mScenes; }
            set
            {
                mScenes = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Selected scene"));
            }
        }

        private ICommand mStartCommand;
        public ICommand StartCommand
        {
            get
            {
                if (mStartCommand == null)
                {
                    mStartCommand = new GeneralCommand(
                        param => StartApplication(),
                        param => (true)
                        );
                }
                return mStartCommand;
            }

        }

        private void StartApplication()
        {
            this.loader.SetupServerConnection(SelectedDevice, SelectedVRServer.Adress, UserName, Password);
        }

        private string mUserName = null;
        public string UserName
        {
            get { return mUserName; }
            set
            {
                mUserName = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("User name"));
            }
        }

        private string mPassword = null;
        public string Password
        {
            get { return mPassword; }
            set
            {
                mPassword = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Password"));
            }
        }
    }

    
}

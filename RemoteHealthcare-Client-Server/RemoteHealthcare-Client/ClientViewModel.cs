﻿using RemoteHealthcare.ClientVREngine.Util.Structs;
using RemoteHealthcare.Ergometer.Software;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RemoteHealthcare_Client
{
    class ClientViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private StartupLoader loader;

        public ClientViewModel(StartupLoader loader)
        {
            // TODO !! blocking call
            List<string> blDevices = PhysicalDevice.ReadAllDevices();
            blDevices.Add("Simulator");
            this.mBLEDevices = new ObservableCollection<string>(blDevices);

            // Setting all the VRserers list
            // !! Also blocking call
            this.mVRServers = new ObservableCollection<ClientData>(loader.GetVRConnections());


            this.loader = loader;
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
            this.loader.SetupServerConnection(SelectedDevice, SelectedVRServer.Adress);
        }
    }
}
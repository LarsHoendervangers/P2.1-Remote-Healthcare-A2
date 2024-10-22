﻿using Newtonsoft.Json;
using RemoteHealthcare.ClientVREngine.Util.Structs;
using RemoteHealthcare.Ergometer.Software;
using RemoteHealthcare_Client.ClientVREngine.Scene;
using RemoteHealthcare_Client.ClientVREngine.Tunnel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RemoteHealthcare_Client
{
    /// <summary>
    /// Class that represents the viewmodel for the client application
    /// </summary>
    public class ClientViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly StartupLoader loader;


        /// <summary>
        /// Constructor for the Client view model, starts the calls to get available bl- and vr- devices
        /// </summary>
        /// <param name="loader">The StartupLoader that handles startup</param>
        public ClientViewModel(StartupLoader loader)
        {
            this.loader = loader;

            // Setting the event for the device callbacks
            this.loader.OnVRConnectionsReceived += (s, d) => this.mVRServers = new ObservableCollection<ClientData>(d);
            this.loader.OnBLEDeviceReceived += (s, d) => this.mBLEDevices = new ObservableCollection<string>(d);
            this.loader.OnLoginResponseReceived += (s, d) =>
            {
                this.isLoggedIn = d;
                if (d)
                {
                    SubmitText = "Start VR";
                    WrongCredentialsOpacity = 0;
                    RightCredentialsOpacity = 100;
                    IsEnabledCredentialBoxes = false;
                    IsEnabledComboBoxes = true;
                }

                if (!d) WrongCredentialsOpacity = 100;
            };
            // Calling the first statup method for the loader
            this.loader.Init();
            
            // Setting the list with Scenes the user can choose from
            List<GeneralScene> scenes = new List<GeneralScene>();
            scenes.Add(new SimpleScene(new TunnelHandler()));
            scenes.Add(new PodraceScene(new TunnelHandler()));
            this.mScenes = new ObservableCollection<GeneralScene>(scenes);
            this.SelectedScene = scenes[0];
        }

        /// <summary>
        /// Binding to the text below the login, it tells the user if the login failed or succeded
        /// </summary>
        private string mSubmitText = "Submit login";
        public string SubmitText
        {
            get { return mSubmitText; }
            set
            {
                mSubmitText = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SubmitText"));
            }
        }

        /// <summary>
        /// Binded list attributes that contains all the vr server data
        /// </summary>
        private ObservableCollection<ClientData> mVRServers;
        public ObservableCollection<ClientData> VRServers
        {
            get { return mVRServers; }
            set
            {
                mVRServers = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VRServers"));
            }
        }

        /// <summary>
        /// Binded attributte that contains the selected vr server
        /// </summary>
        private ClientData mSelectedVRServer = new ClientData();
        public ClientData SelectedVRServer
        {
            get { return mSelectedVRServer; }
            set
            {
                mSelectedVRServer = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedVRServer"));
            }
        }

        /// <summary>
        /// Binded list that contains all the bl devices
        /// </summary>
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

        /// <summary>
        /// Binded Attribute that contains the selected bl device
        /// </summary>
        private string mSelectedDevice = null;
        public string SelectedDevice
        {
            get { return mSelectedDevice; }
            set
            {
                mSelectedDevice = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedDevice"));
            }
        }

        /// <summary>
        /// Binded list attribute that contains the scenes
        /// </summary>
        private ObservableCollection<GeneralScene> mScenes;
        public ObservableCollection<GeneralScene> Scenes
        {
            get { return mScenes; }
            set
            {
                mScenes = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Scenes"));
            }
        }

        /// <summary>
        /// Binding with the selected scene from the scene combobox
        /// </summary>
        private GeneralScene mSelectedScene = null;
        public GeneralScene SelectedScene
        {
            get { return mSelectedScene; }
            set
            {
                mSelectedScene = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedScene"));
            }
        }

        /// <summary>
        /// Binded attribute that stores the username 
        /// </summary>
        private string mUserName = null;
        public string UserName
        {
            get { return mUserName; }
            set
            {
                mUserName = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UserName"));
            }
        }

        /// <summary>
        /// Binded attribute that stores the password 
        /// </summary>
        private string mPassword = null;
        public string Password
        {
            get { return mPassword; }
            set
            {
                mPassword = value;
            }
        }

        /// <summary>
        /// Binding if the buttons to login is are enabled or disabled
        /// </summary>
        private bool mIsEnabledCredentialBoxes = true;
        public bool IsEnabledCredentialBoxes
        {
            get { return mIsEnabledCredentialBoxes; }
            set
            {
                mIsEnabledCredentialBoxes = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsEnabledCredentialBoxes"));
            }
        }

        /// <summary>
        /// Binding if the buttons to select a divece and VR-server is are enabled or disabled
        /// </summary>
        private bool mIsEnabledComboBoxes = false;
        public bool IsEnabledComboBoxes
        {
            get { return mIsEnabledComboBoxes; }
            set
            {
                mIsEnabledComboBoxes = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsEnabledComboBoxes"));
            }
        }

        /// <summary>
        /// Binding if the startbutton is enabled or disabled
        /// </summary>
        private bool mIsEnabledStartButton = true;
        public bool IsEnabledStartButton
        {
            get { return mIsEnabledStartButton; }
            set
            {
                mIsEnabledStartButton = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsEnabledStartButton"));
            }
        }

        /// <summary>
        /// Binding that sets the opacity for the login text if the login failed
        /// </summary>
        private int mWrongCredentialsOpacity = 0;
        public int WrongCredentialsOpacity
        {
            get { return mWrongCredentialsOpacity; }
            set
            {
                mWrongCredentialsOpacity = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("WrongCredentialsOpacity"));
            }
        }

        /// <summary>
        /// Binding that sets the opacity for the login text if the login succeded
        /// </summary>
        private int mRightCredentialsOpacity = 0;
        public int RightCredentialsOpacity
        {
            get { return mRightCredentialsOpacity; }
            set
            {
                mRightCredentialsOpacity = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RightCredentialsOpacity"));
            }
        }

        public bool isLoggedIn = false;

        /// <summary>
        /// Command that is called when the client presses the start button
        /// </summary>
        private ICommand mStartCommand;
        public ICommand StartCommand
        {
            get
            {
                if (mStartCommand == null)
                {
                    mStartCommand = new GeneralCommand(
                        param =>
                        {
                            if (!isLoggedIn)
                            {
                                if(UserName.ToLower().Contains("Twan".ToLower()))
                                {
                                    MessageBox.Show("Twan.exe has stopped working", "ERROR");
                                    return;
                                }
                                this.loader.Login(UserName, Password);
                            }
                                
                            else StartApplicaton();
                        },
                        param => NullCheck() //check if all the fields are filled
                        );
                }
                return mStartCommand;
            }
        }

        /// <summary>
        /// Checks all the fields in the class if they are null, returns true if all fields are filled
        /// </summary>
        /// <returns>true is all attributes are NOT null</returns>
        private bool NullCheck()
        {
            return
                this.Password != null &
                this.UserName != null;
        }


        /// <summary>
        /// Starts the application by calling the loader to start
        /// </summary>
        private void StartApplicaton()
        {

            Debug.WriteLine("Starting Application");
            this.loader.Start(this.SelectedDevice, this.SelectedVRServer.Adress, this.SelectedScene);
            IsEnabledStartButton = false;
            IsEnabledComboBoxes = false;
        }

        [Obsolete] // No longer in use
        private void UpdateVRServers()
        {
            while (true)
            {
                loader.GetAvailableVRConnections();
                Thread.Sleep(3000);
            }
        }

        [Obsolete] // No longer in user
        private void UpdateBLEDevices()
        {
            while (true)
            {
                loader.GetAvailableBLEDevices();
            }
        }
    }
}

﻿using RemoteHealthcare_Shared.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RemoteHealthcare_Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Server server;

        public MainWindow()
        {
            InitializeComponent();
            this.server = new Server(this, IPAddress.Any, ServerSettings.Port);
            this.server.StartServer();
            this.Closing += OnDestroy;
        }

        private void OnDestroy(object sender, CancelEventArgs e)
        {
            this.server.StopServer();
            //this.server.users.OnDestroy();
            //Environment.Exit(Environment.ExitCode);
        }

        public void Button_SendMessage(object sender, RoutedEventArgs e)
        {
            this.server.Broadcast(ChatInputBox.Text);
        }
    }
}

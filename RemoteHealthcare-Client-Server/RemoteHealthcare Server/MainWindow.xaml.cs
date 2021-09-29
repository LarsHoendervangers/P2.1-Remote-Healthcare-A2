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
            //this.server = new Server(this, IPAddress.Parse("145.49.40.199"), 6969);
            this.server = new Server(this, IPAddress.Parse("127.0.0.1"), 6969);
            this.Closing += OnDestroy;
        }

        private void OnDestroy(object sender, CancelEventArgs e)
        {
            this.server.users.OnDestroy();
        }

        public void Button_StartServer(object sender, RoutedEventArgs e)
        {
            this.server.StartServer();
        }

        public void Button_StopServer(object sender, RoutedEventArgs e)
        {
            this.server.StopServer();
        }

        public void Button_SendMessage(object sender, RoutedEventArgs e)
        {
            this.server.Broadcast(ChatInputBox.Text);
        }
    }
}

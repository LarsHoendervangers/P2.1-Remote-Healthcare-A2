using System;
using System.Collections.Generic;
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
            this.server = new Server(this, IPAddress.Parse("127.0.0.1"), 6969);
        }

        public void Button_StartServer(object sender, RoutedEventArgs e)
        {
            this.server.StartServer();
        }

        public void Button_StopServer(object sender, RoutedEventArgs e)
        {
            this.server.StopServer();
        }
    }
}

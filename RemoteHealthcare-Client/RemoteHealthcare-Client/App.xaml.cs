using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RemoteHealthcare_Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            //base.OnStartup(e);
            MainWindow window = new MainWindow();
            ClientViewModel VM = new ClientViewModel();
            window.DataContext = VM;
            window.Show();
        }

    }
}

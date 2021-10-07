using RemoteHealthcare_Client.TCP;
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

        /// <summary>
        /// Starts the main application and sets al the needed classes
        /// </summary>
        /// <param name="e">The StartupEventArgs given on startup</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            //base.OnStartup(e);
            MainWindow window = new MainWindow();

            // starting the viewmodel connected to the gui
            ClientViewModel VM = new ClientViewModel(new StartupLoader());

            // Starting the GUI
            window.DataContext = VM;
            window.Show();
        }

    }
}

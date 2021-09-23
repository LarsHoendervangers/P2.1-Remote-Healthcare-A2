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
        IDataManager manager;

        protected override void OnStartup(StartupEventArgs e)
        {
            manager = new ServerDataManager("145.49.5.228", 6969);
            //IDataManager manager = new ServerDataManager("145.49.53.124", 6969);

        }

    }
}

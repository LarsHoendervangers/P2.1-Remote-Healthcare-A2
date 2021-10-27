using RemoteHealthcare_Dokter.BackEnd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RemoteHealthcare_Dokter.ViewModels;
using System.Diagnostics;

namespace RemoteHealthcare_Dokter.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public bool Shutdown = true;

        private ServerDataManager dataManager;
        public LoginView()
        {
            InitializeComponent();
            dataManager = new ServerDataManager();

            this.Closed += MainWindow_Closed;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            if (this.Shutdown == true)
                Environment.Exit(Environment.ExitCode);
        }

        private void PassBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((LoginViewModel)this.DataContext).Password = ((PasswordBox)sender).Password; }
        }
    }
}

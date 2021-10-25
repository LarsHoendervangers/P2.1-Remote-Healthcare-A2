using RemoteHealthcare_Dokter.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RemoteHealthcare_Dokter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //DataContext = new DashboardViewModel();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Dashboard_Clicked(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();

            /*
            DataContext = new DashboardViewModel();
            DokterButton.Background = (Brush)bc.ConvertFrom("#FFFFFDFD");
            PatientButton.Background = (Brush)bc.ConvertFrom("#FFFFFFFF");
            */
        }

        private void Patient_Click(object sender, RoutedEventArgs e)
        {
            /*
            var bc = new BrushConverter();

            DataContext = new PatientListViewModel();
            DokterButton.Background = (Brush)bc.ConvertFrom("#FFFFFFFF");
            PatientButton.Background = (Brush)bc.ConvertFrom("#FFFFFDFD");
            */
        }
    }
}

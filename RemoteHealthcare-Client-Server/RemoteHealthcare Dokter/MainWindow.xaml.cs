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
        }

        private void Dashboard_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new DashboardViewModel();
        }

        private void Patient_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new PatientViewModel();
        }
    }
}

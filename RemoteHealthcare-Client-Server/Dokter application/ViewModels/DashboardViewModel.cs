using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Dokter_application.ViewModels
{
    internal class DashboardViewModel : ViewModelBase
    {
        public string WelcomeMessage => "Welcome to the dashboard";

        public ICommand NavigateAccountCommand { get; }
    }
}

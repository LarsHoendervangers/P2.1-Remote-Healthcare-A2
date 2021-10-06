using Dokter_application.ViewModels;

namespace Dokter_application
{
    internal class MainViewModel : ViewModelBase
    {
        public ViewModelBase CurrentViewModel { get; }

        public MainViewModel()
        {
            CurrentViewModel = new DashboardViewModel();
        }
    }
}
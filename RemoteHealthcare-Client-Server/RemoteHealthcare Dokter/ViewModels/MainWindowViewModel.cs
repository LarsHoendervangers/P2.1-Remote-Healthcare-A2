using RemoteHealthcare_Dokter.BackEnd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Dokter.ViewModels
{
    class MainWindowViewModel
    {
        private ServerDataManager ServerManager;

        public MainWindowViewModel()
        {
            this.ServerManager = new ServerDataManager();
        }
    }
}

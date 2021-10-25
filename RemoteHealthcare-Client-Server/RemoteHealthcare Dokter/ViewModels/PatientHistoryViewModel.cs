using RemoteHealthcare_Client;
using RemoteHealthcare_Dokter.BackEnd;
using RemoteHealthcare_Server;
using RemoteHealthcare_Shared.DataStructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RemoteHealthcare_Dokter.ViewModels
{
    class PatientHistoryViewModel
    {
        private Window window;
        private SharedPatient Patient;
        private Session Session;
        private PatientHisoryManager manager;

        public PatientHistoryViewModel(Window window, SharedPatient selectedPatient, Session selectedSession)
        {
            this.window = window;
            this.Patient = selectedPatient;
            this.Session = selectedSession;

            this.FullName = this.Patient.FirstName + " " + this.Patient.LastName;
            this.Age = "Leeftijd:\t\t" + CalculateAge();
            this.ID = "ID persoon:\t" + this.Patient.ID;
        }

        private string _FullName;
        public string FullName
        {
            get { return _FullName; }
            set
            {
                _FullName = value;
            }
        }

        private string _Age;
        public string Age
        {
            get { return _Age; }
            set
            {

                _Age = value;
            }
        }

        private int CalculateAge()
        {
            int years = DateTime.Now.Year - this.Patient.DateOfBirth.Year;
            if (DateTime.Now.Month > this.Patient.DateOfBirth.Month)
            {
                return years;
            }
            else if (DateTime.Now.Month < this.Patient.DateOfBirth.Month)
            {
                return years - 1;
            }
            else
            {
                if (DateTime.Now.Day >= this.Patient.DateOfBirth.Day)
                {
                    return years;
                }

                return years - 1;
            }
        }

        private string _ID;
        public string ID
        {
            get { return _ID; }
            set
            {
                _ID = value;
            }
        }

        private ICommand _CloseHistoryCommand;
        public ICommand CloseHistoryCommand
        {
            get
            {
                if (_CloseHistoryCommand == null)
                {
                    _CloseHistoryCommand = new GeneralCommand(
                        param => CloseHistory()
                        ); ; ;
                }
                return _CloseHistoryCommand;
            }

        }

        private void CloseHistory()
        {
            this.window.Content = new PatientListViewModel(this.window);
        }
    }
}

using RemoteHealthcare_Server.Data.Logic;
using RemoteHealthcare_Server.Data.Logic.SubManagers;
using RemoteHealthcare_Server.Data.Processing;
using RemoteHealthcare_Server.Data.User;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Server.Data
{
    public class UserManagement
    {
        //Submangers
        private LoginSubmanager loginManager;
        private SessionSubmanager sessionManager;
        private FindingSubmanager findingSubmanager;


        //Data
        public static List<IUser> users;

        //Active
        public static List<Session> activeSessions;
        public List<Host> activeHosts;

        public UserManagement()
        {
            this.loginManager = new LoginSubmanager(this);
            this.sessionManager = new SessionSubmanager(this);
            this.findingSubmanager = new FindingSubmanager(this);

            //Lists
            users = new List<IUser>();
            activeSessions = new List<Session>();
            activeHosts = new List<Host>();

            //Loading
            try {
                //first trying filees
                users = FileProcessing.LoadUsers();
            }
            catch  {
                BackupUsers();
            }
        }

        #region Internal Management
        public void OnDestroy()
        {
            FileProcessing.SaveUsers(users);
            foreach (Host host in this.activeHosts)
            {
                host.Stop(host);
            }
        }

        private void BackupUsers()
        {
            //Notify user
            Server.PrintToGUI("Error in reading...");

            //Backup data
            users.Add(new Admin("Admin", "Password123", true));
            users.Add(new Patient("JHAOogstvogel", "Welkom123", new DateTime(2002, 2, 1), "Joe", "Oogstvogel", "A12345", true));
            users.Add(new Patient("RCADuinen", "ElpticCurves", new DateTime(1969, 2, 2), "Ronald", "Duinen", "A12346", true));
            users.Add(new Patient("AESPeeren", "AESisTheBest", new DateTime(1969, 2, 2), "Arnold", "Peeren", "A12347", true));
            users.Add(new Patient(" ", " ", new DateTime(1969, 2, 2), "Arnold", "Peeren", "A12347", true));
            users.Add(new Doctor("COMBomen", "Communication", new DateTime(1969, 2, 2), "Cornee", "Bomen", 
                "Doctor FyssioTherapy", "PHD Avans Hogeschool", true));
            users.Add(new Doctor("Twan", "wachtwoord", new DateTime(2002, 5, 8), "Twan", "van Noorloos",
                "Doctor FyssioTherapy", "PHD Avans Hogeschool", true));
        }
        #endregion

        #region Method Wrappers
        internal IUser Credentials(string username, string password, int flag)
        {
            return this.loginManager.Credentials(username, password, flag);
        }

        internal Host FindHost(string v)
        {
            return this.findingSubmanager.FindHost(v);
        }

        internal List<string> GetAllPatients()
        {
            return this.findingSubmanager.GetAllPatients();
        }

        internal List<string> GetActivePatients()
        {
            return this.findingSubmanager.GetActivePatients();
        }

        internal void Subscribe(Doctor d, List<string> patientIdentiefiers)
        {
            this.sessionManager.Subscribe(d, patientIdentiefiers);
        }

        internal void Unsubscribe(Doctor d, List<string> patientIdentiefiers)
        {
            this.sessionManager.Unsubscribe(d, patientIdentiefiers);
        }

        internal Patient FindPatient(string v)
        {
            return this.findingSubmanager.FindPatient(v);
        }

        internal void SessionStart(Patient p)
        {
            this.sessionManager.SessionStart(p);
        }

        internal void SessionEnd(IUser user)
        {
            this.sessionManager.SessionEnd(user);
        }

        internal Session SessionUpdateBike(int v1, int v2, int v3, int v4, int v5, DateTime dateTime, IUser user)
        {
           return this.sessionManager.SessionUpdateBike(v1, v2, v3, v4, v5, dateTime, user);
        }

        internal Session SessionUpdateHRM(DateTime dateTime, int v, IUser user)
        {
           return this.sessionManager.SessionUpdateHRM(dateTime, v, user);
        }

        internal Host FindHost(Doctor d)
        {
            return this.findingSubmanager.FindHost(d);
        }

        internal bool FindSessoin(Patient p)
        {
            return this.findingSubmanager.GetSession(p);
        }

        #endregion
    }
}




















  


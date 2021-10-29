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
            users.Add(new Patient(" ", " ", new DateTime(1969, 2, 2), "Arnold", "Peeren", "A69420", true));
            users.Add(new Patient("Lars", " ", new DateTime(1969, 2, 2), "Lars", "Hoendervangers", "A42069", true));
            users.Add(new Patient("Luuk", " ", new DateTime(1969, 2, 2), "Luuk", "van Berkel", "A96669", true));
            users.Add(new Patient("Twan", " ", new DateTime(1969, 2, 2), "Twan", "van Noorloos", "A66666", true));
            users.Add(new Patient("Jesse", " ", new DateTime(1969, 2, 2), "Jesse", "Krijgsman", "A25565", true));
            users.Add(new Doctor("COMBomen", "Communication", new DateTime(1969, 2, 2), "Cornee", "Bomen", 
                "Doctor FyssioTherapy", "PHD Avans Hogeschool", true));
            users.Add(new Doctor("Twan", "wachtwoord", new DateTime(2002, 5, 8), "Twan", "van Noorloos",
                "Doctor FyssioTherapy", "PHD Avans Hogeschool", true));
        }
        #endregion

        #region Method Wrappers
        public IUser Credentials(string username, string password, int flag)
        {
            return this.loginManager.Credentials(username, password, flag);
        }

        public Host FindHost(string v)
        {
            return this.findingSubmanager.FindHost(v);
        }

        public List<string> GetAllPatients()
        {
            return this.findingSubmanager.GetAllPatients();
        }

        public List<string> GetActivePatients()
        {
            return this.findingSubmanager.GetActivePatients();
        }

        public void Subscribe(Doctor d, List<string> patientIdentiefiers)
        {
            this.sessionManager.Subscribe(d, patientIdentiefiers);
        }

        public void Unsubscribe(Doctor d, List<string> patientIdentiefiers)
        {
            this.sessionManager.Unsubscribe(d, patientIdentiefiers);
        }

        public Patient FindPatient(string v)
        {
            return this.findingSubmanager.FindPatient(v);
        }

        public void SessionStart(Patient p)
        {
            this.sessionManager.SessionStart(p);
        }

        public void SessionEnd(IUser user)
        {
            this.sessionManager.SessionEnd(user);
        }

        public Session SessionUpdateBike(int v1, int v2, int v3, int v4, int v5, DateTime dateTime, IUser user)
        {
           return this.sessionManager.SessionUpdateBike(v1, v2, v3, v4, v5, dateTime, user);
        }

        public Session SessionUpdateHRM(DateTime dateTime, int v, IUser user)
        {
           return this.sessionManager.SessionUpdateHRM(dateTime, v, user);
        }

        public Host FindHost(Doctor d)
        {
            return this.findingSubmanager.FindHost(d);
        }

        public bool FindSessoin(Patient p)
        {
            return this.findingSubmanager.GetSession(p);
        }

        #endregion
    }
}




















  


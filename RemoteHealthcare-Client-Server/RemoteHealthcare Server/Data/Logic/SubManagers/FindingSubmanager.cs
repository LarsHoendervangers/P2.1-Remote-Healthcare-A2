using RemoteHealthcare_Server.Data.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Server.Data.Logic
{
    class FindingSubmanager
    {
        private UserManagement management;

        public FindingSubmanager(UserManagement userManagement)
        {
            this.management = userManagement;
        }

        /// <summary>
        /// Finds a patient based on ID
        /// </summary>
        /// <param name="patientID">This is the ID for the search</param>
        /// <returns></returns>
        public Patient FindPatient(string patientID)
        {

            List<Patient> patients = new List<Patient>();
            foreach (IUser user in UserManagement.users)
            {
                if (user.getUserType() == UserTypes.Patient)
                {
                    patients.Add(user as Patient);
                }
            }

            foreach (Patient p in patients)
            {
                if (p.PatientID == patientID)
                {

                    return p;
                }
            }
            
            return null;
        }

        /// <summary>
        /// Finds a host based on patientID
        /// </summary>
        /// <param name="patientID">This is the patientID based for the search</param>
        /// <returns></returns>
        public Host FindHost(string patientID)
        {
            foreach (Host h in this.management.activeHosts)
            {
                if (h.GetUser().getUserType() == UserTypes.Patient)
                {
                    Patient p = (Patient)h.GetUser();
                    if (p.PatientID == patientID)
                    {
                        return h;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// This function returns all active patients
        /// </summary>
        /// <returns>List with patients</returns>
        public List<string> GetActivePatients()
        {
            List<string> activePatients = new List<string>();
            foreach (Host h in this.management.activeHosts)
            {


                if (h.GetUser() != null && h.GetUser().getUserType() == UserTypes.Patient)
                {
                    Patient p = (Patient)h.GetUser();
                    activePatients.Add(p.PatientID);
                }
            }
            return activePatients;
        }

        /// <summary>
        /// Returns all patients
        /// </summary>
        /// <returns>List with patients</returns>
        public List<string> GetAllPatients()
        {
            List<string> activePatients = new List<string>();
            foreach (IUser user in UserManagement.users)
            {
                if (user.getUserType() == UserTypes.Patient)
                {
                    Patient p = (Patient)user;
                    activePatients.Add(p.PatientID);
                }
            }
            return activePatients;
        }

        /// <summary>
        /// Find a host based on doctor object
        /// </summary>
        /// <param name="d">The doctor for the host</param>
        /// <returns></returns>
        public Host FindHost(Doctor d)
        {
            foreach (Host h in this.management.activeHosts)
            {
                if (h.GetUser().getUserType() == UserTypes.Doctor)
                {
                    Doctor doctor = h.GetUser() as Doctor;
                    if (doctor == d)
                    {
                        return h;
                    }
                }
            }

            return null;
        }

        public bool GetSession(Patient p)
        {
            Server.PrintToGUI(UserManagement.activeSessions.Count + " count of sessions");
            foreach (Session s in UserManagement.activeSessions)
            {
                if (s.Patient == p)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

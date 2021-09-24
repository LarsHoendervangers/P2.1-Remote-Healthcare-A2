using RemoteHealthcare_Server.Data.User;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Server.Data
{
    public class Usermanagement
    {
        private List<Patient> patients;
        private List<Doctor> doctors;
        private Admin admin;


        public Usermanagement()
        {


            //*************NOTE***********
            //Temporary needs to be read of disk


            //Admin this acount has instructions for adding users. How else would you add users then.
            //The doctors should probably not do that in my opinion
            //Kind regards Luuk
            admin = new Admin("Admin", "Password123");

            //Lists for users
            patients = new List<Patient>();
            doctors = new List<Doctor>();

            //Filling lists
            patients.Add(new Patient("JHAOogstvogel", "Welkom123", new DateTime(2002, 2, 1),  "Joe", "Oogstvogel",  "A12345"));
            patients.Add(new Patient("RCADuinen", "ElpticCurves", new DateTime(1969, 2, 2), "Ronald", "Duinen", "A12346"));
            patients.Add(new Patient("AESPeeren", "AESisTheBest", new DateTime(1969, 2, 2), "Arnold", "Peeren", "A12347"));
            doctors.Add(new Doctor("COMBomen", "Communication", new DateTime(1969, 2, 2), "Cornee", "Bomen", "Doctor FyssioTherapy", "PHD Avans Hogeschool"));

            //*******************************


        }


        public void AddPatient(Patient p)
        {
            //Needs to be implemented
            //Also needs to call to a mthode in file processing for writing to the list.
        }

        public void AddDoctor(Doctor d)
        {
            //Needs to be implemented
            //Also needs to call to a mthode in file processing for writing to the list.
        }

        public void RemovePatient(Patient p)
        {
            //Needs to be implemented
            //Also needs to call to a mthode in file processing for writing to the list.
        }

        public void RemoveDoctor(Doctor d)
        {
            //Needst to be implemented
            //Also needs to call to a mthode in file processing for writing to the list.
        }

        public Patient CheckPatientCredentials(string username, string password)
        {
            //Finding patient
            foreach(Patient p in this.patients){
                if (p.Password == password && p.Username == username)
                {
                    return p;
                }
            }

            //Patient not found
            return null;
        }

        public Doctor CheckDoctorCredentials(string username, string password)
        {
            //Finding patient
            foreach (Doctor d in this.doctors)
            {
                if (d.Password == password && d.Username == username)
                {
                    return d;
                }
            }

            //Patient not found
            return null;
        }

        public Admin CheckAdminCredentials(string username, string password)
        {
            Debug.WriteLine(password + this.admin.Password);
            if (this.admin.Password == password && this.admin.Username == username)
            {
                
                return admin;
            }
            return null;
        }
    }
}

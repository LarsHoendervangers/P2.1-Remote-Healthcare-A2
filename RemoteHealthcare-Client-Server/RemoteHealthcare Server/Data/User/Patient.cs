using RemoteHealthcare_Server.Data;
using RemoteHealthcare_Server.Data.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteHealthcare_Server
{
    public class Patient : IUser
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PatientID { get; set; }

        public Session session { get; set; }



        public Patient(string username, string password, DateTime dateOfBirth, string firstName, string lastName, string medicalSystemID)
        {
            //Username and login
            this.Username = username;
            this.Password = password;

            //Patien data
            this.DateOfBirth = dateOfBirth;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.PatientID = medicalSystemID;


         
        }

        public int getType()
        {
            return 0;
        }

        public void sessionSetter(Session s)
        {
            this.session = s;
        }


        public Session sessionGetter()
        {
            return session;
        }
    }
}
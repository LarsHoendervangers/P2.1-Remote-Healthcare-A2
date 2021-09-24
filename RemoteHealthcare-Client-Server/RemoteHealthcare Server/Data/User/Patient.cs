using RemoteHealthcare_Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteHealthcare_Server
{
    public class Patient
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MedicalSystemID { get; set; }



        public Patient(string username, string password, DateTime dateOfBirth, string firstName, string lastName, string medicalSystemID)
        {
            //Username and login
            this.Username = username;
            this.Password = password;

            //Patien data
            this.DateOfBirth = dateOfBirth;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.MedicalSystemID = medicalSystemID;
         
        }

      
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteHealthcare_Server
{
    public class Patient
    {
        public string Username { get; set; }

        public DateTime DateOfBirth { get; set; }

        public Session Session { get; set; }


        public string password { get; set; }

        public Patient(string username, string password, DateTime dateOfBirth, Session session)
        {
            this.Username = username;
            this.DateOfBirth = dateOfBirth;
            this.Session = session;
        }
    }
}
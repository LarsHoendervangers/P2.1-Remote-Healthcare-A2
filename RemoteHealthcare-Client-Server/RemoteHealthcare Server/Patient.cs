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

        public Host Host { get; set; }

        public Patient(string username, DateTime dateOfBirth, Session session, Host host)
        {
            this.Username = username;
            this.DateOfBirth = dateOfBirth;
            this.Session = session;
            this.Host = host;
        }
    }
}
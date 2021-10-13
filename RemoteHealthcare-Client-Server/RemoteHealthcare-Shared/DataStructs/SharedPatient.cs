using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteHealthcare_Shared.DataStructs
{
    public struct SharedPatient
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ID { get; set; }
        public bool InSession { get; set; }
        public DateTime DateOfBirth { get; set; }

        public SharedPatient(string firstName, string lastName, string iD, bool inSession, DateTime dateOfBirth)
        {
            FirstName = firstName;
            LastName = lastName;
            ID = iD;
            InSession = inSession;
            DateOfBirth = dateOfBirth;
        }
    }
}
    
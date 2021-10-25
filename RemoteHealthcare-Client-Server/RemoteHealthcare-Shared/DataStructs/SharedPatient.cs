using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        /// <summary>
        /// Struct that is used to store the patient data.
        /// </summary>
        /// <param name="firstName">The first name of the user.</param>
        /// <param name="lastName">The last name of the user.</param>
        /// <param name="iD">Unique patient id.</param>
        /// <param name="inSession">Indicates whether a patient is currently in a session.</param>
        /// <param name="dateOfBirth">Birtdate of the patient.</param>
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
    
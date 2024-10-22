﻿using RemoteHealthcare_Server.Data;
using RemoteHealthcare_Server.Data.Processing;
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

    
        public string Password{get;set;}

        public DateTime DateOfBirth { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PatientID { get; set; }

        public readonly UserTypes type;



        public Patient(string username, string password, DateTime dateOfBirth, string firstName, string lastName, string medicalSystemID, bool hashable)
        {
            //Username and login
            this.Username = username;
            if (hashable) this.Password = HashProcessing.HashString(password);
            else this.Password = password;

            //Patien data
            this.DateOfBirth = dateOfBirth;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.PatientID = medicalSystemID;
            type = UserTypes.Patient;
        }

  

        public UserTypes getUserType()
        {
            return type;
        }
    }
}
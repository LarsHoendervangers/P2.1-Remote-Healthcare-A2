using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace RemoteHealthcare_Server
{
    public class FileProcessing
    {
        //************NOTE******************************

        //TODO Needs to be redone there need to come a method for saving session based on user dir wich is fine
        //but there also needs to be a list saved to the disk for users.


        public void SaveSession(Patient p)
        {
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), p.Username);

            Directory.CreateDirectory(FolderPath);                
            CreateFile(FolderPath, p.Session);
        }

        public JArray LoadSession(Session s)
        {
            string FileName = s.Patient.Username + "-" + s.StartTime;
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), s.Patient.Username, FileName);

            JArray ReadData = JArray.Parse(File.ReadAllText(FilePath));

            return ReadData;
        }

        public void CreateFile(string folderpath, Session s)
        {
            string FileName = s.Patient.Username + "-" + s.StartTime;

            JArray BikeArray = JArray.FromObject(s.BikeMeasurements);
            JArray HeartArray = JArray.FromObject(s.HRMeasurements);

            JArray Combined = new JArray(BikeArray.Union(HeartArray));

            string FilePath = Path.Combine(folderpath, FileName);

            try
            {
                File.WriteAllText(FilePath, Combined.ToString());
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                Debug.WriteLine(e.Message);
            }
        }
    }
}
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteHealthcare_Server
{
    public class FileProcessing
    {
        public void SaveSession(Patient p)
        {
            string FolderPath = Directory.GetCurrentDirectory() + "/" + p.Username;
            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
                CreateFile(FolderPath, p.Session);
            }
            else
            {
                CreateFile(FolderPath, p.Session);
            }
        }

        public JArray LoadSession(Session s)
        {
            string FilePath = Directory.GetCurrentDirectory() + "/" + s.Patient.Username + "/" + s.Patient.Username + s.StartTime;

            JArray ReadData = JArray.Parse(File.ReadAllText(FilePath));

            return ReadData;
        }

        public void CreateFile(string folderpath, Session s)
        {
            JArray BikeArray = JArray.FromObject(s.BikeMeasurements);
            JArray HeartArray = JArray.FromObject(s.HRMeasurements);

            string FilePath = folderpath + "/" + s.Patient.Username + s.StartTime;

            try
            {
                File.WriteAllText(FilePath, BikeArray.ToString());
                File.WriteAllText(FilePath, HeartArray.ToString());
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }
        }
    }
}
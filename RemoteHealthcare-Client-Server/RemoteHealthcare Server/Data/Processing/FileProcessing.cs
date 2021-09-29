using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RemoteHealthcare_Server.Data.User;
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

        public static void SaveUsers(List<IUser> users)
        {
            JArray data = JArray.FromObject(users);
            Server.PrintToGUI(data.ToString());

            try
            {
                File.WriteAllText(Directory.GetCurrentDirectory() + @"\users.txt" , data.ToString());
            } catch
            {
                Server.PrintToGUI(data.ToString());
            }


        }

        public static List<IUser> LoadUsers()
        {
            List<IUser> users = new List<IUser>();
            string data = File.ReadAllText(Directory.GetCurrentDirectory() + @"\users.txt");
            if (!File.Exists(data)) throw new Exception();

            JArray array = JArray.Parse(data);
            foreach (JObject o in array)
            {
                UserTypes? type = UserTypesUtil.Parse(int.Parse(o.GetValue("type").ToString()));
                if (type == UserTypes.Patient)
                {
                    users.Add(o.ToObject<Patient>());
                } else if (type == UserTypes.Doctor)
                {
                    users.Add(o.ToObject<Doctor>());
                } else if (type == UserTypes.Admin)
                {
                    users.Add(o.ToObject<Admin>());
                } 
            }

            return users;
        }


        public static void SaveSession(Session s)
        {
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), s.Patient.Username);

            Directory.CreateDirectory(FolderPath);                
            CreateFile(FolderPath, s);
        }

        public static JArray LoadSession(Session s)
        {
            string FileName = s.Patient.Username + "-" + s.StartTime;
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), s.Patient.Username, FileName);

            JArray ReadData = JArray.Parse(File.ReadAllText(FilePath));

            return ReadData;
        }

        public static void CreateFile(string folderpath, Session s)
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
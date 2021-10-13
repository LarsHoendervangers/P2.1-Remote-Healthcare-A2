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
        /// <summary>
        /// Saves all the users to a disk
        /// </summary>
        /// <param name="users"></param>
        public static void SaveUsers(List<IUser> users)
        {
            JArray data = JArray.FromObject(users);
            Server.PrintToGUI(data.ToString());

            try
            {
                File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory() , "users.txt") , data.ToString());
            } catch
            {
                Server.PrintToGUI(data.ToString());
            }
        }
        
        /// <summary>
        /// Reads all the users to disk
        /// </summary>
        /// <returns></returns>
        public static List<IUser> LoadUsers()
        {
            List<IUser> users = new List<IUser>();
            string data = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory() , "users.txt"));
      

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

        /// <summary>
        /// Saves the sessoin to disk
        /// </summary>
        /// <param name="s"></param>
        public static void SaveSession(Session s)
        {
            //Directory name....
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), s.Patient.Username);

            //If the dir doesn't exist
            if (!Directory.Exists(FolderPath)) Directory.CreateDirectory(FolderPath);
            
            //Creating file
            CreateFile(FolderPath, s);
        }

        /// <summary>
        /// Loads a sessoins and returns a list
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static List<Session> LoadSessions(Patient p)
        {
            //List for sessions
            List<Session> sessions = new List<Session>();

            //Path for the directory
            string dirPath = Path.Combine(Directory.GetCurrentDirectory(), p.Username);
            if (Directory.Exists(dirPath))
            {
                //Getting sessions
                foreach (string file in Directory.GetFiles(dirPath))
                {
                    try
                    {
                        string data = File.ReadAllText(file);
                        sessions.Add(JObject.Parse(data).ToObject<Session>());
                    } catch (Exception e)
                    {
                        Server.PrintToGUI(e.Message);
                    }
                }

            }

            return sessions;
        }

        /// <summary>
        /// Creates the file...
        /// </summary>
        /// <param name="folderpath"></param>
        /// <param name="s"></param>
        public static void CreateFile(string folderpath, Session s)
        {
            //Settingup variables
            JObject Jsession = JObject.FromObject(s);
            string FileName = s.Patient.Username + "-" + s.StartTime;
            string FilePath = Path.Combine(folderpath, FileName);

            //Writing it...
            try
            {
                File.WriteAllText(FilePath, Jsession.ToString());
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                Debug.WriteLine(e.Message);
            }
        }
    }
}
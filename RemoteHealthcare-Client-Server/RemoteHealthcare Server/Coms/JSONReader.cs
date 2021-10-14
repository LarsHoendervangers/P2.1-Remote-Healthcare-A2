using Newtonsoft.Json.Linq;
using RemoteHealthcare_Server.Data;
using RemoteHealthcare_Server.Data.User;
using RemoteHealthcare_Shared;
using RemoteHealthcare_Shared.DataStructs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace RemoteHealthcare_Server
{
    public class JSONReader
    {
        //Callback for the Iuser object...
        public event EventHandler<IUser> CallBack;

        /// <summary>
        /// Getting jsonobject..
        /// </summary>
        /// <param name="jObject"></param>
        /// <param name="sender"></param>
        /// <param name="user"></param>
        /// <param name="managemet"></param>
        public void DecodeJsonObject(JObject jObject, ISender sender, IUser user, UserManagement managemet)
        {
            //Checking if it is safe...
            JToken token;
            if (jObject != null && jObject.TryGetValue("command", out token))
            {
                //Getting command
                string command = token.ToString();

                //Going to method with reflection
                MethodInfo[] methods = typeof(JSONReader).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.ExactBinding);
                foreach (MethodInfo method in methods)
                {
                    //This if could probably be short but this is much clearer
                    if (method.GetCustomAttribute<AccesManagerAttribute>() != null && method.GetCustomAttribute<AccesManagerAttribute>().GetCommand() == command
                         && (user != null && method.GetCustomAttribute<AccesManagerAttribute>().GetUserType() == user.getUserType() ||
                             user == null && method.GetCustomAttribute<AccesManagerAttribute>().GetUserType() == UserTypes.Unkown))
                    {
                        method.Invoke(this, new object[] { jObject, sender, user, managemet });
                    }
                }
            }
        }

        /// <summary>
        /// Login function
        /// </summary>
        /// <param name="Jobject">The object send</param>
        /// <param name="sender">The receiver and sender</param>
        /// <param name="management">The management object</param>
        [AccesManager("login", UserTypes.Unkown)]
        private void LoginAction(JObject Jobject, ISender sender, IUser u, UserManagement management)
        {
            JToken username = Jobject.SelectToken("data.us");
            JToken password = Jobject.SelectToken("data.pass");
            JToken flag = Jobject.SelectToken("data.flag");
            if (username != null && password != null && flag != null)
            {
                //Getting the user
                IUser user = management.Credentials(username.ToString(), password.ToString(), int.Parse(flag.ToString()));
                if (user != null)
                {
                    JSONWriter.LoginWrite(true, sender);
                    Server.PrintToGUI("[Login debug] - " + username.ToString()+ " has logged on to the server.");
                    CallBack?.Invoke(this, user);
                    return;
                }
                else
                {
                    JSONWriter.LoginWrite(false, sender);
                    Server.PrintToGUI("[Login debug] - The following request was not a valid user.");
                    return;
                }
            }
            return;
        }

        /// <summary>
        /// This method does save and send data to subs and session.
        /// </summary>
        /// <param name="Jobject"></param>
        /// <param name="sender"></param>
        /// <param name="user"></param>
        /// <param name="usermanagement"></param>
        [AccesManager("ergodata", UserTypes.Patient)]
        private void ReceiveMeasurement(JObject Jobject, ISender sender, IUser user, UserManagement usermanagement)
        {
            //All data from the jobject
            JToken rpm = Jobject.SelectToken("data.rpm");
            JToken speed = Jobject.SelectToken("data.speed");
            JToken dist = Jobject.SelectToken("data.dist");
            JToken pow = Jobject.SelectToken("data.pow");
            JToken accpow = Jobject.SelectToken("data.accpow");
            JToken bpm = Jobject.SelectToken("data.bpm");
            JToken time = Jobject.SelectToken("data.time");

            //Getting sessoin
            Session session = null;
            if (rpm != null)
            {
                session =  usermanagement.SessionUpdateBike(int.Parse(rpm.ToString()),
                    (int)double.Parse(speed.ToString()), (int)double.Parse(dist.ToString()), int.Parse(pow.ToString()),
                    int.Parse(accpow.ToString()), DateTime.Parse(time.ToString()), user);
            }
            else if (bpm != null)
            {
                session =  usermanagement.SessionUpdateHRM(DateTime.Parse(time.ToString()), int.Parse(bpm.ToString()), user);
            }

            //Sending it to the subs
            if (session != null)
            {
                Server.PrintToGUI("[Session debug] - Data is added to a session");
                Patient p = user as Patient;
                List<Doctor> subs = session.Subscribers;
                foreach(Doctor d in subs)
                {
                    Host h = usermanagement.FindHost(d);
                    if (h != null)
                    {
                        JSONWriter.DoctorSubWriter(h, session, p.PatientID, h.GetSender());
                    }
                }
            }
        }

        /// <summary>
        /// Sends the resistance from the doctor to the client.
        /// </summary>
        /// <param name="jObject">This is the JSON-file to decode were the data is stored</param> 
        /// <param name="sender">This is the sender for sending it to the client</param>
        /// <param name="user">This is the adress for sending it.</param>
        /// <param name="managemet">This is a managment object.</param>
        [AccesManager("setresist", UserTypes.Doctor)]
        private void SettingErgometer(JObject jObject, ISender sender, IUser user, UserManagement managemet)
        {
            //Getting data
            JToken patientIDs = jObject.SelectToken("data.patid");
            JToken resitance = jObject.SelectToken("data.resistance");
            if (patientIDs != null && resitance != null)
            {
                //Getting patients
                Server.PrintToGUI("[Session debug] - Request for changing resistance");
                List<Patient> targetPatients = new List<Patient>();
                foreach (JObject patientID in (JArray)patientIDs)
                {
                    Host h = managemet.FindHost(patientID.ToString());
                    JSONWriter.ResistanceWrite(int.Parse(resitance.ToString()), h.GetSender());
                }
            }
        }

        /// <summary>
        /// Sends abort to the request clients..
        /// </summary>
        /// <param name="jObject"></param>
        /// <param name="sender"></param>
        /// <param name="user"></param>
        /// <param name="managemet"></param>
        [AccesManager("abort", UserTypes.Doctor)]
        private void AbortingClient(JObject jObject, ISender sender, IUser user, UserManagement managemet)
        {
            //Getting data
            JToken patientIDs = jObject.SelectToken("data.patid");
            if (patientIDs != null)
            {
                Server.PrintToGUI("[Session debug] - Aborting patients");
                //Getting patients
                List<Patient> targetPatients = new List<Patient>();
                foreach (JObject patientID in (JArray)patientIDs)
                {
                    Host h = managemet.FindHost(patientID.ToString());
                    JSONWriter.AbortWrite(h.GetSender());
                }
            }
        }


        /// <summary>
        /// This method gives back all patient ids
        /// </summary>
        /// <param name="jObject"></param>
        /// <param name="sender"></param>
        /// <param name="user"></param>
        /// <param name="managemet"></param>
        [AccesManager("getallpatients", UserTypes.Doctor)]
        private void GetAllClients(JObject jObject, ISender sender, IUser user, UserManagement managemet)
        {
            //Sending patient IDS back
            Server.PrintToGUI("[Logic debug] - Getting all patients");
            JSONWriter.AllPatientWrite(managemet.GetAllPatients(), sender);
        }

        /// <summary>
        /// This method gives all active patients
        /// </summary>
        /// <param name="jObject">command</param>
        /// <param name="sender">for sending response</param>
        /// <param name="user">as of the type that requested</param>
        /// <param name="managemet">that controls everthing related to users</param>
        [AccesManager("getactivepatients", UserTypes.Doctor)]
        private void GetActiveClients(JObject jObject, ISender sender, IUser user, UserManagement managemet)
        {
            //Sending patient IDS back
            Server.PrintToGUI("[Logic debug] - Getting active patients");
            JSONWriter.ActivePatientWrite(managemet.GetActivePatients(), sender);
        }

        /// <summary>
        /// Subscribes to a patient..
        /// </summary>
        /// <param name="jObject"></param>
        /// <param name="sender"></param>
        /// <param name="user"></param>
        /// <param name="managemet"></param>
        [AccesManager("subtopatient", UserTypes.Doctor)]
        private void SubscribeToLiveSession(JObject jObject, ISender sender, IUser user, UserManagement management)
        {
            JToken patientIDs = jObject.SelectToken("data.patid");
            JToken subscribeState = jObject.SelectToken("data.state");
            if (patientIDs != null && subscribeState != null)
            {
                //Getting patient IDs..
                List<string> patientIdentiefiers = new List<string>();
                foreach (JObject patientID in (JArray)patientIDs)
                {
                    patientIdentiefiers.Add(patientID.ToString());
                }

                //Getting state
                bool state = int.Parse(subscribeState.ToString()) == 0 ? true : false;

                //Subs or unsubing...
                if (user.getUserType() == UserTypes.Doctor) {
                    Doctor d = user as Doctor;
                    if (state) { management.Subscribe(d, patientIdentiefiers); Server.PrintToGUI("[Session debug] - Subbing to session"); }
                    else { management.Unsubscribe(d, patientIdentiefiers); Server.PrintToGUI("[Session debug] - Unsubbing to session"); }
                }        
            }
        }
            
        /// <summary>
        /// Startin a new session..
        /// </summary>
        /// <param name="jObject"></param>
        /// <param name="sender"></param>
        /// <param name="user"></param>
        /// <param name="management"></param>
        [AccesManager("newsession", UserTypes.Doctor)]
        private void StartNewSession(JObject jObject, ISender sender, IUser user, UserManagement management)
        {
            
            //Getting data
            JToken patientIDs = jObject.SelectToken("data.patid");
            JToken sessionState = jObject.SelectToken("data.state");

            //Verifying and reading it.
            if (patientIDs != null && sessionState != null)
            {
                //Getting patients
                List<Patient> targetPatients = new List<Patient>();
                //Afvangen cast.....
                foreach(string patientID in patientIDs)
                {
                    Patient p = management.FindPatient(patientID);
                    if (p != null) targetPatients.Add(p);
                }

                //Getting state
                bool state = int.Parse(sessionState.ToString()) == 0 ? true : false;

                //Executing action
                foreach(Patient p in targetPatients)
                {
                    if (state) { management.SessionStart(user); Server.PrintToGUI("[Session debug] - Starting to patient"); }
                    else { management.SessionEnd(user); Server.PrintToGUI("[Logic debug] - Stopping to patient"); }
                }
            }
        }

        /// <summary>
        /// Getting detailed patient data...
        /// </summary>
        /// <param name="jObject"></param>
        /// <param name="sender"></param>
        /// <param name="user"></param>
        /// <param name="management"></param>
        [AccesManager("getdetailpatient", UserTypes.Doctor)]
        private void GettingDetails(JObject jObject, ISender sender, IUser user, UserManagement management)
        {

          
            JToken patientIDs = jObject.SelectToken("data");
            if (patientIDs != null)
            {
                //Getting patient IDs..
                List<string> patientIdentiefiers = new List<string>();
                foreach (string patientID in (JArray)patientIDs)
                {
                    patientIdentiefiers.Add(patientID);
                   
                }



                //Getting detailed data
                Server.PrintToGUI("[Logic debug] - Getting details from patients");
                List<SharedPatient> patients = new List<SharedPatient>();
                foreach (string id in patientIdentiefiers)
                {
                    if (management.FindPatient(id) != null)
                    {
                        Patient serverPatient = management.FindPatient(id);
                        SharedPatient sharedPatient = 
                            new SharedPatient(serverPatient.FirstName,
                            serverPatient.LastName, serverPatient.PatientID,
                            management.FindSessoin(serverPatient),  
                            serverPatient.DateOfBirth);

                        patients.Add(sharedPatient);
                   
                    }
                }

                //Sending patients over..
                JSONWriter.SendDetails(patients, sender);


            }
        }

        //Sends back all the session of a patient...
        [AccesManager("getsessions", UserTypes.Doctor)]
        private void GettingSessions(JObject jObject, ISender sender, IUser user, UserManagement management)
        {
            JToken patientID = jObject.SelectToken("data");
            if (patientID != null)
            {
                Server.PrintToGUI("[Logic debug] - Getting sessions history");
                string id = patientID.ToString();
                Patient p = management.FindPatient(id);

                if (p != null)
                {
                    List<Session> sessoins = FileProcessing.LoadSessions(p);

                    JSONWriter.HistoryWrite(sender, sessoins, p.PatientID);
                }
            }
        }

        /// <summary>
        /// Message funtion to vr engine.
        /// </summary>
        /// <param name="jObject"></param>
        /// <param name="sender"></param>
        /// <param name="user"></param>
        /// <param name="management"></param>
        [AccesManager("message", UserTypes.Doctor)]
        private void ResendMessage(JObject jObject, ISender sender, IUser user, UserManagement management)
        {
            JToken message = jObject.SelectToken("data.message");
            JToken patids = jObject.SelectToken("data.patid");
            if (message != null)
            {
                Server.PrintToGUI("[Logic debug] - message send");
                //If there are no patient ids
                if (patids == null) JSONWriter.WriteMessage(message.ToString(), management.activeHosts.Where(host => host.GetUser() != null && host.GetUser().getUserType() == UserTypes.Patient).ToList());
                //If there are....
                else
                {
                    //Getting targets
                    List<Host> targets = new List<Host>();
                    foreach (string id in (JArray)patids)
                    {
                        targets.Add( management.FindHost(id));
                    }

                    //Sending it over
                    JSONWriter.WriteMessage(message.ToString(), targets);
                }
            }
        }


    }

    /// <summary>
    /// This is the custom attribute used for jumping to the correct method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AccesManagerAttribute : Attribute
    {
        string command;
        UserTypes type;

        public AccesManagerAttribute(string command, UserTypes type)
        {
            this.command = command;
            this.type = type;
        }

        public string GetCommand()
        {
            return command;
        }

        public UserTypes GetUserType()
        {
            return type;
        }
    }
}
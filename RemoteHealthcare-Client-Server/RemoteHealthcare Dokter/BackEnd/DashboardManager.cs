using Newtonsoft.Json.Linq;
using RemoteHealthcare_Client;
using RemoteHealthcare_Dokter.ViewModels;
using RemoteHealthcare_Shared.DataStructs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RemoteHealthcare_Dokter.BackEnd
{
    class DashboardManager : DataManager
    {
        public event EventHandler<List<SharedPatient>> OnPatientUpdated;
        private DashboardViewModel model;
        public bool running { get; set; }


        public DashboardManager(DashboardViewModel model)
        {
            // Oncreation the server wil ask the server for all active clients
            running = true;



            //Crappy fix for updating..
            new Thread(() =>
            {
                while (running) 
                {
                    RequestActiveClients();
                    Thread.Sleep(5000);
                }
            }).Start();
        }

        /// <summary>
        /// Method which checks whether a command is valid and calls the right method belong to the command
        /// </summary>
        /// <param name="data"></param>
        public override void ReceivedData(JObject data)
        {
            JToken value;

            bool correctCommand = data.TryGetValue("command", StringComparison.InvariantCulture, out value);

            // Return if the parsing of command was not succesfull
            if (!correctCommand) return;
           
            switch (value.ToString())
            {
                case "getactivepatients":
                    // Setting the command to the command to ask for detailed data and sending to server
                    data["command"] = "getdetailpatient";
                    SendToManagers(data);
                    break;
                case "detaildata":
                    ParseIncomingPatients(data);
                    break;

            }
        }

        /// <summary>
        /// Method which creates an object to send to  the manager to get all the active clients in the server
        /// </summary>
        public void RequestActiveClients()
        {
            // Command to request all the logged in clients, see dataprotocol
            object o = new
            {
                command = "getactivepatients",
            };

            SendToManagers(JObject.FromObject(o));
        }

        /// <summary>
        /// Method which handles the object which is returned from the server when a request for all the clients has been send.
        /// Creates users from a JArray and adds them to a list in the DashboardViewModel
        /// </summary>
        /// <param name="data"></param>
        private void ParseIncomingPatients(JObject data)
        {
            JArray patientIDs = data.GetValue("data") as JArray;

            List<SharedPatient> patients = new List<SharedPatient>();
            foreach(JObject jo in patientIDs)
            {
                patients.Add(jo.ToObject<SharedPatient>());
            }

            this.OnPatientUpdated.Invoke(this, patients);
        }

        /// <summary>
        ///Method which send an object to all the managers to send a message to all the active clients
        /// </summary>
        /// <param name="message"></param>
        public void BroadcastMessage(string message)
        {
            object o = new
            {
                command = "message",
                data = new
                {
                    message = message
                },
                flag = "2"
            };

            this.SendToManagers(JObject.FromObject(o));
        }

        /// <summary>
        /// Method which send an object to all the managers to sub to a patient and get all the live data 
        /// corresponding to the patient
        /// </summary>
        /// <param name="patient"></param>
        public void StartSession(SharedPatient patient)
        {
            string[] patients = new string[] { patient.ID };

            // JSON object to start a new session
            object o = new
            {
                command = "newsession",
                data = new
                {
                    patid = patients,
                    state = 0
                }
            };

            //Asking the sever to start a session, no further actions are taken until new userineraction
            this.SendToManagers(JObject.FromObject(o));
        }
    }
}

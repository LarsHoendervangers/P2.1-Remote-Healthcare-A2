using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RemoteHealthcare_Server;
using RemoteHealthcare_Server.Data;
using RemoteHealthcare_Server.Data.Processing;
using RemoteHealthcare_Server.Data.User;
using RemoteHealthcare_Shared;
using System;

namespace UnitTestServer
{

    [TestClass]
    public class DecodeTest
    {
        /// <summary>
        /// Tests the login 
        /// </summary>
        [TestMethod]
        public void Test_Login_TestUser_Correct()
        {
            // Arrange
            //Data for the test.......
            object o;
            o = new
            {
                command = "message",
                data = "succesfull connect",
                flag = 1
            };
            object m = new
            {
                command = "login",
                data = new
                {
                    us = "TestUser",
                    pass = "TestPassword",
                    flag = 0
                }
            };

            //Functions needed for test...
            IUser user = null;
            JSONReader reader = new JSONReader();
            UnitSender sender = new UnitSender(JsonConvert.SerializeObject(o));
            UserManagement management = new UserManagement();
            UserManagement.users.Add(
               new Patient("TestUser", "TestPassword",
               DateTime.Now, null, null, null, true));

            //Act
            sender.CheckCallback += (s, b) => Assert.IsTrue(b);
            reader.DecodeJsonObject(JObject.FromObject(m), sender, user, management);

            // Assert
            reader.CallBack += (s, u) =>
            {
                user = u;
                bool correct = false;
                Patient p = (Patient)user;
                if (p != null)
                {
                    correct = p.Username == "TestUser";
                }

                Assert.IsTrue(correct);
            };
        }

        [TestMethod]
        public void Test_Login_TestUser_Incorrect()
        {
            // Arrange
            //Data for the test.......
            object o;
            o = new
            {
                command = "message",
                data = "failed connect",
                flag = 1
            };
            object m = new
            {
                command = "login",
                data = new
                {
                    us = "TestUser",
                    pass = " ",
                    flag = 0
                }
            };

            //Functions needed for test...
            IUser user = null;
            JSONReader reader = new JSONReader();
            UnitSender sender = new UnitSender(JsonConvert.SerializeObject(o));
            UserManagement management = new UserManagement();
            UserManagement.users.Add(
               new Patient("TestUser", "TestPassword",
               DateTime.Now, null, null, null, true));

            //Act
            sender.CheckCallback += (s, b) => Assert.IsTrue(b);
            reader.DecodeJsonObject(JObject.FromObject(m), sender, user, management);

            // Assert
            reader.CallBack += (s, u) => Assert.IsTrue(u == null);
        }

        [TestMethod]
        public void Test_Login_Incorrect_Format()
        {
            // Arrange
            //Data for the test.......
            object m = new
            {
                command = "login",
                data = new
                {
                    pass = " ",
                    flag = 0

                }
            };

            //Functions needed for test...
            IUser user = null;
            JSONReader reader = new JSONReader();
            UnitSender sender = new UnitSender(JsonConvert.SerializeObject(m));
            UserManagement management = new UserManagement();
            UserManagement.users.Add(
               new Patient("TestUser", "TestPassword",
               DateTime.Now, null, null, null, true));

            //Act
            reader.DecodeJsonObject(JObject.FromObject(m), sender, user, management);

            // Assert
            reader.CallBack += (s, u) => Assert.IsTrue(u == null);
        }








}
}

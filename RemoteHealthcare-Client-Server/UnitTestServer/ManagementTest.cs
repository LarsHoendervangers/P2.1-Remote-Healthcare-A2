using Microsoft.VisualStudio.TestTools.UnitTesting;
using RemoteHealthcare_Server;
using RemoteHealthcare_Server.Data;
using System;

namespace UnitTestServer
{
    [TestClass]
    public class ManagementTest
    {
        [TestMethod]
        public void Test_Management_PatFinder_Succeed()
        {
            //Arrange
            UserManagement testManager = new UserManagement();
            Patient testPatient = new Patient("TestUser",
                "TestPassword", DateTime.Now, null, null, "TestID", false);
            UserManagement.users.Add(testPatient);

            //Act
            bool passed = testManager.FindPatient(testPatient.PatientID) == testPatient;

            //Assert
            Assert.IsTrue(passed);
        }

        [TestMethod]
        public void Test_Management_HostFinder_Failed()
        {
            //Arrange
            UserManagement testManager = new UserManagement();
            Patient testPatient = new Patient("TestUser",
                "TestPassword", DateTime.Now, null, null, "TestID", false);
            UserManagement.users.Add(testPatient);

            
            //Act
            bool passed = testManager.FindHost(testPatient.PatientID) == null;

            //Assert
            Assert.IsTrue(passed);
        }

        [TestMethod]
        public void Test_Management_AllPatient()
        {
            //Arrange
            UserManagement testManager = new UserManagement();
            UserManagement.users.Clear();
            Patient testPatient = new Patient("TestUser",
                "TestPassword", DateTime.Now, null, null, "TestID", false);
            UserManagement.users.Add(testPatient);

            //Act
            bool passed = testManager.GetAllPatients().Count == 1;

            //Assert
            Assert.IsTrue(passed);
        }

        [TestMethod]
        public void Test_Management_SessionAdd_Succeed()
        {
            //Arrange
            UserManagement testManager = new UserManagement();
            UserManagement.users.Clear();
            Patient testPatient = new Patient("TestUser",
                "TestPassword", DateTime.Now, null, null, "TestID", false);
            UserManagement.users.Add(testPatient);


            //Act
            testManager.SessionStart(testPatient);
            bool passed = UserManagement.activeSessions.Count == 1;

            //Assert
            Assert.IsTrue(passed);
        }

        public void Test_Management_SessionEnd_Failed()
        {
            //Arrange
            UserManagement testManager = new UserManagement();
            UserManagement.users.Clear();
            Patient testPatient = new Patient("TestUser",
                "TestPassword", DateTime.Now, null, null, "TestID", false);
            UserManagement.users.Add(testPatient);


            //Act
            testManager.SessionEnd(testPatient);
            bool passed = UserManagement.activeSessions.Count == 0;

            //Assert
            Assert.IsTrue(passed);
        }




    }
}

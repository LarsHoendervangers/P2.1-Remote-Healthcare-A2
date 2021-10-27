using Microsoft.VisualStudio.TestTools.UnitTesting;
using RemoteHealthcare_Server;
using RemoteHealthcare_Server.Data.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestServer
{
    [TestClass]
    public class FileProcessingTest
    {
        [TestMethod]
        public void Test_FileProcessing_ReadAndWrite()
        {
            //Arrange
            List<IUser> users = new List<IUser>();
            List<string> usernamesActual = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                users.Add(new Patient("blabla" + i, "test", DateTime.Now, null, null, null, true));
                usernamesActual.Add("blabla" + i);
            }

            //Act
            FileProcessing.SaveUsers(users);
            List<IUser> returnvalue = FileProcessing.LoadUsers();
            List<string> usernamesValue = new List<string>();
            
            //Arrange
            Assert.AreEqual(users.Count, returnvalue.Count);
            File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "users.txt"));
        }
    }
}

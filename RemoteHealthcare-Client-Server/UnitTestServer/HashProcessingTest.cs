using Microsoft.VisualStudio.TestTools.UnitTesting;
using RemoteHealthcare_Server.Data.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestServer
{
    [TestClass]
    public class HashProcessingTest
    {
        [TestMethod]
        public void Test_HashProcessor_Correct()
        {
            //Arrange
            SHA256 shaM = new SHA256Managed();
            string encoded = BitConverter.ToString(shaM.ComputeHash(Encoding.UTF32.GetBytes("test")));

            //Act
            string value = HashProcessing.HashString("test");

            //Arrange
            Assert.AreEqual(encoded, value);
        }


        [TestMethod]
        public void Test_HashProcessor_InCorrect()
        {
            //Arrange
            SHA256 shaM = new SHA256Managed();
            string encoded = BitConverter.ToString(shaM.ComputeHash(Encoding.UTF32.GetBytes("test")));

            //Act
            string value = HashProcessing.HashString("not equal");

            //Arrange
            Assert.AreNotEqual(encoded, value);
        }



    }
}

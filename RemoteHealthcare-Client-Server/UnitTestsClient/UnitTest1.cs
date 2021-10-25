using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using RemoteHealthcare_Client;

namespace UnitTestsClient
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            ClientViewModel clientViewModel = new ClientViewModel(new StartupLoader());

            bool isLoggedIn = clientViewModel.isLoggedIn;

            Assert.AreEqual(isLoggedIn, false, "Wrong logged in information");
        }
    }
}

using System;
using RemoteHealthcare_Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    class TestUnitTest
    {
        ClientViewModel clientViewModel = new ClientViewModel(new StartupLoader());
    }
}

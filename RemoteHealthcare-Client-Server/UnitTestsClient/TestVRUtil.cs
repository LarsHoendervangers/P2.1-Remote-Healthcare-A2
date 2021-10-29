using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RemoteHealthcare.ClientVREngine.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestsClient
{
    [TestClass]
    public class TestVRUtil
    {
        [TestMethod]
        public void Test_GetId_Success()
        {
            // Arrange
            string id = Guid.NewGuid().ToString();
            string expected = id;
            string json = JsonConvert.SerializeObject(new
            {
                data = new
                {
                    data = new
                    {
                        data = new
                        {
                            uuid = id
                        }
                    }
                }
            });

            // Act
            string result = VRUTil.GetId(json);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Test_GenerateTerrain_Success()
        {
            // Arrange
            int expected = 25;

            // Act
            float[] terrain = VRUTil.GenerateTerrain(5, 5, 10, 2);

            // Assert
            Assert.IsTrue(terrain.Length == expected);
        }
    }
}

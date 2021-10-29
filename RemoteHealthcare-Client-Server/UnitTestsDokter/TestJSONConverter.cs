using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RemoteHealthcare_Dokter.BackEnd;
using RemoteHealthcare_Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestsDokter
{
    [TestClass]
    public class TestJSONConverter
    {
        [TestMethod]
        public void Test_ConvertBikeObject_Succes()
        {
            // Arrange
            DateTime dateTime = DateTime.Now;
            int currentRPM = 45;
            double currentSpeed = 39.34;
            double currentWattage = 135.23;
            int currentTotalWattage = 5434;
            int currentTotalDistance = 34542;
            JObject jObject = new JObject();
            jObject.Add("", dateTime);

            BikeMeasurement expected = new BikeMeasurement(
                dateTime, 
                currentRPM, 
                currentSpeed, 
                currentWattage, 
                currentTotalWattage, 
                currentTotalDistance);

            string value = JsonConvert.SerializeObject(expected);

            // Act
            BikeMeasurement result = JSONConverter.ConverBikeObject(JObject.Parse(value));

            // Assert
            bool areEqual = false;
        }
    }
}

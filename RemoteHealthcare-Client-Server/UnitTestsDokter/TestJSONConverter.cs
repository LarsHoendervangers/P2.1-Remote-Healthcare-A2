using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RemoteHealthcare_Dokter.BackEnd;
using RemoteHealthcare_Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            DateTime dateTime = new DateTime(2021, 1, 1);
            int currentRPM = 45;
            double currentSpeed = 39.34;
            double currentWattage = 135.23;
            int currentTotalWattage = 5434;
            int currentTotalDistance = 34542;

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
            if (
                expected.CurrentRPM == result.CurrentRPM
                && expected.CurrentSpeed == result.CurrentSpeed
                && expected.CurrentWattage == result.CurrentWattage
                && expected.CurrentTotalDistance == result.CurrentTotalDistance
                && expected.CurrentTotalWattage == result.CurrentTotalWattage
                ) areEqual = true;
            // Cannot use AreEqual on the objects, because DateTime has slightly different ticks.
            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void Test_ConvertHRObject_Succes()
        {
            // Arrange
            DateTime dateTime = new DateTime(2021, 1, 1);
            int currentHeartrate = 45;

            HRMeasurement expected = new HRMeasurement(dateTime, currentHeartrate);

            string value = JsonConvert.SerializeObject(expected);

            // Act
            HRMeasurement result = JSONConverter.ConvertHRObject(JObject.Parse(value));

            // Assert
            bool areEqual = false;
            if (result.CurrentHeartrate == expected.CurrentHeartrate) areEqual = true;
            // Cannot use AreEqual on the objects, because DateTime has slightly different ticks.
            Assert.IsTrue(areEqual);
        }
    }
}

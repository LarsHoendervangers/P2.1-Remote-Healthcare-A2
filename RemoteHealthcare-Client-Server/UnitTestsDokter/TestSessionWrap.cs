using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class TestSessionWrap
    {
        [TestMethod]
        public void Test_SessionWrapToString_Success()
        {
            // Arrange
            List<HRMeasurement> hrMeasurements = new List<HRMeasurement>();
            hrMeasurements.Add(new HRMeasurement(DateTime.Now, 115));
            List<BikeMeasurement> bikeMeasurements = new List<BikeMeasurement>();
            bikeMeasurements.Add(new BikeMeasurement(DateTime.Now, 20, 20, 2, 1240, 12122));

            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now.AddMinutes(34);

            SessionWrap sw = new SessionWrap(hrMeasurements, bikeMeasurements, start, end);
            string expected = $"{start.ToString("dddd d MMMM yyyy hh:mm:ss")} - {end.Subtract(start).Minutes} minuten";

            // Act
            string result = sw.ToString();

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}

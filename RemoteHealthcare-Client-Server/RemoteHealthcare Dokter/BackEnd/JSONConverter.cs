using Newtonsoft.Json.Linq;
using RemoteHealthcare_Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Dokter.BackEnd
{
    public static class JSONConverter
    {
        /// <summary>
        /// Method which Creates a BikeMeasurement from the JObject in the parameters of the method
        /// </summary>
        /// <param name="dataObject"></param>
        /// <returns></returns>
        public static BikeMeasurement ConverBikeObject(JObject dataObject)
        {
            return new BikeMeasurement(
                DateTime.Parse(dataObject.GetValue("MeasurementTime").ToString()),
                int.Parse(dataObject.GetValue("CurrentRPM").ToString()),
                double.Parse(dataObject.GetValue("CurrentSpeed").ToString()),
                double.Parse(dataObject.GetValue("CurrentWattage").ToString()),
                int.Parse(dataObject.GetValue("CurrentTotalWattage").ToString()),
                int.Parse(dataObject.GetValue("CurrentTotalDistance").ToString())
                );
        }

        /// <summary>
        /// Method which Creates a HRMeasurement from the JObject in the parameters of the method
        /// </summary>
        /// <param name="dataObject"></param>
        /// <returns></returns>
        public static HRMeasurement ConvertHRObject(JObject dataObject)
        {
            return new HRMeasurement(
                DateTime.Parse(dataObject.GetValue("MeasurementTime").ToString()),
                int.Parse(dataObject.GetValue("CurrentHeartrate").ToString())
                );
        }
    }
}

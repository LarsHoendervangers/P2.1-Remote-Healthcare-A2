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

        public static HRMeasurement ConvertHRObject(JObject dataObject)
        {
            return new HRMeasurement(
                DateTime.Parse(dataObject.GetValue("MeasurementTime").ToString()),
                int.Parse(dataObject.GetValue("CurrentHeartrate").ToString())
                );
        }

    }
}

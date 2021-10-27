using RemoteHealthcare_Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Dokter.BackEnd
{
    class SessionWrap
    {

        public List<HRMeasurement> HRMeasurements;
        public List<BikeMeasurement> BikeMeasurements;

        public DateTime Startdate;
        public DateTime Enddate;

        public SessionWrap(List<HRMeasurement> hRMeasurements, List<BikeMeasurement> bikeMeasurements, DateTime startdate, DateTime enddate)
        {
            HRMeasurements = hRMeasurements;
            BikeMeasurements = bikeMeasurements;
            Startdate = startdate;
            Enddate = enddate;
        }
    }
}

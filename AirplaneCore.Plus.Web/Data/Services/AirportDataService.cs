using AirplaneCore.Plus.Web.Cores;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;

namespace AirplaneCore.Plus.Web.Data.Services
{
    public class AirportDataService
    {
        private readonly IHostingEnvironment _env;

        public AirportDataService(IHostingEnvironment env)
        {
            this._env = env;
        }

        public IEnumerable<AirportData> GetAll()
        {
            var f = Path.Combine(this._env.WebRootPath, "data", "airports-extended.dat");
            var csv = new CsvReader(new FileStream(f, FileMode.Open));

            var lines = new List<AirportData>();
            foreach (string[] colsPerLine in csv.RowEnumerator)
            {
                lines.Add(MappingAirportData(colsPerLine));
            }
            return lines;
        }

        private static AirportData MappingAirportData(string[] cols)
        {
            var data = new AirportData
            {
                AirportId = Convert.ToInt32(cols[0]),
                Name = CheckValueIsNullValue(cols[1]) ? string.Empty : cols[1],
                City = CheckValueIsNullValue(cols[2]) ? string.Empty : cols[2],
                Country = CheckValueIsNullValue(cols[3]) ? string.Empty : cols[3],
                IATA = CheckValueIsNullValue(cols[4]) ? string.Empty : cols[4],
                ICAO = CheckValueIsNullValue(cols[5]) ? string.Empty : cols[5],
                Latitude = CheckValueIsNullValue(cols[6]) ? (double?)null : Convert.ToDouble(cols[6]),
                Longitude = CheckValueIsNullValue(cols[7]) ? (double?)null : Convert.ToDouble(cols[7]),
                Altitude = CheckValueIsNullValue(cols[8]) ? (double?)null : Convert.ToDouble(cols[8]),
                Timezone = CheckValueIsNullValue(cols[9]) ? (double?)null : Convert.ToDouble(cols[9]),
                DST = CheckValueIsNullValue(cols[10]) ? string.Empty : cols[10],
                TzDatabaseTimezone = CheckValueIsNullValue(cols[11]) ? string.Empty : cols[11],
                Type = CheckValueIsNullValue(cols[12]) ? string.Empty : cols[12],
                Source = CheckValueIsNullValue(cols[13]) ? string.Empty : cols[13]
            };
            return data;
        }

        private static bool CheckValueIsNullValue(string data)
        {
            return data.ToUpper() == "\\N";
        }
    }
}

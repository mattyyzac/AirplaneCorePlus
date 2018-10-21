namespace AirplaneCore.Plus.Web.Data
{
    public class AirportData
    {
        public int AirportId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string IATA { get; set; }
        public string ICAO { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? Altitude { get; set; }
        public double? Timezone { get; set; }
        public string DST { get; set; }
        public string TzDatabaseTimezone { get; set; }
        public string Type { get; set; }
        public string Source { get; set; }
    }
}
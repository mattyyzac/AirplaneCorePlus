namespace AirplaneCore.Plus.Web.Data
{
    public class AppSettings
    {
        public GooglePlaceApi GooglePlaceApi { get; set; }
        public Secrets Secrets { get; set; }
    }

    public class GooglePlaceApi
    {
        public string Nearbysearch { get; set; }
        public string Detail { get; set; }
        public string Photo { get; set; }
    }
}
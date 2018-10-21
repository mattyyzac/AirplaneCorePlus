namespace AirplaneCore.Plus.Web.Data
{
    public class Secrets
    {
        public Mapbox Mapbox { get; set; }
        public Google Google { get; set; }
    }
    public class Mapbox
    {
        public string AccessToken { get; set; }
    }
    public class Google
    {
        public string ApiKey { get; set; }
    }
}
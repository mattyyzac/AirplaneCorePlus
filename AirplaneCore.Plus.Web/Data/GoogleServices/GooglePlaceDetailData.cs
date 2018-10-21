namespace AirplaneCore.Plus.Web.Data.GoogleServices
{
    public class GooglePlaceDetailData
    {
        public string[] html_attributions { get; set; }
        public Dresult result { get; set; }
        public string status { get; set; }
    }
    public class Dresult
    {
        // only grab info needed

        public string adr_address { get; set; }
        public string formatted_address { get; set; }
        public string formatted_phone_number { get; set; }
        public string international_phone_number { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string vicinity { get; set; }
        public string website { get; set; }
    }
}
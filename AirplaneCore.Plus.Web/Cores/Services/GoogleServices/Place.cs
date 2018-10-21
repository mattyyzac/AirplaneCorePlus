using AirplaneCore.Plus.Web.Data.GoogleServices;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace AirplaneCore.Plus.Web.Cores.Services.GoogleServices
{
    public class Place
    {
        private readonly IConfiguration _config;
        private readonly Strings _strings;
        private readonly string _gkey;
        private readonly HttpClient _http;

        public Place(IConfiguration config)
        {
            this._config = config;
            this._strings = new Strings(this._config);
            this._gkey = this._strings.Secrets.Google.ApiKey;
            this._http = new HttpClient();
        }

        public async Task<GooglePlaceData> Nearbysearch(double lng, double lat, string name, string lang)
        {
            return await Nearbysearch(lng, lat, name, 1000, "airport", lang);
        }

        public async Task<GooglePlaceData> Nearbysearch(double lng, double lat, string name, int radius, string type, string lang)
        {
            var api = string.Format(this._strings.AppSettings?.GooglePlaceApi.Nearbysearch, this._gkey, lat, lng, radius, name, type, lang);
            var jsonStr = await this._http.GetStringAsync(api);
            var data = JsonConvert.DeserializeObject<GooglePlaceData>(jsonStr);
            return data;
        }

        public async Task<GooglePlaceDetailData> Detail(string placeid, string lang)
        {
            var api = string.Format(this._strings.AppSettings?.GooglePlaceApi.Detail, this._gkey,  placeid, lang);
            var jsonStr = await this._http.GetStringAsync(api);
            var data = JsonConvert.DeserializeObject<GooglePlaceDetailData>(jsonStr);
            return data;
        }

        public async Task<string> Photo(string photoreference, int maxWidth)
        {
            var api = string.Format(this._strings.AppSettings?.GooglePlaceApi.Photo, this._gkey, photoreference, maxWidth);
            var strm = await this._http.GetStreamAsync(api);
            var base64Str = System.Convert.ToBase64String(StreamToBytes(strm));
            return base64Str;
        }
        private static byte[] StreamToBytes(System.IO.Stream stream)
        {
            var ret = new byte[] { };
            using (var memoryStream = new System.IO.MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

    }
}
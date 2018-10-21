using AirplaneCore.Plus.Web.Data.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace AirplaneCore.Plus.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _env;

        public IndexModel(IConfiguration config, IHostingEnvironment env)
        {
            this._config = config;
            this._env = env;
        }

        public string MapboxAccessToken;
        public string GooglePlaceApiKey;


        public void OnGet()
        {
            var secrets = new Cores.Strings(this._config).AppSettings?.Secrets;
            this.MapboxAccessToken = secrets?.Mapbox.AccessToken;
            this.GooglePlaceApiKey = secrets?.Google.ApiKey;
        }

        public IActionResult OnAirportsGet()
        {
            var airportDataService = new AirportDataService(this._env);
            var data = airportDataService.GetAll();
            return new JsonResult(data);
        }
    }
}
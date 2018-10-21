using AirplaneCore.Plus.Web.Data.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AirplaneCore.Plus.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportsController : ControllerBase
    {
        #region service def

        private readonly IHostingEnvironment _env;

        public AirportsController(IHostingEnvironment env)
        {
            this._env = env;
        }
        #endregion

        [Route("[action]")]
        public IActionResult Get()
        {
            var svc = new AirportDataService(this._env);
            var features = svc.GetAll().Select(o => new
            {
                type = "Feature",
                geometry = new
                {
                    type = "Point",
                    coordinates = new[] { o.Longitude, o.Latitude }
                },
                properties = new
                {
                    name = o.Name,
                    iataCode = o.IATA
                }
            });
            return Ok(new {
                type = "FeatureCollection",
                features
            });
        }
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace AirplaneCore.Plus.Web.Controllers
{
    [Route("api/g")]
    [ApiController]
    public class GoogleController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly Cores.Services.GoogleServices.Place _placeService;

        const string OK = "OK";

        public GoogleController(IConfiguration config)
        {
            this._config = config;
            this._placeService = new Cores.Services.GoogleServices.Place(this._config);
        }

        [Route("nearby/{lng}/{lat}/{name}/{lang?}")]
        public async Task<IActionResult> GetNearby(double lng, double lat, string name, string lang)
        {
            var ret = await this._placeService.Nearbysearch(lng, lat, name, lang);
            return Ok(ret);
        }

        [Route("detail/{placeid}/{lang?}")]
        public async Task<IActionResult> GetDetail(string placeid, string lang)
        {
            var ret = await this._placeService.Detail(placeid, lang);
            return Ok(ret);
        }
        
        [Route("r/{lng}/{lat}/{name}/{width}/{lang?}")]
        public async Task<IActionResult> GetMixedUp(double lng, double lat, string name, int width, string lang)
        {
            var nearby = await this._placeService.Nearbysearch(lng, lat, name, lang);
            if (nearby == null || nearby.status != OK || !nearby.results.Any())
            {
                return Ok(new
                {
                    name = string.Empty,
                    photo = string.Empty,
                    formattedAddress = string.Empty,
                    phoneNumber = string.Empty,
                    website = string.Empty
                });
            }

            var detail = await this._placeService.Detail(nearby.results.FirstOrDefault().place_id, lang);
            var detailResult = detail?.result;

            var photos = nearby.results.FirstOrDefault().photos;
            var photo = string.Empty;
            if (photos != null && photos.Any())
            {
                photo = await this._placeService.Photo(photos.FirstOrDefault().photo_reference, width);
            }
            
            return Ok(new {
                detailResult.name,
                photo,
                formattedAddress = detailResult?.formatted_address,
                phoneNumber = detailResult?.international_phone_number,
                detailResult?.website
            });
        }
    }
}
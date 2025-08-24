using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace LearnWebApi_Controllers.Controllers.v2
{
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "v2")]
    [ApiController]
    [ApiVersion("2.0")]
    public class WeatherForecastV2Controller : ControllerBase
    {

        [HttpGet]
        [Route("[action]")]
        [EndpointName("WeatherApiV2")]
        [EndpointSummary("Gives SOME DETAILS V2")]
        [EndpointGroupName("v2")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Tags("Weather")]
        public IActionResult Index()
        {
            return Ok("This is the V2 of WeatherForecast");
        }
    }
}

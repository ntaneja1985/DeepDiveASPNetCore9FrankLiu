using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace LearnWebApi_Controllers.Controllers.v1
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "v2")]
    [ApiVersion("1.0")]
    public class WeatherForecastV1Controller : ControllerBase
    {

        [HttpGet]
        [Route("[action]")]
        [EndpointName("WeatherApiV1")]
        [EndpointSummary("Gives SOME DETAILS")]
        [EndpointGroupName("v1")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Tags("Weather")]
        public IActionResult Index()
        {
            return Ok("This is the V1 of WeatherForecast");
        }
    }
}

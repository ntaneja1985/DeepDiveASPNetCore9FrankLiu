using Microsoft.AspNetCore.Mvc;

namespace LearnWebApi_Controllers.Controllers
{
    //[ApiController]
    //[Route("[controller]")]
    

    //public class WeatherForecastController : ControllerBase
    //{
    //    private static readonly string[] Summaries = new[]
    //    {
    //        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    //    };

    //    private readonly ILogger<WeatherForecastController> _logger;

    //    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    //    {
    //        _logger = logger;
    //    }

    //    [HttpGet]
    //    [Route("[action]")]
    //    [EndpointName("WeatherApi")]
    //    [EndpointSummary("Gives the weather details")]
    //    [EndpointGroupName("v1")]
    //    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //    [Tags("Weather")]
    //    public IEnumerable<WeatherForecast> Get()
    //    {
    //        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    //        {
    //            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
    //            TemperatureC = Random.Shared.Next(-20, 55),
    //            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    //        })
    //        .ToArray();
    //    }

    //    [HttpGet]
    //    [Route("[action]")]
    //    [EndpointName("TemperatureApi")]
    //    [EndpointSummary("Gives the temperature details")]
    //    [EndpointGroupName("v1")]
    //    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //    [Tags("Temperature")]
    //    public IEnumerable<WeatherForecast> GetByTemperature(int temperature)
    //    {
    //        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    //        {
    //            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
    //            //TemperatureC = Random.Shared.Next(-20, 55),
    //            TemperatureC = temperature,
    //            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    //        })
    //        .ToArray();
    //    }


    //}
}

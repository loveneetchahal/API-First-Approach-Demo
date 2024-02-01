using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIFirstDemo.Controllers.V2
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    public class WeatherForecastController : ControllerBase
    {
        [MapToApiVersion("2.0")]
        [HttpGet]
        public string Get() => ".Net Core Web API Version 2";
    }
}

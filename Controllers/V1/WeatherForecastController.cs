using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIFirstDemo.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class WeatherForecastController : ControllerBase
    {
        [MapToApiVersion("1.0")]
        [HttpGet]
        public string Get() => ".Net Core Web API Version 1";
    }
}

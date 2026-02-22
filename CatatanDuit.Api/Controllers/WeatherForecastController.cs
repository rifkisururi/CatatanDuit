using Microsoft.AspNetCore.Mvc;

namespace CatatanDuit.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<WeatherForecast[]> Get()
        {
            return await Task.Run(() =>
            {
                var today = DateTime.Now;
                var result = new WeatherForecast[5];

                for (int i = 0; i < 5; i++)
                {
                    result[i] = new WeatherForecast
                    {
                        Date = DateOnly.FromDateTime(today.AddDays(i + 1)),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                    };
                }

                return result;
            });
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastsController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastsController> _logger;

        public WeatherForecastsController(ILogger<WeatherForecastsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Get()
        {
            var rng = new Random();
            if(rng == null)
            {
                return BadRequest();
            }
            var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.UtcNow.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            if(id < 0)
            {
                return BadRequest();
            }

            var rng = new Random();
           
            var generationForcasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Id = index,
                Date = DateTime.UtcNow.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

            var searchedForcast = generationForcasts.SingleOrDefault(f => f.Id == id);
            if (searchedForcast == null)
            {
                return NotFound();
            }

            return Ok(searchedForcast);
        }
    }
}

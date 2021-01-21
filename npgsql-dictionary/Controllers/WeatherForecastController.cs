using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using npgsql_dictionary.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace npgsql_dictionary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly MainDbContext _mainDbContext;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, MainDbContext mainDbContext)
        {
            _logger = logger;
            _mainDbContext = mainDbContext;
        }

        [HttpGet("/test")]
        public async Task<IActionResult> GetTest()
        {
            var tenant = new Models.Tenant("Sabeco");
            tenant.ExtraProperties.Add("Connection", "locahost");
            await _mainDbContext.Tenants.AddAsync(tenant);
            await _mainDbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public IEnumerable<Tenant> Get()
        {
            _mainDbContext.Database.EnsureCreated();
            var tenant = _mainDbContext.Tenants.ToList();
            return tenant;
        }
    }
}

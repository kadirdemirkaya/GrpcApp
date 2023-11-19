using ArticleService;
using gRPCClientApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace gRPCClientApp.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IBookDataClient _bookDataClient;

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IBookDataClient bookDataClient)
    {
        _logger = logger;
        _bookDataClient = bookDataClient;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet("GetArticleById")]
    public async Task<IActionResult> GetArticleById()
    {
        Random random = new Random();
        string randomNumber = random.Next(1, 4).ToString();

        GrpcArticleModel article = _bookDataClient.ReturnAllArticles(randomNumber);

        return Ok(article);
    }
}

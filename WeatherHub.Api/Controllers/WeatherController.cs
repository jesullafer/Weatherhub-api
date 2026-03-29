using Microsoft.AspNetCore.Mvc;
using WeatherHub.Application.UseCases.GetWeatherByCity;

namespace WeatherHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class WeatherController : ControllerBase
{
    private readonly GetWeatherByCityQueryHandler _handler;

    public WeatherController(GetWeatherByCityQueryHandler handler)
    {
        _handler = handler;
    }

    [HttpGet("by-city")]
    public async Task<IActionResult> GetByCity([FromQuery] string city, CancellationToken cancellationToken)
    {
        var query = new GetWeatherByCityQuery(city);

        var result = await _handler.HandleAsync(query, cancellationToken);        

        return Ok(result);
    }
}
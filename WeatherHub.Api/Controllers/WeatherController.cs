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
        if (string.IsNullOrWhiteSpace(city))
            return BadRequest(new { message = "The city parameter is required." });

        var result = await _handler.HandleAsync(city, cancellationToken);

        if (result is null)
            return NotFound(new { message = $"No weather data found for city '{city}'." });

        return Ok(result);
    }
}
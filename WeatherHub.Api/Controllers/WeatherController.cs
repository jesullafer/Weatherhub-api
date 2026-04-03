using MediatR;
using Microsoft.AspNetCore.Mvc;
using WeatherHub.Application.UseCases.GetWeatherByCity;

namespace WeatherHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class WeatherController : ControllerBase
{    
    private readonly IMediator _mediator;

    public WeatherController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("by-city")]
    public async Task<IActionResult> GetByCity([FromQuery] string city, CancellationToken cancellationToken)
    {
        var query = new GetWeatherByCityQuery(city);

        var result = await _mediator.Send(query, cancellationToken);         

        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchCities([FromQuery] string query, CancellationToken cancellationToken) 
    { 
        var result = await _mediator.Send(new SearchCitiesQuery(query), cancellationToken);

        return Ok(result);
    }
}
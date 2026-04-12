using MediatR;
using Microsoft.AspNetCore.Mvc;
using WeatherHub.Application.Common.Models;
using WeatherHub.Application.DTOs;
using WeatherHub.Application.UseCases.Favorites.AddFavorite;
using WeatherHub.Application.UseCases.Favorites.GetFavorites;
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

        return Ok(ApiResponse<WeatherDto>.Ok(result));
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchCities([FromQuery] string query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new SearchCitiesQuery(query), cancellationToken);

        return Ok(ApiResponse<IEnumerable<CityDto>>.Ok(result));
    }

    [HttpPost("favorites")]
    public async Task<IActionResult> AddFavorite([FromBody] string city)
    {
        await _mediator.Send(new AddFavoriteCommand(city));

        return Ok();
    }

    [HttpGet("favorites")]
    public async Task<IActionResult> GetFavorites()
    {
        var favorites = await _mediator.Send(new GetFavoritesQuery());

        return Ok(favorites);
    }
}
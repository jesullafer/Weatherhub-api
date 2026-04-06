using MediatR;
using WeatherHub.Application.Abstractions;
using WeatherHub.Application.Common.Exceptions;
using WeatherHub.Application.DTOs;

namespace WeatherHub.Application.UseCases.GetWeatherByCity;

public sealed class GetWeatherByCityQueryHandler : IRequestHandler<GetWeatherByCityQuery, WeatherDto?>
{
    private readonly IWeatherProvider _weatherProvider;

    public GetWeatherByCityQueryHandler(IWeatherProvider weatherProvider)
    {
        _weatherProvider = weatherProvider;
    }    

    public async Task<WeatherDto?> Handle(GetWeatherByCityQuery query, CancellationToken cancellationToken = default)
    {        
        var weather = await _weatherProvider.GetByCityAsync(query.City, cancellationToken);

        if (weather is null)
            throw new AppException($"Weather not found for city '{query.City}'", 404);

        return new WeatherDto
        {
            City = weather.City,
            Temperature = weather.Temperature.Celsius,
            Description = weather.Description,
            Humidity = weather.Humidity
        };
    }
}
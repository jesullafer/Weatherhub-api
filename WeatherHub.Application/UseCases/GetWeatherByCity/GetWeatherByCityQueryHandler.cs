using WeatherHub.Application.Abstractions;
using WeatherHub.Application.DTOs;

namespace WeatherHub.Application.UseCases.GetWeatherByCity;

public sealed class GetWeatherByCityQueryHandler
{
    private readonly IWeatherProvider _weatherProvider;

    public GetWeatherByCityQueryHandler(IWeatherProvider weatherProvider)
    {
        _weatherProvider = weatherProvider;
    }

    public async Task<WeatherResponseDto?> HandleAsync(string city, CancellationToken cancellationToken = default)
    {
        var weather = await _weatherProvider.GetByCityAsync(city, cancellationToken);

        if (weather is null)
            return null;

        return new WeatherResponseDto
        {
            City = weather.City,
            Temperature = weather.Temperature,
            Description = weather.Description,
            Humidity = weather.Humidity
        };
    }
}
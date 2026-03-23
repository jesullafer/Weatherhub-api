using WeatherHub.Domain.Entities;

namespace WeatherHub.Application.Abstractions;

public interface IWeatherProvider
{
    Task<Weather?> GetByCityAsync(string city, CancellationToken cancellationToken = default);
}
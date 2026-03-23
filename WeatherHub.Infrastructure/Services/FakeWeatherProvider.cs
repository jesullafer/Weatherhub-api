using WeatherHub.Application.Abstractions;
using WeatherHub.Domain.Entities;

namespace WeatherHub.Infrastructure.Services;

public sealed class FakeWeatherProvider : IWeatherProvider
{
    public Task<Weather?> GetByCityAsync(string city, CancellationToken cancellationToken = default)
    {
        if (string.Equals(city, "Madrid", StringComparison.OrdinalIgnoreCase))
        {
            var weather = new Weather(
                city: "Madrid",
                temperature: 25,
                description: "Soleado",
                humidity: 40);

            return Task.FromResult<Weather?>(weather);
        }

        return Task.FromResult<Weather?>(new Weather(
            city: city,
            temperature: 22,
            description: "Despejado",
            humidity: 50));
    }
}
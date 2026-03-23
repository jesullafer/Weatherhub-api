using WeatherHub.Application.Abstractions;
using WeatherHub.Domain.Entities;

namespace WeatherHub.Infrastructure.Services;

public sealed class FakeWeatherProvider : IWeatherProvider
{
    public Task<Weather?> GetByCityAsync(string city, CancellationToken cancellationToken = default)
    {
        Weather weather = city.ToLowerInvariant() switch
        {
            "madrid" => new Weather(
                city: "Madrid",
                temperature: 25,
                description: "Soleado",
                humidity: 40),

            "barcelona" => new Weather(
                city: "Barcelona",
                temperature: 12,
                description: "Lluvioso",
                humidity: 50),

            "valencia" => new Weather(
                city: "Valencia",
                temperature: 18,
                description: "Nublado",
                humidity: 50),

            _ => new Weather(
                city: city,
                temperature: 22,
                description: "Despejado",
                humidity: 50)
        };

        return Task.FromResult<Weather?>(weather);
    }


}
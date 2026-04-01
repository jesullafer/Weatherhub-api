using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using WeatherHub.Application.Abstractions;
using WeatherHub.Domain.Entities;

namespace WeatherHub.Infrastructure.Services
{
    public class CachedWeatherProvider: IWeatherProvider
    {
        private readonly IWeatherProvider _inner;
        private readonly IMemoryCache _cache;
        private readonly ILogger<CachedWeatherProvider> _logger;
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

        public CachedWeatherProvider (IWeatherProvider inner, IMemoryCache cache, ILogger<CachedWeatherProvider> logger)
        {
            _inner = inner;
            _cache = cache;
            _logger = logger;
        }

        public async Task<Weather?> GetByCityAsync(string city, CancellationToken cancellationToken = default)
        {
            var normalizedCity = NormalizeCity(city);
            var key = $"weather:{normalizedCity}";

            if (_cache.TryGetValue(key, out Weather? cachedWeather) && cachedWeather is not null)
            {
                _logger.LogInformation("Cache hit for city: {city}", normalizedCity);
                return cachedWeather;                                       
            }

            _logger.LogInformation("Cache miss for city: {city}", normalizedCity);

            var weather = await _inner.GetByCityAsync(city, cancellationToken);
            
            if (weather is not null)
            {                    
                _cache.Set(key, weather, CacheDuration);
                _logger.LogInformation("Weather cached for city: {city} with TTL: {ttl}", normalizedCity, CacheDuration);
            }           

            return weather;                         
        }

        private static string NormalizeCity(string city)
        {
            return city.Trim().ToLowerInvariant();
        }
    }
}

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WeatherHub.Application.Abstractions;
using WeatherHub.Domain.Entities;
using WeatherHub.Infrastructure.Options;

namespace WeatherHub.Infrastructure.Services
{
    public class CachedWeatherProvider : IWeatherProvider
    {
        private readonly IWeatherProvider _inner;
        private readonly IMemoryCache _cache;
        private readonly ILogger<CachedWeatherProvider> _logger;
        private readonly TimeSpan _ttl;

        public CachedWeatherProvider(IWeatherProvider inner, IMemoryCache cache, ILogger<CachedWeatherProvider> logger, IOptions<CacheOptions> options)
        {
            _inner = inner;
            _cache = cache;
            _logger = logger;

            if (options.Value.WeatherTTLMinutes <= 0)
            {
                _logger.LogWarning("Cache disabled due to invalid TTL configuration");
                _ttl = TimeSpan.Zero;
            }
            else
                _ttl = TimeSpan.FromMinutes(options.Value.WeatherTTLMinutes);
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

            var weather = await _cache.GetOrCreateAsync(key, async entry =>
            {
                if (_ttl > TimeSpan.Zero)                
                    entry.AbsoluteExpirationRelativeToNow = _ttl;                

                _logger.LogInformation("Cache miss for city: {city}", normalizedCity);

                var result = await _inner.GetByCityAsync(city, cancellationToken);

                if (result is not null && _ttl > TimeSpan.Zero)
                {
                    _logger.LogInformation("Weather cached for city: {city} with TTL: {ttl}", normalizedCity, _ttl);
                }

                return result;
            });

            return weather;           
        }

        private static string NormalizeCity(string city)
        {
            return city.Trim().ToLowerInvariant();
        }
    }
}

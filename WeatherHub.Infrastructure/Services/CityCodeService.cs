using WeatherHub.Application.Abstractions;

namespace WeatherHub.Infrastructure.Services
{
    public sealed class CityCodeService: ICityCodeService
    {
        private readonly Dictionary<string, string> _cityToCodeMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {"Madrid", "28079"},
            {"Barcelona", "08019"},
            {"Valencia", "46250"}
        };

        public Task<string?> GetCode(string city)
        {
            // TO-DO In a real implementation, this method would likely query a database or an external API
            // to retrieve the city code based on the city name.
            city = city.Trim();

            if (_cityToCodeMap.TryGetValue(city, out var code))
            {
                return Task.FromResult<string?>(code);
            }
            
            return Task.FromResult<string?>(null);
        }
    }
}

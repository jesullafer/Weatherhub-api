using WeatherHub.Application.Abstractions;
using WeatherHub.Application.DTOs;

namespace WeatherHub.Infrastructure.Services
{
    public sealed class CitySearchService : ICitySearchService
    {
        private static readonly List<CityDto> _cities = new List<CityDto>()
        {
            new CityDto { Name = "Madrid", Code = "28079"},
            new CityDto { Name = "Barcelona", Code = "08019"},
            new CityDto { Name = "Valencia", Code = "46250"}
        };

        public Task<IEnumerable<CityDto>> SearchAsync(string query, CancellationToken cancellationToken)
        {
            // TO-DO In a real implementation, this method would likely query a database or an external API
            // to retrieve the city code based on the city name.

            if (string.IsNullOrWhiteSpace(query))
                return Task.FromResult(Enumerable.Empty<CityDto>());

            var cities = _cities.Where(c => c.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
                                .ToList();            

            return Task.FromResult<IEnumerable<CityDto>>(cities);
        }
    }
}

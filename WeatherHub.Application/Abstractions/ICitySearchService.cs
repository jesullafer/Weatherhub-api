using WeatherHub.Application.DTOs;

namespace WeatherHub.Application.Abstractions
{
    public interface ICitySearchService
    {
        Task<IEnumerable<CityDto>> SearchAsync(string query, CancellationToken cancellationToken);
    }
}

using MediatR;
using WeatherHub.Application.DTOs;

public class SearchCitiesQuery : IRequest<IEnumerable<CityDto>>
{
    public string Query { get; }

    public SearchCitiesQuery(string query)
    {
        Query = query?.Trim() ?? string.Empty;
    }
}
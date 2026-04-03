using MediatR;
using WeatherHub.Application.Abstractions;
using WeatherHub.Application.DTOs;

public class SearchCitiesQueryHandler: IRequestHandler<SearchCitiesQuery, IEnumerable<CityDto>>
{
    private readonly ICitySearchService _service;

    public SearchCitiesQueryHandler(ICitySearchService service)
    {
        _service = service;
    }

    public async Task<IEnumerable<CityDto>> Handle(SearchCitiesQuery request, CancellationToken cancellationToken)
    {
        return await _service.SearchAsync(request.Query, cancellationToken);
    }
}
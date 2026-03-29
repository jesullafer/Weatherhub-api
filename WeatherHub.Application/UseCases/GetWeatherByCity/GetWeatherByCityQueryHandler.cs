using FluentValidation;
using WeatherHub.Application.Abstractions;
using WeatherHub.Application.Common.Exceptions;
using WeatherHub.Application.DTOs;

namespace WeatherHub.Application.UseCases.GetWeatherByCity;

public sealed class GetWeatherByCityQueryHandler
{
    private readonly IWeatherProvider _weatherProvider;
    private readonly IValidator<GetWeatherByCityQuery> _validator;

    public GetWeatherByCityQueryHandler(IWeatherProvider weatherProvider, IValidator<GetWeatherByCityQuery> validator)
    {
        _weatherProvider = weatherProvider;
        _validator = validator;
    }

    public async Task<WeatherResponseDto?> HandleAsync(GetWeatherByCityQuery query, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(query);

        if (!validationResult.IsValid)        
            throw new AppException(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)), 400);

        var weather = await _weatherProvider.GetByCityAsync(query.City, cancellationToken);

        if (weather is null)
            throw new AppException($"Weather not found for city '{query.City}'", 404);

        return new WeatherResponseDto
        {
            City = weather.City,
            Temperature = weather.Temperature,
            Description = weather.Description,
            Humidity = weather.Humidity
        };

    }
}
using Microsoft.Extensions.Options;
using System.Text.Json;
using WeatherHub.Application.Abstractions;
using WeatherHub.Application.Common.Exceptions;
using WeatherHub.Domain.Entities;
using WeatherHub.Infrastructure.Options;

namespace WeatherHub.Infrastructure.Services;

public sealed class AemetWeatherProvider : IWeatherProvider
{
    private readonly HttpClient _httpClient;
    private readonly AemetOptions _options;    
    private readonly ICityCodeService _cityCodeService;

    public AemetWeatherProvider(
        HttpClient httpClient,
        IOptions<AemetOptions> options,
        ICityCodeService cityCodeService)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _cityCodeService = cityCodeService;
    }

    public async Task<Weather?> GetByCityAsync(string city, CancellationToken cancellationToken = default)
    {
        // TODO: map city name to AEMET municipality code instead of using Madrid fixed code.               
        var cityCode = await _cityCodeService.GetCode(city);

        if (cityCode == null)
            throw new AppException($"City {city} is not supported.", 400);

        var endpoint = GetEndpoint(cityCode);

        var response = await _httpClient.GetAsync(endpoint, cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new AppException($"Failed to retrieve data from AEMET.", 503);

        if (response.Content is null)
            throw new InvalidOperationException("AEMET response content is null.");

        var json = await response.Content.ReadAsStringAsync(cancellationToken);

        using var doc = JsonDocument.Parse(json);

        var datosUrl = doc.RootElement.GetProperty("datos").GetString();

        if (string.IsNullOrEmpty(datosUrl))
            throw new AppException($"Invalid response from AEMET service.", 502);

        // Segunda llamada 
        var datosResponse = await _httpClient.GetAsync(datosUrl, cancellationToken);

        if (!datosResponse.IsSuccessStatusCode)
            throw new AppException($"Failed to retrieve weather data from AEMET.", 502);

        var datosJson = await datosResponse.Content.ReadAsStringAsync(cancellationToken);

        using var datosDoc = JsonDocument.Parse(datosJson);

        var root = datosDoc.RootElement[0];

        var cityName = root.GetProperty("nombre").GetString() ?? city;

        var temperatura = root
            .GetProperty("prediccion")
            .GetProperty("dia")[0]
            .GetProperty("temperatura")
            .GetProperty("maxima")
            .GetDecimal();

        return new Weather(
            city: cityName,
            temperature: temperatura,
            description: "Datos AEMET",
            humidity: 50
        );
    }

    private string GetEndpoint(string cityCode)    {        

       return $"https://opendata.aemet.es/opendata/api/prediccion/especifica/municipio/diaria/{cityCode}?api_key={_options.ApiKey}";
    }
}
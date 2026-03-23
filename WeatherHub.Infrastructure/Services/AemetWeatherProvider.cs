using Microsoft.Extensions.Options;
using System.Text.Json;
using WeatherHub.Application.Abstractions;
using WeatherHub.Domain.Entities;
using WeatherHub.Infrastructure.Options;

namespace WeatherHub.Infrastructure.Services;

public sealed class AemetWeatherProvider : IWeatherProvider
{
    private readonly HttpClient _httpClient;
    private readonly AemetOptions _options;

    public AemetWeatherProvider(
        HttpClient httpClient,
        IOptions<AemetOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<Weather?> GetByCityAsync(string city, CancellationToken cancellationToken = default)
    {
        // TODO: map city name to AEMET municipality code instead of using Madrid fixed code.
        var endpoint = $"https://opendata.aemet.es/opendata/api/prediccion/especifica/municipio/diaria/28079?api_key={_options.ApiKey}";

        var response = await _httpClient.GetAsync(endpoint, cancellationToken);

        if (!response.IsSuccessStatusCode)
            return null;

        if (response.Content is null)
        {
            throw new InvalidOperationException("AEMET response content is null.");
        }

        var json = await response.Content.ReadAsStringAsync(cancellationToken);


        using var doc = JsonDocument.Parse(json);

        var datosUrl = doc.RootElement.GetProperty("datos").GetString();

        if (string.IsNullOrEmpty(datosUrl))
            return null;

        // Segunda llamada 
        var datosResponse = await _httpClient.GetAsync(datosUrl, cancellationToken);

        if (!datosResponse.IsSuccessStatusCode)
            return null;

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
}
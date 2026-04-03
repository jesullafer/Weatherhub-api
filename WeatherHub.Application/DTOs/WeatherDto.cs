namespace WeatherHub.Application.DTOs;

public sealed class WeatherDto
{
    public string City { get; init; } = default!;
    public decimal Temperature { get; init; }
    public string Description { get; init; } = default!;
    public int Humidity { get; init; }
}
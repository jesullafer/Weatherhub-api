using WeatherHub.Domain.ValueObjects;

namespace WeatherHub.Domain.Entities;

public sealed class Weather
{
    public string City { get; private set; }
    public Temperature Temperature { get; private set; }
    public string Description { get; private set; }
    public int Humidity { get; private set; }
    public Coordinates? Coordinates { get; private set; }

    public Weather(
        string city, 
        Temperature temperature, 
        string description, 
        int humidity,
        Coordinates? coordinates = null)
    {
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City cannot be empty.", nameof(city));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty.", nameof(description));

        if (humidity < 0 || humidity > 100)
            throw new ArgumentOutOfRangeException(nameof(humidity), "Humidity must be between 0 and 100.");

        City = city;
        Temperature = temperature;
        Description = description;
        Humidity = humidity;
        Coordinates = coordinates;
    }

    public bool IsFreezing => Temperature.IsFreezing;

    public bool IsHumid => Humidity > 70;

    public bool IsPleasant => !IsFreezing && !Temperature.IsHot;
}
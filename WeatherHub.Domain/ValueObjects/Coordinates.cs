namespace WeatherHub.Domain.ValueObjects;

public sealed record Coordinates
{
    public decimal Latitude { get; }
    public decimal Longitude { get; }

    public Coordinates(decimal latitude, decimal longitude)
    {
        if (latitude < -90 || latitude > 90)
            throw new ArgumentOutOfRangeException(nameof(latitude));

        if (longitude < -180 || longitude > 180)
            throw new ArgumentOutOfRangeException(nameof(longitude));

        Latitude = latitude;
        Longitude = longitude;
    }

    public bool IsValid => Latitude >= -90 && Latitude <= 90
                    && Longitude >= -180 && Longitude <= 180;

    public bool IsNorthernHemisphere => Latitude > 0;
}
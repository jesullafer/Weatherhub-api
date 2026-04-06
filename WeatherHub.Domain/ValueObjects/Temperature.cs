namespace WeatherHub.Domain.ValueObjects;

public sealed record Temperature
{
    public decimal Celsius { get; }

    public bool IsFreezing => Celsius <= 0;

    public Temperature(decimal celsius)
    {
        if (celsius < -100 || celsius > 100)
            throw new ArgumentOutOfRangeException(nameof(celsius), "Temperature must be between -100 and 100 ºC.");

        Celsius = celsius;
    }

    public override string ToString() => $"{Celsius:0.#} ºC";
}
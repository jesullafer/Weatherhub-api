namespace WeatherHub.Domain.ValueObjects;

public sealed record Temperature
{
    public decimal Celsius { get; }    

    public Temperature(decimal celsius)
    {
        if (celsius < -100 || celsius > 100)
            throw new ArgumentOutOfRangeException(nameof(celsius), "Temperature must be between -100 and 100 ºC.");

        Celsius = celsius;
    }

    public bool IsFreezing => Celsius <= 0;
    public bool IsHot => Celsius >= 30;

    public decimal ToFahrenheit()
    {
        return (Celsius * 9 / 5) + 32;
    }

    public override string ToString() => $"{Celsius:0.#} ºC";
    
}
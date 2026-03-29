namespace WeatherHub.Application.UseCases.GetWeatherByCity
{
    public  class GetWeatherByCityQuery
    {
        public string City { get; }

        public GetWeatherByCityQuery(string city)
        {
            City = city?.Trim() ?? string.Empty;
        }
    }
}

using MediatR;
using WeatherHub.Application.DTOs;

namespace WeatherHub.Application.UseCases.GetWeatherByCity
{
    public  class GetWeatherByCityQuery : IRequest<WeatherDto?>
    {
        public string City { get; }

        public GetWeatherByCityQuery(string city)
        {
            City = city?.Trim() ?? string.Empty;
        }
    }
}

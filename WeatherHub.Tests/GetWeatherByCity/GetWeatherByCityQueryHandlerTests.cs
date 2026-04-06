using FluentValidation;
using FluentValidation.Results;
using Moq;
using WeatherHub.Application.Abstractions;
using WeatherHub.Application.Common.Exceptions;
using WeatherHub.Application.UseCases.GetWeatherByCity;
using WeatherHub.Domain.Entities;

namespace WeatherHub.Tests.GetWeatherByCity
{
    
    public  class GetWeatherByCityQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnWeatherResponseDto_WhenRequestIsValid()
        {
            var weatherProviderMock = new Mock<IWeatherProvider>();

            var query = new GetWeatherByCityQuery("Madrid");            
           
            var weather = new Weather(
                city: "Madrid",
                temperature: new Domain.ValueObjects.Temperature(25),
                description: "Soleado",
                humidity: 40
            );

            weatherProviderMock
                .Setup(p => p.GetByCityAsync(query.City, default))
                .ReturnsAsync(weather);

            var handler = new GetWeatherByCityQueryHandler(
                weatherProviderMock.Object
            );

            var result = await handler.Handle(query);

            Assert.NotNull(result);
            Assert.Equal("Madrid", result.City);
            Assert.Equal(25, result.Temperature);
            Assert.Equal("Soleado", result.Description);
            Assert.Equal(40, result.Humidity);
            Assert.False(result.IsFreezing);
            Assert.False(result.IsHot);
        }

        [Fact]
        public async Task HandleAsync_ShouldThrowAppException_WhenWeatherNotFound()
        {
            var weatherProviderMock = new Mock<IWeatherProvider>();
            var validatorMock = new Mock<IValidator<GetWeatherByCityQuery>>();

            var query = new GetWeatherByCityQuery("VillaArriba");

            Weather? weather = null;

            weatherProviderMock
                .Setup(p => p.GetByCityAsync(query.City, default))
                .ReturnsAsync(weather);

            var handler = new GetWeatherByCityQueryHandler(
                weatherProviderMock.Object
            );           

            var exception = await Assert.ThrowsAsync<AppException>(() =>
                 handler.Handle(query)
             );

            weatherProviderMock.Verify(
                p => p.GetByCityAsync(query.City, default),
                Times.Once
            );

            Assert.Equal(404, exception.StatusCode);
            Assert.Contains($"Weather not found for city 'VillaArriba'", exception.Message);
        }
    }
}
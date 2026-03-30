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
        public async Task HandleAsync_ShouldThrowAppException_WhenValidationFails()
        {            
            var weatherProviderMock = new Mock<IWeatherProvider>();
            var validatorMock = new Mock<IValidator<GetWeatherByCityQuery>>();
            var query = new GetWeatherByCityQuery("A"); 

            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("City", "City must have at least 2 characters")
            };

            var validationResult = new ValidationResult(validationFailures);

            validatorMock
                .Setup(v => v.ValidateAsync(query, default))
                .ReturnsAsync(validationResult);

            var handler = new GetWeatherByCityQueryHandler(
                weatherProviderMock.Object,
                validatorMock.Object
            );

            var exception = await Assert.ThrowsAsync<AppException>(() =>
                handler.HandleAsync(query)
            );

            Assert.Equal(400, exception.StatusCode);
            Assert.Contains("City must have at least 2 characters", exception.Message);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnWeatherResponseDto_WhenRequestIsValid()
        {
            var weatherProviderMock = new Mock<IWeatherProvider>();
            var validatorMock = new Mock<IValidator<GetWeatherByCityQuery>>();

            var query = new GetWeatherByCityQuery("Madrid");

            validatorMock
                .Setup(v => v.ValidateAsync(query, default))
                .ReturnsAsync(new ValidationResult());
           
            var weather = new Weather(
                city: "Madrid",
                temperature: 25,
                description: "Soleado",
                humidity: 40
            );

            weatherProviderMock
                .Setup(p => p.GetByCityAsync(query.City, default))
                .ReturnsAsync(weather);

            var handler = new GetWeatherByCityQueryHandler(
                weatherProviderMock.Object,
                validatorMock.Object
            );

            var result = await handler.HandleAsync(query);

            Assert.NotNull(result);
            Assert.Equal("Madrid", result.City);
            Assert.Equal(25, result.Temperature);
            Assert.Equal("Soleado", result.Description);
            Assert.Equal(40, result.Humidity);
        }

        [Fact]
        public async Task HandleAsync_ShouldThrowAppException_WhenWeatherNotFound()
        {
            var weatherProviderMock = new Mock<IWeatherProvider>();
            var validatorMock = new Mock<IValidator<GetWeatherByCityQuery>>();

            var query = new GetWeatherByCityQuery("VillaArriba");

            validatorMock
                .Setup(v => v.ValidateAsync(query, default))
                .ReturnsAsync(new ValidationResult());

            Weather? weather = null;

            weatherProviderMock
                .Setup(p => p.GetByCityAsync(query.City, default))
                .ReturnsAsync(weather);

            var handler = new GetWeatherByCityQueryHandler(
                weatherProviderMock.Object,
                validatorMock.Object
            );           

            var exception = await Assert.ThrowsAsync<AppException>(() =>
                 handler.HandleAsync(query)
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
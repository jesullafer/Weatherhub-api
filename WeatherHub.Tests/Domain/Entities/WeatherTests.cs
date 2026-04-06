using WeatherHub.Domain.Entities;
using WeatherHub.Domain.ValueObjects;

namespace WeatherHub.Tests.Domain.Entities
{
    public class WeatherTests
    {
        [Fact]
        public void Constructor_ShouldCreateWeather_WhenValuesAreValid()
        {
            // Arrange
            var city = "Madrid";
            var temperature = new Temperature(25);
            var description = "Soleado";
            var humidity = 40;

            // Act
            var weather = new Weather(city, temperature, description, humidity);

            // Assert
            Assert.Equal(city, weather.City);
            Assert.Equal(temperature, weather.Temperature);
            Assert.Equal(description, weather.Description);
            Assert.Equal(humidity, weather.Humidity);
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenCityIsEmpty()
        {
            var temperature = new Temperature(20);

            Assert.Throws<ArgumentException>(() =>
                new Weather("", temperature, "desc", 50)
            );
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenDescriptionIsEmpty()
        {
            var temperature = new Temperature(20);

            Assert.Throws<ArgumentException>(() =>
                new Weather("Madrid", temperature, "", 50)
            );
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenHumidityIsOutOfRange()
        {
            var temperature = new Temperature(20);

            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new Weather("Madrid", temperature, "desc", 200)
            );
        }
    }
}

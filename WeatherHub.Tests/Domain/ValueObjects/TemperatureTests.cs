using WeatherHub.Domain.ValueObjects;

namespace WeatherHub.Tests.Domain.ValueObjects
{
    public class TemperatureTests
    {
        [Fact]
        public void Constructor_ShouldCreateTemperature_WhenValueIsValid()
        {
            // Arrange
            var value = 25;

            // Act
            var temperature = new Temperature(value);

            // Assert
            Assert.Equal(25, temperature.Celsius);
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenValueIsOutOfRange()
        {
            // Arrange
            var invalidValue = 200;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new Temperature(invalidValue)
            );
        }

        [Fact]
        public void IsFreezing_ShouldReturnTrue_WhenTemperatureIsZeroOrLess()
        {
            var temperature = new Temperature(0);

            Assert.True(temperature.IsFreezing);
        }

        [Fact]
        public void IsHot_ShouldReturnTrue_WhenTemperatureIsHigh()
        {
            var temperature = new Temperature(35);

            Assert.True(temperature.IsHot);
        }
    }
}

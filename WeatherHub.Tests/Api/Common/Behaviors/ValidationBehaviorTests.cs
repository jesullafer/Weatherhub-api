using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;
using WeatherHub.Application.Common.Behaviors;
using WeatherHub.Application.Common.Exceptions;

namespace WeatherHub.Tests.Api.Common.Behaviors
{
    public class FakeValidator : AbstractValidator<object>
    {
        public FakeValidator()
        {
            RuleFor(x => x).Custom((_, context) =>
            {
                context.AddFailure("Property", "Error message");
            });
        }
    }

    public class ValidationBehaviorTests
    {
        [Fact]
        public async Task Handle_ShouldThrowAppException_WhenValidationFails()
        {
            // Arrange
            var validator = new FakeValidator();

            var behavior = new ValidationBehavior<object, object>(
                new[] { validator }
            );

            RequestHandlerDelegate<object> next = (ct) =>
                Task.FromResult(new object());

            // Act
            var exception = await Assert.ThrowsAsync<AppException>(() =>
                behavior.Handle(
                    new object(),
                    next,
                    default
                )
            );

            // Assert
            Assert.Equal(400, exception.StatusCode);
            Assert.Contains("Error message", exception.Message);
        }

        [Fact]
        public async Task Handle_ShouldCallNext_WhenValidationSucceeds()
        {
            // Arrange

            // Validator que NO devuelve errores
            var validatorMock = new Mock<IValidator<object>>();

            validatorMock
                .Setup(v => v.ValidateAsync(
                    It.IsAny<ValidationContext<object>>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(new ValidationResult());

            var behavior = new ValidationBehavior<object, object>(
                new[] { validatorMock.Object }
            );

            var nextCalled = false;

            RequestHandlerDelegate<object> next = (ct) =>
            {
                nextCalled = true;
                return Task.FromResult(new object());
            };

            // Act
            var result = await behavior.Handle(
                new object(),
                next,
                default
            );

            // Assert
            Assert.True(nextCalled);
            Assert.NotNull(result);
        }
    }
}

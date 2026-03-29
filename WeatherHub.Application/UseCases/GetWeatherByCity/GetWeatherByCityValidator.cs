using FluentValidation;

namespace WeatherHub.Application.UseCases.GetWeatherByCity
{
    public class GetWeatherByCityValidator : AbstractValidator<GetWeatherByCityQuery>
    {
        public GetWeatherByCityValidator()
        {
            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required.")
                .MinimumLength(2).WithMessage("City must have at least 2 characters");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherHub.Application.Common.Behaviors
{
    using FluentValidation;
    using FluentValidation.Results;
    using MediatR;
    using WeatherHub.Application.Common.Exceptions;

    public class ValidationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                var failures = validationResults
                    .Where(r => r != null)
                    .SelectMany(r => r.Errors ?? Enumerable.Empty<ValidationFailure>())
                    .ToList();

                if (failures.Count != 0)
                {
                    throw new AppException(
                        string.Join(", ", failures.Select(f => f.ErrorMessage)),
                        400);
                }
            }

            return await next();
        }
    }
}

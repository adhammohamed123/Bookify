using Bookify.Application.Abstractions.Messaging;
using FluentValidation;
using MediatR;

namespace Bookify.Application.Abstractions.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IBaseCommand
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
            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);

            var validationErrors = _validators
                .Select(validator => validator.Validate(context))
                .Where(validationResult => validationResult.Errors.Any())
                .SelectMany(validationResult => validationResult.Errors)
                .Select(validationFailure => new ValidationError(
                    validationFailure.PropertyName,
                    validationFailure.ErrorMessage))
                .ToList();

            if (validationErrors.Any())
            {
                throw new ValidationException(validationErrors);
            }

            return await next();
        }
    }

   

    public sealed class ValidationException : Exception
    {
        public ValidationException(IEnumerable<ValidationError> errors)
        {
            Errors = errors;
        }

        public IEnumerable<ValidationError> Errors { get; }
    }
    public sealed record ValidationError(string PropertyName, string ErrorMessage);


    //public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    //    where TRequest : IBaseCommand
    //{
    //    private readonly IValidator? validator;

    //    public ValidationBehavior(IValidator? validator)
    //    {
    //        this.validator = validator;
    //    }
    //    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    //    {
    //       if (validator is null)
    //       {
    //            return await next(cancellationToken);
    //       }
    //       var context = new ValidationContext<TRequest>(request);
    //       var validationResult = validator.Validate(context);
    //       if (!validationResult.IsValid)
    //       {
    //         throw new ValidationException(validationResult.Errors);
    //       }
    //       return await next(cancellationToken);
    //    }
    //}
}

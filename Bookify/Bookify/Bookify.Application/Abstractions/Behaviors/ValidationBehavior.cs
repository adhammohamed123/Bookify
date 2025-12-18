using Bookify.Application.Abstractions.Messaging;
using FluentValidation;
using MediatR;

namespace Bookify.Application.Abstractions.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IBaseCommand
    {
        private readonly IValidator? validator;

        public ValidationBehavior(IValidator? validator)
        {
            this.validator = validator;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
           if (validator is null)
           {
                return await next(cancellationToken);
           }
           var context = new ValidationContext<TRequest>(request);
           var validationResult = validator.Validate(context);
           if (!validationResult.IsValid)
           {
             throw new ValidationException(validationResult.Errors);
           }
           return await next(cancellationToken);
        }
    }
}

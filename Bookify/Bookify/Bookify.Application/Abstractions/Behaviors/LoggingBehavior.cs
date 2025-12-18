using Bookify.Application.Abstractions.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookify.Application.Abstractions.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
       where TRequest : IBaseCommand
    {
        private readonly ILogger<TRequest> logger;

        public LoggingBehavior(ILogger<TRequest> logger)
        {
            this.logger = logger;
        }

        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            try
            {
                logger.LogInformation("Handling request {RequestName}", requestName);
                var response = next();
                logger.LogInformation("Request {RequestName} handled successfully", requestName);
                return response;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Request {RequestName} failed with exception: {ExceptionMessage}", requestName, ex.Message);
                throw;
            }
        }
    }
}

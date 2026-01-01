using Bookify.Application.Abstractions.Caching;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookify.Application.Abstractions.Behaviors
{
    public class CacheQueryBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICachedQuery
        where TResponse : Result
    {
        private readonly ICacheService cacheService;
        private readonly ILogger<CacheQueryBehavior<TRequest, TResponse>> logger;

        public CacheQueryBehavior(ICacheService cacheService,ILogger<CacheQueryBehavior<TRequest,TResponse>> logger)
        {
            this.cacheService = cacheService;
            this.logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var result = await cacheService.GetAsync<TResponse>(request.CacheKey, cancellationToken);
            var requestName = typeof(TRequest).Name;

            if(result is not null)
            {
                logger.LogInformation("Cache Hit For {Query}", requestName);
                return result;
            }
            logger.LogInformation("Cache Miss For {Query}", requestName);
            
            var call= await next(cancellationToken);

            if(call.IsSuccess)
            {
               await cacheService.SetAsync<TResponse>(request.CacheKey, call, request.Expiration, cancellationToken);
            }

            return call;
        }
    }
}

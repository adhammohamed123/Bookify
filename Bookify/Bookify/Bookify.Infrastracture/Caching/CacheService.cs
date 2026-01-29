using Bookify.Application.Abstractions.Caching;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Bookify.Infrastracture.Caching
{
    internal sealed class CacheService(IDistributedCache cache) : ICacheService
    {
        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            var value=  await cache.GetAsync(key, cancellationToken);
            return (value is null ? default : JsonSerializer.Deserialize<T>(value) )!;
        }

      

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        => await cache.RemoveAsync(key, cancellationToken);
        

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            var cachingOptions = new DistributedCacheEntryOptions();
            if (expiration == null)
               cachingOptions. AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
            else
                cachingOptions.AbsoluteExpirationRelativeToNow = expiration;

           var valueSerialized = JsonSerializer.SerializeToUtf8Bytes<T>(value);

           await cache.SetAsync(key,valueSerialized,cachingOptions , cancellationToken);
        }
    }
}

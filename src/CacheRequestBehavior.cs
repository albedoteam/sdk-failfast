namespace AlbedoTeam.Sdk.FailFast
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Abstractions;
    using Cache.Abstractions;
    using Cache.Attributes;
    using MediatR;

    public class CacheRequestBehavior<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
        where TRequest : IRequest<TResult>
        where TResult : class, IResult, new()
    {
        private readonly ICacheService _cache;

        public CacheRequestBehavior(ICacheService cache)
        {
            _cache = cache;
        }

        public Task<TResult> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResult> next)
        {
            var cacheAttribute = Attribute
                .GetCustomAttributes(request.GetType())
                .FirstOrDefault(a => a is CacheAttribute);

            if (cacheAttribute is null || request is ICachedRequest<TResult> {NoCache: true})
                return next();

            var cached = _cache.TryGet<TRequest, TResult>(request, out var cachedResult);
            if (cached)
                return Task.FromResult(cachedResult);

            var innerResult = next().Result;
            _cache.Set(request, innerResult, ((CacheAttribute) cacheAttribute).ExpirationInSeconds);
            return Task.FromResult(innerResult);
        }
    }
}
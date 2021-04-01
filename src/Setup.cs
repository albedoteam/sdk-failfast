namespace AlbedoTeam.Sdk.FailFast
{
    using System;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;

    public static class Setup
    {
        public static IServiceCollection AddFailFastRequest(this IServiceCollection services, Type handlerAssemblyType)
        {
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CacheRequestBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(FailFastRequestBehavior<,>));
            services.AddMediatR(handlerAssemblyType);

            return services;
        }
    }
}
namespace AlbedoTeam.Sdk.FailFast
{
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public static class Setup
    {
        public static IServiceCollection AddFailFastRequest(this IServiceCollection services)
        {
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CacheRequestBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(FailFastRequestBehavior<,>));
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
    }
}
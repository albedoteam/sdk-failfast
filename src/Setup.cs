using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AlbedoTeam.Sdk.FailFast
{
    public static class Setup
    {
        public static IServiceCollection AddFailFastRequest(this IServiceCollection services, Type handlerAssemblyType)
        {
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(FailFastRequestBehavior<,>));
            services.AddMediatR(handlerAssemblyType);

            return services;
        }
    }
}
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CaseItau.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        //MediatR
        services.AddMediatR(typeof(DependencyInjection));

        return services;
    }
}

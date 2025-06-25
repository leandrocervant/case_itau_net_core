using CaseItau.Infrastructure.Common.Middleware;
using Microsoft.AspNetCore.Builder;

namespace CaseItau.Infrastructure;

public static class RequestPipeline
{
    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<EventualConsistencyMiddleware>();

        return builder;
    }
}

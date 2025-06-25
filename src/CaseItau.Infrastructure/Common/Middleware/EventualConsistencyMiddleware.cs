using CaseItau.Domain.Common.Interfaces;
using CaseItau.Infrastructure.Persistence.Contexts;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CaseItau.Infrastructure.Common.Middleware;

internal class EventualConsistencyMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context, IPublisher publisher, FundsDbContext dbContext)
    {
        var transaction = await dbContext.Database.BeginTransactionAsync();

        try
        {
            await _next(context);

            if (
                context.Items.TryGetValue("DomainEventsQueue", out var value) &&
                value is Queue<IDomainEvent> domainEventsQueue)
            {
                while (domainEventsQueue!.TryDequeue(out var domainEvent))
                {
                    await publisher.Publish(domainEvent);
                }
            }

            await transaction.CommitAsync();
        }
        catch (Exception)
        {

        }
        finally
        {
            await transaction.DisposeAsync();
        }
    }
}

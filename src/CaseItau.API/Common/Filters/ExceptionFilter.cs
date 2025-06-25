using CaseItau.Domain.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CaseItau.API.Common.Filters;

public class ExceptionFilter : IExceptionFilter
{
    private static readonly Type[] _knownTypes =
    [
        typeof(DomainException),
    ];

    public void OnException(ExceptionContext context)
    {
        context.ExceptionHandled = true;

        var requestId = context.HttpContext.TraceIdentifier;

        if (_knownTypes.Any(type => type.IsInstanceOfType(context.Exception)))
        {
            context.Result = new BadRequestObjectResult(
                new
                {
                    requestId,
                    message = context.Exception.Message
                });
        }
        else
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ExceptionFilter>>();

            logger.LogError(
                context.Exception,
                "One or more error has occurried. RequestId {requestId}",
                requestId);

            var result = new ObjectResult(
                new
                {
                    requestId,
                    message = context.Exception.ToString(),
                })
            {
                StatusCode = 500
            };

            context.Result = result;
        }
    }
}

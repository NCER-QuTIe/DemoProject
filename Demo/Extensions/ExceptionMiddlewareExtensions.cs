using Contracts;
using Contracts.Logger;
using Microsoft.AspNetCore.Diagnostics;
using Entities.ErrorModel;
using System.Net;
using Entities.Exceptions;
using Entities.Enums;

namespace CompanyEmployees.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILoggerManager logger)
    {
        app.UseExceptionHandler(error =>
        {
            error.Run(async context =>
            {
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    (int statusCode, LogLevelEnum logLevel)  = contextFeature.Error switch
                    {
                        NotAccessibleException => (StatusCodes.Status403Forbidden, LogLevelEnum.Warning),
                        NotFoundException => (StatusCodes.Status404NotFound, LogLevelEnum.Warning),
                        BadRequestException => (StatusCodes.Status400BadRequest, LogLevelEnum.Warning),
                        _ => (StatusCodes.Status500InternalServerError, LogLevelEnum.Error)
                    };

                    context.Response.StatusCode = statusCode;
                    logger.Log(contextFeature.Error.Message, logLevel);

                    await context.Response.WriteAsync(new ErrorDetails()
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = contextFeature.Error.Message
                    }.ToString());
                }
            });
        });
    }
}

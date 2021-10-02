using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RentItAPI.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RentItAPI.Middleware
{
    public class ErrorHandlingMiddleWare : IMiddleware
    {
        private readonly ILogger _logger;
        public ErrorHandlingMiddleWare(ILogger<ErrorHandlingMiddleWare> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try 
            {
                await next.Invoke(context);
            }
           catch (BadRequestException badRequestException)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(badRequestException.Message);

            }
            catch (NotFoundException notFoundException)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(notFoundException.Message);
            }
            
            catch (ExternalServerError externalServerError)
            {
                _logger.LogError(externalServerError, externalServerError.Message);
                context.Response.StatusCode = 502;
                await context.Response.WriteAsync(externalServerError.Message);
            }
            catch (FileNotFoundException fileNotFoundException)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(fileNotFoundException.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Internal server error.");
            }
        }
    }
}

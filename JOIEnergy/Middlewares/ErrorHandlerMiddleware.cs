using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using JOIEnergy.Application.Exception;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace JOIEnergy.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case ApiException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;

                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;

                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
                var result = JsonSerializer.Serialize(error.Message);

                await response.WriteAsync(result);
            }
        }
    }

    public static class ErrorHandlerExtension
    {
        public static void UseErrorHandler(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}

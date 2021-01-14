using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace SEOAssistant.UI.Models
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ILogger logger) {
            try {

                // Call the next delegate/middleware in the pipeline
                await _next(context);
            }
            catch (Exception error) {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error) {                    
                    case KeyNotFoundException e:
                        // not found error
                        logger.LogError(e.Message);
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        logger.LogCritical(error.Message);
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(new { message = error?.Message });
                await response.WriteAsync(result);
            }
        }
    }

}

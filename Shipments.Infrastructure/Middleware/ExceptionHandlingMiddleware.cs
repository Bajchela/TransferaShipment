using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Shipments.Application.Exceptions;

namespace Shipments.Infrastructure.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        public const string CorrelationIdHeader = "X-Correlation-Id";
        private readonly RequestDelegate _next;
        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var correlationId = GetOrCreateCorrelationId(context);
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, correlationId);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex, string correlationId)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            PublicExceptionResponse responseJson = new PublicExceptionResponse
            {
                ErrorCode = 500,
                IsAuthenticationFailed = false,
                CorrelationId = correlationId
            };

            switch (ex)
            {
                case AppException appEx:
                    response.StatusCode = appEx.StatusCode;
                    responseJson.ErrorCode = appEx.ErrorCode;
                    responseJson.IsAuthenticationFailed = appEx.IsAuthenticationFailed;
                    responseJson.FrontDebugMessage = appEx.Message;
                    break;           

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    responseJson.FrontDebugMessage = "An unexpected error has occurred.";
                    break;
            }                  

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var stringResponse = JsonSerializer.Serialize(responseJson, options);

            response.ContentLength = stringResponse.Length;
            return response.WriteAsync(stringResponse);
        }

        public class PublicExceptionResponse
        {
            /// <summary>
            /// Public error code used to represent to the client  the error.
            /// </summary>
            public int ErrorCode { get; set; } = 500;
            public string? FrontDebugMessage { get; set; }
            public bool IsAuthenticationFailed { get; set; } = false;
            public string? CorrelationId { get; set; }
        }

        private static string GetOrCreateCorrelationId(HttpContext context)
        {
            // Prioritet: header -> Activity -> TraceIdentifier -> new Guid
            var headerValue = context.Request.Headers[CorrelationIdHeader].ToString();
            if (!string.IsNullOrWhiteSpace(headerValue))
                return headerValue;

            var activityId = Activity.Current?.TraceId.ToString();
            if (!string.IsNullOrWhiteSpace(activityId))
                return activityId;

            if (!string.IsNullOrWhiteSpace(context.TraceIdentifier))
                return context.TraceIdentifier;

            return Guid.NewGuid().ToString("N");
        }

    }
}

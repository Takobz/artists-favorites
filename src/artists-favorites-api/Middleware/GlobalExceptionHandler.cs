using System.Net;
using artists_favorites_api.Constants;
using artists_favorites_api.Exceptions;
using artists_favorites_api.Models.DTOs.Responses;

namespace artists_favorites_api.Middleware 
{
    public class GlobalExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlerMiddleware> logger
    ) 
    {
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try 
            {
                await next(httpContext);
            }
            catch(ArtistsFavoritesHttpException exception)
            {
                await UseCustomExceptionHandler(httpContext, exception);
            }
            catch(Exception exception)
            {
                await UseCustomExceptionHandler(httpContext, exception);
            }
            
        }

        public async Task UseCustomExceptionHandler(
            HttpContext context,
            Exception exception)
        {
            if (exception == null || exception?.InnerException == null)
                await WriteErrorResponse(context);

            logger.LogError("Exception thrown was not custom type: {CustomExceptionType} but was {ExceptionType} with message {ExceptionMessage}",
                typeof(ArtistsFavoritesHttpException),
                exception?.GetType() ?? exception?.InnerException?.GetType(),
                exception?.Message ?? exception?.InnerException?.Message ?? "No Exception Message"
            );

            await WriteErrorResponse(context);
        }

        public async Task UseCustomExceptionHandler(
            HttpContext context,
            ArtistsFavoritesHttpException exception
        )
        {
            await WriteErrorResponse(
                context,
                exception?.HttpStatusCode ?? (int)HttpStatusCode.InternalServerError,
                exception?.Message ?? FriendlyErrorMessage.GenericInternalAppError
            );
        }

        private static async Task WriteErrorResponse(
            HttpContext context,
            int statusCode = 500,
            string message = FriendlyErrorMessage.GenericInternalAppError)
        {
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(
                new ErrorResponseDTO (
                    statusCode,
                    message
                )
            );
        }
    }
}

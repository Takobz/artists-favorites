using artists_favorites_api.Constants;
using artists_favorites_api.Exceptions;
using artists_favorites_api.Models.DTOs.Responses;
using Microsoft.AspNetCore.Diagnostics;

namespace artists_favorites_api.Middleware 
{
    public static class GlobalExceptionHandler 
    {
        public static WebApplication UseCustomExceptionHandler(this WebApplication app)
        {
            app.UseExceptionHandler(appBuilder => {
                appBuilder.Run(async context => {
                    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (exceptionHandlerFeature == null || 
                        (exceptionHandlerFeature?.Error == null && exceptionHandlerFeature?.Error?.InnerException == null)) 
                    {
                        await context.WriteErrorResponse();
                        return;
                    }

                    if (exceptionHandlerFeature!.Error?.GetType() != typeof(ArtistsFavoritesHttpException) ||
                        exceptionHandlerFeature!.Error?.InnerException?.GetType() != typeof(ArtistsFavoritesHttpException)) 
                    {
                        await context.WriteErrorResponse();
                        return;
                    }

                    var applicationException = exceptionHandlerFeature.Error as ArtistsFavoritesHttpException ??
                         exceptionHandlerFeature.Error?.InnerException as ArtistsFavoritesHttpException;
                    if (applicationException == null) 
                    {
                        await context.WriteErrorResponse();
                        return;
                    }

                    await context.WriteErrorResponse(
                        applicationException.HttpStatusCode,
                        applicationException.Message
                    );
                    
                });
            });

            return app;
        }

        private static async Task WriteErrorResponse(
            this HttpContext context,
            int statusCode = 500,
            string message = FriendlyErrorMessage.GenericInternalAppError)
        {
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(
                new ErrorResponse (
                    statusCode,
                    message
                )
            );
        }
    }
}

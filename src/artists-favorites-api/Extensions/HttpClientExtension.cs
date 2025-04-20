using System.Net;
using System.Text.Json;
using artists_favorites_api.Constants;
using artists_favorites_api.Exceptions;

namespace artists_favorites_api.Extensions 
{
    public static class HttpClientExtension
    {
        /// <summary>
        /// Send HTTP call to remote server and converts expected data to type TValue
        /// </summary>
        /// <typeparam name="TValue">The Type we want to get from the http call</typeparam>
        /// <typeparam name="TClientType">The Type of client class</typeparam>
        /// <param name="httpClient">The httpClient configured to call the remote server</param>
        /// <param name="request">HttpRequestMessage with needed data to call the remote server</param>
        /// <param name="logger">ILogger for the calling client for logging purposes</param>
        /// <returns>CSharp class that represents the expected data on successful call</returns>
        /// <exception cref="ArtistsFavoritesHttpException">Thrown for non success httpclient responses</exception>
        public static async Task<TValue> SpotifySendAsyncAndReturnResponse<TValue, TClientType>(
            this HttpClient httpClient,
            HttpRequestMessage request,
            ILogger<TClientType> logger
        ) 
        where TValue : class
        where TClientType : class
        {
            var response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var result = await JsonSerializer.DeserializeAsync<TValue>(
                    await response.Content.ReadAsStreamAsync()
                );

                if (result == null)
                {
                    logger.LogError("Failed to deserialize Http Content: {HttpResponseContent} to class: {ClassType}",
                        await response.Content.ReadAsStringAsync() ?? string.Empty,
                        nameof(TValue)
                    );

                    throw new ArtistsFavoritesHttpException(
                        (int)HttpStatusCode.InternalServerError,
                        FriendlyErrorMessage.GenericInternalAppError
                    );
                } 

                return result;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new ArtistsFavoritesHttpException(
                    (int)HttpStatusCode.BadRequest,
                    FriendlyErrorMessage.BadOrInvalid
                );
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new ArtistsFavoritesHttpException(
                    (int)HttpStatusCode.Forbidden,
                    FriendlyErrorMessage.ForbiddenAccess
                );
            }

            throw new ArtistsFavoritesHttpException(
                (int)HttpStatusCode.InternalServerError,
                FriendlyErrorMessage.GenericInternalAppError
            );
        }
    }
}
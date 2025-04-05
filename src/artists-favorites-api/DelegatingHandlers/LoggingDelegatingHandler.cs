namespace artists_favorites_api.DelegatingHandlers 
{
    public class LoggingDelegatingHandler(ILogger<LoggingDelegatingHandler> logger) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Begin Request with RequestURI: {RequestUri}", request.RequestUri);

            var result =  await base.SendAsync(request, cancellationToken);        

            logger.LogInformation("After Request RequestURI: {RequestUri}, StatusCode: {StatusCode}",
                request.RequestUri, result.StatusCode);

            return result;
        }
    }
}
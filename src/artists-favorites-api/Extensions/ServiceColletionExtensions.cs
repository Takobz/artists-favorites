using artists_favorites_api.AuthProviders;
using artists_favorites_api.Clients.Spotify;
using artists_favorites_api.DelegatingHandlers;
using artists_favorites_api.Models.Options;
using artists_favorites_api.Services;

namespace artists_favorites_api.Extensions 
{
    public static class ServiceCollectionExtensions 
    {
        public static IServiceCollection AddSpotifyServices (this IServiceCollection services, IConfiguration configuration)
        {
            //Delegating Handlers
            services.AddTransient<SpotifyClientCredentialsHandler>();
            services.AddTransient<LoggingDelegatingHandler>();

            //http clients
            services.AddHttpClient<ISpotifySearchClient, SpotifySearchClient>(client => {
                var options = configuration.GetSection(SpotifyOptions.Section).Get<SpotifyOptions>();
                client.BaseAddress = new Uri(options!.SpotifySearchV1Url);
            })
            .AddHttpMessageHandler<LoggingDelegatingHandler>()
            .AddHttpMessageHandler<SpotifyClientCredentialsHandler>();

            services.AddHttpClient<ISpotifyAuthProvider, SpotifyAuthProvider>()
                .ConfigurePrimaryHttpMessageHandler(serviceProvider => {
                    return new HttpClientHandler {
                        AllowAutoRedirect = false
                    };
                });

            //Services
            services.Configure<SpotifyOptions>(configuration.GetSection(SpotifyOptions.Section));
            services.AddTransient<ISpotifySearchService, SpotifySearchService>();

            return services;
        }
    }
}
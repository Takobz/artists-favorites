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
            services.AddHttpClient<ISpotifyClient, SpotifyClient>(client => {
                var options = configuration.GetSection(SpotifyOptions.Section).Get<SpotifyOptions>();
                client.BaseAddress = new Uri(options!.SpotifyAuthUrl);
            })
            .AddHttpMessageHandler<LoggingDelegatingHandler>()
            .AddHttpMessageHandler<SpotifyClientCredentialsHandler>();

            //Services
            services.Configure<SpotifyOptions>(configuration.GetSection(SpotifyOptions.Section));
            services.AddTransient<ISpotifyAuthProvider, SpotifyAuthProvider>();
            services.AddTransient<ISpotifySearchService, SpotifySearchService>();

            return services;
        }
    }
}
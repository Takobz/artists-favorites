using artists_favorites_api.Authentication;
using artists_favorites_api.Clients.Spotify;
using artists_favorites_api.Constants;
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
            services.AddTransient<SpotifyClientCredentialsDelegatingHandler>();
            services.AddTransient<LoggingDelegatingHandler>();
            services.AddTransient<SpotifyUserAccessTokenDelegatingHandler>();

            //Add HttpClient
            var options = configuration.GetRequiredSection(SpotifyOptions.Section).Get<SpotifyOptions>();
            services.AddSpotifyHttpClients(options!);

            //Services
            services.Configure<SpotifyOptions>(configuration.GetSection(SpotifyOptions.Section));
            services.AddTransient<ISpotifySearchService, SpotifySearchService>();
            services.AddTransient<ISpotifyPlaylistService, SpotifyPlaylistService>();
            services.AddTransient<ISpotifyTrackService, SpotifyTrackService>();

            return services;
        }

        public static IServiceCollection AddSpotifyAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication()
                .AddScheme<SpotifyAuthenticationSchemeOptions, SpotifyAuthenticationHandler>(
                    SpotifyAuthenticationDefaults.AuthenticationScheme,
                    options => {}
            );

            services.AddAuthorizationBuilder()
                .AddPolicy(SpotifyAuthenticationCustomPolicies.SpotifyUser, policy => {
                    policy.RequireClaim(SpotifyAuthenticationCustomClaims.SpotifyAccessToken);
                    policy.RequireClaim(SpotifyAuthenticationCustomClaims.SpotifyUserEntityId);
            });

            return services;
        }

        private static IServiceCollection AddSpotifyHttpClients(this IServiceCollection services, SpotifyOptions options) 
        {
            services.AddHttpClient<ISpotifyPlaylistClient, SpotifyPlaylistClient>(client => {
                client.BaseAddress = new Uri($"{options!.SpotifyV1Url}");
            })
            .AddHttpMessageHandler<LoggingDelegatingHandler>()
            .AddHttpMessageHandler<SpotifyUserAccessTokenDelegatingHandler>();

             services.AddHttpClient<ISpotifyTrackClient, SpotifyTrackClient>(client => {
                client.BaseAddress = new Uri($"{options!.SpotifyV1Url}/tracks");
            })
            .AddHttpMessageHandler<LoggingDelegatingHandler>()
            .AddHttpMessageHandler<SpotifyUserAccessTokenDelegatingHandler>();

            services.AddHttpClient<ISpotifySearchClient, SpotifySearchClient>(client => {
                client.BaseAddress = new Uri($"{options.SpotifyV1Url}/search");
            })
            .AddHttpMessageHandler<LoggingDelegatingHandler>()
            .AddHttpMessageHandler<SpotifyClientCredentialsDelegatingHandler>();

            //TODO: Move SpotifyUserClient into SpotifyAuthProvider
            //TODO: rename SpotifyAuthProvider to SpotifyAuthServerClient
            services.AddHttpClient<ISpotifyUserClient, SpotifyUserClient>(client => {
                client.BaseAddress = new Uri($"{options.SpotifyV1Url}/me");
            })
            .AddHttpMessageHandler<LoggingDelegatingHandler>();

            services.AddHttpClient<ISpotifyAuthProvider, SpotifyAuthProvider>()
                .AddHttpMessageHandler<LoggingDelegatingHandler>()
                .ConfigurePrimaryHttpMessageHandler(serviceProvider => {
                    return new HttpClientHandler {
                        AllowAutoRedirect = false
                    };
                });

            return services;
        }
    }
}
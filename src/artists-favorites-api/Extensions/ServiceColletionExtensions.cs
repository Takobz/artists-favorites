using artists_favorites_api.AuthProviders;
using artists_favorites_api.Models.Options;

namespace artists_favorites_api.Extensions 
{
    public static class ServiceCollectionExtensions 
    {
        public static IServiceCollection AddSpotifyServices (this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SpotifyOptions>(configuration.GetSection(SpotifyOptions.Section));
            services.AddTransient<ISpotifyAuthProvider, SpotifyAuthProvider>();
            return services;
        }
    }
}
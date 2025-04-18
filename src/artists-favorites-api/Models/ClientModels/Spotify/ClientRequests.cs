using System.Text.Json.Serialization;
using artists_favorites_api.Constants;

namespace  artists_favorites_api.Models.ClientModels.Spotify 
{
    public record AuthorizeUserRequest (
        [property: JsonPropertyName("client_id")] string ClientId,
        [property: JsonPropertyName("redirect_uri")] string RedirectUri,
        [property: JsonPropertyName("state")] string State,
        [property: JsonPropertyName("scope")] string Scope,
        [property: JsonPropertyName("show_dialog")] bool ShowDialog = true,
        [property: JsonPropertyName("response_type")] string ResponseType = SpotifyUserAuthorizationCodes.Code
    );

    public record CreatePlaylist(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("description")] string Description,
        [property: JsonPropertyName("public")] bool Public
    );
}
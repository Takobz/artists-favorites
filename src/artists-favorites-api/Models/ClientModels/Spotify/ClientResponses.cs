using System.Text.Json.Serialization;

namespace artists_favorites_api.Models.ClientModels.Spotify 
{
    public record SpotifyCurrentUserProfileResponse(
        [property: JsonPropertyName("country")] string Country,
        [property: JsonPropertyName("display_name")] string DisplayName,
        [property: JsonPropertyName("email")] string Email
    ) : SpotifyEntity;

    public record SpotifyPlaylistResponse(
        [property: JsonPropertyName("tracks")] SpotifySearchResponse<SpotifyTrack> Tracks,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("description")] string Description = ""
    ) : SpotifyEntity;

    public record BaiscAccessTokenResponse (
        [property: JsonPropertyName("access_token")] string AccessToken,
        [property: JsonPropertyName("token_type")] string TokenType,
        [property: JsonPropertyName("expires_in")] int ExpiresIn);

    public record AuthorizationCodeAccessTokenResponse(
        [property: JsonPropertyName("access_token")] string AccessToken,
        [property: JsonPropertyName("token_type")] string TokenType,
        [property: JsonPropertyName("expires_in")] int ExpiresIn,
        [property: JsonPropertyName("refresh_token")] string RefreshToken,
        [property: JsonPropertyName("scope")] string Scope
    );

    public record SpotifySearchResponses(
        [property: JsonPropertyName("artists")] SpotifySearchResponse<SpotifyArtist> ArtistsSearchResponses,
        [property: JsonPropertyName("tracks")] SpotifySearchResponse<SpotifyTrack> TracksSearchResponses
    );

    public record SpotifySearchResponse<T>(
        [property: JsonPropertyName("href")] string HRef,
        [property: JsonPropertyName("limit")] int Limit,
        [property: JsonPropertyName("next")] string Next,
        [property: JsonPropertyName("previous")] string Previous,
        [property: JsonPropertyName("offset")] int Offset,
        [property: JsonPropertyName("total")] int Total,
        [property: JsonPropertyName("items")] IEnumerable<T> Items 
    ) where T : SpotifyEntity; 


    public record SpotifyArtist (
        [property: JsonPropertyName("external_urls")] SpotifyExternalUrls ExternalUrls,
        [property: JsonPropertyName("followers")] SpotifyFollowers Followers,
        [property: JsonPropertyName("genres")] IEnumerable<string> Genres,
        [property: JsonPropertyName("images")] IEnumerable<SpotifyImage> Images,
        [property: JsonPropertyName("name")] string ArtistName,
        [property: JsonPropertyName("popularity")] int ArtistPopularity
    ) : SpotifyEntity();

    public record SpotifySavedTrack(
        [property: JsonPropertyName("added_at")] string AddedAt,
        [property: JsonPropertyName("track")] SpotifyTrack Track
    ) : SpotifyEntity;

    public record SpotifyTrack (
        [property: JsonPropertyName("artists")] IEnumerable<SpotifySimplifiedArtist> Artists,
        [property: JsonPropertyName("name")] string TrackName,
        [property: JsonPropertyName("album")] SpotifyAlbum Album
    ) : SpotifyEntity;

    public record SpotifySimplifiedArtist(
        [property: JsonPropertyName("name")] string ArtistName
    ) : SpotifyEntity;

    public record SpotifyAlbum(
        [property: JsonPropertyName("album_type")] string AlbumType,
        [property: JsonPropertyName("total_tracks")] int TotalTracks,
        [property: JsonPropertyName("release_date")] string ReleaseDate,
        [property: JsonPropertyName("artists")] IEnumerable<SpotifySimplifiedArtist> Artists,
        [property: JsonPropertyName("images")] IEnumerable<SpotifyImage> Images
    ) : SpotifyEntity;

    public record SpotifyExternalUrls(
        [property: JsonPropertyName("spotify")] string SpotifyUrl);

    public record SpotifyFollowers(
        [property: JsonPropertyName("href")] string HRef,
        [property: JsonPropertyName("total")] int Total);

    public record SpotifyImage(
        [property: JsonPropertyName("url")] string ImageUrl,
        [property: JsonPropertyName("height")] int Height,
        [property: JsonPropertyName("width")] int Width
    );

    public record SpotifyEntity 
    {
        [JsonPropertyName("id")] 
        public string EntityId { get; init; } = string.Empty;

        [JsonPropertyName("href")] 
        public string HRef { get; init; } = string.Empty;

        [JsonPropertyName("uri")] 
        public string SpotifyUri { get; init; } = string.Empty;

        [JsonPropertyName("type")]
        public string EntityType { get; init; } = string.Empty;
    };
}
namespace artists_favorites_api.Models.ServiceModels.SpotifyServiceModels.Commands 
{
    public record CreateEmptyPlaylist(
        string PlaylistName,
        string PlaylistDescription,
        bool IsPublicPlaylist,
        string AccessToken,
        string RefreshToken = "" //To add functionaliy.
    );
}
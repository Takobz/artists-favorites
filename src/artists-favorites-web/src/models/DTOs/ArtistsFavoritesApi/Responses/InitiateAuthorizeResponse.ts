export class InitiateAuthorizeResponse {
    spotifyAuthorizeUrl: string;

    constructor(spotifyAuthorizeUrl: string){
        this.spotifyAuthorizeUrl = spotifyAuthorizeUrl;
    }
}
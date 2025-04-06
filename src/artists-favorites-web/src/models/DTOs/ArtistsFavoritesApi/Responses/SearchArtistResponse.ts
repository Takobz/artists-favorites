export class SearchArtistResponse 
{
    name: string;
    spotifyUrl: string;
    imageUrl: string;

    constructor(name: string, spotifyUrl: string, imageUrl: string){
        this.name = name;
        this.spotifyUrl = spotifyUrl;
        this.imageUrl = imageUrl;
    }
}
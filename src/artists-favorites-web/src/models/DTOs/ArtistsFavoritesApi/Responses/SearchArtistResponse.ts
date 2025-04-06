export class SearchArtistResponse 
{
    name: string;
    spotifyUrl: string;
    imageUrl: string;
    artistPopularity: number;

    constructor(name: string, spotifyUrl: string, imageUrl: string, artistPopularity: number){
        this.name = name;
        this.spotifyUrl = spotifyUrl;
        this.imageUrl = imageUrl;
        this.artistPopularity = artistPopularity;
    }
}
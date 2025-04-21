export class SearchArtistResponse 
{
    name: string;
    spotifyUrl: string;
    imageUrl: string;
    artistPopularity: number;
    artistEntityId: string;

    constructor(
        name: string,
        spotifyUrl: string, 
        imageUrl: string, 
        artistPopularity: number,
        artistEntityId: string
    )
    {
        this.name = name;
        this.spotifyUrl = spotifyUrl;
        this.imageUrl = imageUrl;
        this.artistPopularity = artistPopularity;
        this.artistEntityId = artistEntityId;
    }
}
import { SearchArtistResponse } from "../models/DTOs/ArtistsFavoritesApi/Responses/SearchArtistResponse"

interface IArtistsFavoritesApiService {
    searchArtistByName(name: string) : SearchArtistResponse[]
}

class ArtistsFavoritesApiService implements IArtistsFavoritesApiService {
    searchArtistByName(name: string): SearchArtistResponse[] {
        //TODO: add Axios calls to the backend to get artists response.

        return [];
    }
}
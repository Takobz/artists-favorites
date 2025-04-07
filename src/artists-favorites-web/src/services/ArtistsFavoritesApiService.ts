import axios from "axios";
import { SearchArtistResponse } from "../models/DTOs/ArtistsFavoritesApi/Responses/SearchArtistResponse"
import { ErrorModel } from "../models/ScreenModels/ErrorModel";

const baseUrl = import.meta.env.VITE_ArtistsFavoritesApi_BASE_URL;

interface IArtistsFavoritesApiService {
    searchArtistByName(name: string) : Promise<SearchArtistResponse[]>
}

export class ArtistsFavoritesApiService implements IArtistsFavoritesApiService {
    async searchArtistByName(name: string): Promise<SearchArtistResponse[]> {
        var artistResult : SearchArtistResponse[] = [];
        await axios.get<SearchArtistResponse[]>(`${baseUrl}/v1/search/${name}`).then(response => {
            if (response.status && response.status === 200){
                artistResult = response.data;
            }
        }).catch(err => {
            console.log(err); //TODO: use better logger service
            const error : ErrorModel = {
                displayMessage: `Error Getting Artist ${name}`,
                errorCode: "APICallError"
            }
            //return error;
        });

        return artistResult;
    }
}
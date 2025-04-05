import axios from "axios";
import { SearchArtistResponse } from "../models/DTOs/ArtistsFavoritesApi/Responses/SearchArtistResponse"
import { ErrorModel } from "../models/ScreenModels/ErrorModel";

interface IArtistsFavoritesApiService {
    searchArtistByName(name: string) : Promise<SearchArtistResponse[] | ErrorModel>
}

export default class ArtistsFavoritesApiService implements IArtistsFavoritesApiService {
    async searchArtistByName(name: string): Promise<SearchArtistResponse[] | ErrorModel> {
        var artistResult : SearchArtistResponse[] = []; 
        await axios.get<SearchArtistResponse[]>("").then(response => {
            if (response.status && response.status === 200){
                artistResult = response.data;
            }
        }).catch(err => {
            console.log(err); //TODO: use better logger service
            const error : ErrorModel = {
                displayMessage: `Error Getting Artist ${name}`,
                errorCode: "APICallError"
            }
            return error;
        });

        return artistResult;
    }
}
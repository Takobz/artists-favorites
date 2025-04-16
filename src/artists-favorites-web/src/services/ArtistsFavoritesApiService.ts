import axios from "axios";
import { SearchArtistResponse } from "../models/DTOs/ArtistsFavoritesApi/Responses/SearchArtistResponse"
import { GetUserTokenResponse } from "../models/DTOs/ArtistsFavoritesApi/Responses/GetUserTokenResponse";
import { GetUserTokenRequest } from "../models/DTOs/ArtistsFavoritesApi/Requests/GetUserTokenRequest";
import { InitiateAuthorizeResponse } from '../models/DTOs/ArtistsFavoritesApi/Responses/InitiateAuthorizeResponse'
//import { ErrorModel } from "../models/ScreenModels/ErrorModel";

const baseUrl = import.meta.env.VITE_ArtistsFavoritesApi_BASE_URL;

interface IArtistsFavoritesApiService {
    searchArtistByName(name: string) : Promise<SearchArtistResponse[]>;
    initiateAuthorizeRequest() : Promise<InitiateAuthorizeResponse>;
    getUserAccessToken(code: string, state: string) : Promise<GetUserTokenResponse>;
}

export class ArtistsFavoritesApiService implements IArtistsFavoritesApiService {
    async searchArtistByName(name: string): Promise<SearchArtistResponse[]> {
        let artistResult : SearchArtistResponse[] = [];
        await axios.get<SearchArtistResponse[]>(`${baseUrl}/v1/search/${name}`).then(response => {
            if (response.status && response.status === 200){
                artistResult = response.data;
            }
        }).catch(err => {
            console.log(err); //TODO: use better logger service
            // const error : ErrorModel = {
            //     displayMessage: `Error Getting Artist ${name}`,
            //     errorCode: "APICallError"
            // }
            //return error;
        });

        return artistResult;
    }

    async initiateAuthorizeRequest(): Promise<InitiateAuthorizeResponse> {
        return await axios.get<InitiateAuthorizeResponse>(`${baseUrl}/initiate-authorize`).then(response => {
            if (response.status && response.status === 200){
                return response.data as InitiateAuthorizeResponse;
            }
            
            throw Error("Failed to initiate the authorize request");
        });
    }

    async getUserAccessToken(code: string, state: string): Promise<GetUserTokenResponse> {
        const request : GetUserTokenRequest = {
            authorizationCode: code,
            state: state 
        };

        const response = await axios.post<GetUserTokenResponse>(`${baseUrl}/spotify-user-token`, request).then(response => {
            if (!response.status) throw Error();

            if (response.status === 200){
                return response.data;
            }
            //TODO: handle 4xx's errors.

            throw Error();
        })

        return response;
    }
}
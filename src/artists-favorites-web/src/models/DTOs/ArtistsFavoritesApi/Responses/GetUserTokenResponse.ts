export class GetUserTokenResponse {
    accessToken: string;

    constructor(accessToken: string){
        this.accessToken = accessToken;
    }
}
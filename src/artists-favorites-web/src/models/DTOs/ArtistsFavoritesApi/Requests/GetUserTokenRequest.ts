export class GetUserTokenRequest 
{
    public authorizationCode: string;
    public state: string;

    constructor(
        authorizationCode: string,
        state: string) {
        this.authorizationCode = authorizationCode;
        this.state = state;
    }
}
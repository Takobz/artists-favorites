export class ErrorModel {
    displayMessage: string;
    errorCode: string;

    constructor(displayMessage: string, errorCode: string) {
        this.displayMessage = displayMessage;
        this.errorCode = errorCode;
    }
}
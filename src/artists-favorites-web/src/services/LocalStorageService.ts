export interface ILocalStorageService {
    setItemIntoLocalStorage(itemName: string, itemValue: string) : void;
    getItemFromLocalStorage(itemName: string) : string | null;
}

export class LocalStorageService implements ILocalStorageService {
    setItemIntoLocalStorage(itemName: string, itemValue: string): void {
        if (localStorage.getItem(itemName)){
            localStorage.removeItem(itemName);
        }
        localStorage.setItem(itemName, itemValue);
    }
    
    getItemFromLocalStorage(itemName: string) : string | null {
        return localStorage.getItem(itemName)
    }

}
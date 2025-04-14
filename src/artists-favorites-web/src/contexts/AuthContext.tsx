import { createContext } from "react";

export interface AuthContextProviderState {
    accessToken: string;
    refreshToken: string;
    state: string;
    setAccessToken: (accessToken: string) => void;
    setRefreshToken: (refreshToken: string) => void;
    setState: (state: string) => void;
}

export const AuthContext = createContext<AuthContextProviderState | undefined>(undefined)

import { useState } from 'react'
import { AuthContext, AuthContextProviderState } from './AuthContext'

/*
AuthContext wrapper component that will attach state to access and refresh token
This will allow children components to use the AuthContextProviderState interface.
The interface will expose the state and functions to change, thus changing context from child components.
*/
export const AuthContextProvider = ({ children }: React.PropsWithChildren) => {
    const [accessToken, setAccessToken] = useState<string>("");
    const [refreshToken, setRefreshToken] = useState<string>("");
    const [state, setState] = useState<string>("");

    const defaultAuthContextState : AuthContextProviderState = {
        accessToken: accessToken,
        refreshToken: refreshToken,
        state: state,
        setState: setState,
        setAccessToken: setAccessToken,
        setRefreshToken: setRefreshToken
    };

    return (
        <AuthContext.Provider value={defaultAuthContextState}>
            {children}
        </AuthContext.Provider>
    );
}
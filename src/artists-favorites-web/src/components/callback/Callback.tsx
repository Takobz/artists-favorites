import { useContext, useEffect, useState } from "react";
import { ArtistsFavoritesApiService } from "../../services/ArtistsFavoritesApiService";
import { AuthContext } from "../../contexts/AuthContext";

const Callback = () => {
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const authContext = useContext(AuthContext)
    if (!authContext) throw Error("Auth Context can't be null");

    useEffect(() => {
        const urlQueryParams = window.location.hash.substring(1);
        const urlParams = new URLSearchParams(urlQueryParams);
        const authorizationCode = urlParams.get("code") ?? '';
        const state = urlParams.get("state") ?? '';

        const getAccessToken = async () => {
            return await new ArtistsFavoritesApiService().getUserAccessToken(authorizationCode, state)
            .then(response => {
                if (response) {
                    setIsLoading(false);
                    authContext.setAccessToken(response.accessToken);
                    authContext.setRefreshToken(response.refreshToken);
                    return response;
                }
            });
        }

        getAccessToken();

        //empty clean up function
        return () => {}
    }, [authContext]);

    return (
        <>
        {
            isLoading ? <>Some Loading Modal</> 
                : <>Take user to playlists screen</>
        }
        </>
    );
}

export default Callback;
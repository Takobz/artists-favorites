import { useEffect, useState } from "react";
import { ArtistsFavoritesApiService } from "../../services/ArtistsFavoritesApiService";

const Callback = () => {
    const [isLoading, setIsLoading] = useState<boolean>(true);

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
                    //set context's access token values.
                    return response;
                }
            })
        }

        getAccessToken();

        //empty clean up function
        return () => {}
    }, []);

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
import { useContext, useEffect, useState } from "react";
import { ArtistsFavoritesApiService } from "../../services/ArtistsFavoritesApiService";
import { AuthContext } from "../../contexts/AuthContext";
import { useNavigate } from "react-router-dom";

const Callback = () => {
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const authContext = useContext(AuthContext);
    const navigate = useNavigate();
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
                    authContext.setState(state);
                    navigate("/playlist/create");
                }
            });
        }

        getAccessToken();

        //empty clean up function
        return () => {}
    });

    return (
        <>
        {
            isLoading ? <>Some Loading Modal</> 
                : <>Creating Playlist...</>
        }
        </>
    );
}

export default Callback;
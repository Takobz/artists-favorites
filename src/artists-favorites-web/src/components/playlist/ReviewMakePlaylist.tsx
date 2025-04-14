import { useContext } from "react";
import { AuthContext } from "../../contexts/AuthContext";

const MakePlaylist = () => {
    const authContext = useContext(AuthContext);

    return (
        <>
            This will preview songs to be added to playlist.
            Will use access token: {authContext?.accessToken}
        </>
    );
}

export default MakePlaylist;

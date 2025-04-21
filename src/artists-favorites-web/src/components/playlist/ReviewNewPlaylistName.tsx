//import { AuthContext } from "../../contexts/AuthContext";

export interface ReviewNewPlaylistNameProps {
    playlistName: string
}

const ReviewNewPlaylistName = (props: ReviewNewPlaylistNameProps) => {
    //const authContext = useContext(AuthContext);

    return (
        <>
            Playlist name will be: <b>{props.playlistName}</b>
        </>
    );
}

export default ReviewNewPlaylistName;
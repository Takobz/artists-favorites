import { useEffect, useState } from "react";
import { Box, Button, Stack } from "@mui/material";
import { LocalStorageService } from "../../services/LocalStorageService";
import { WEB_APP_CONSTANTS } from "../../constants/webappconstants";
import ReviewNewPlaylistName from "./ReviewNewPlaylistName";
import ReviewSongsInPlaylist from "./ReviewSongsInPlaylist";
import ReviewConfirmCreate from "./ReviewConfirmCreate";

const MakePlaylist = () => {    
    const [playlistName, setPlaylistName] = useState<string>("");
    const [viewReviewNewPlaylistNameComp, setViewReviewNewPlaylistNameComp] = useState<boolean>(true);
    const [viewReviewSongsInPlaylistComp, setviewReviewSongsInPlaylistComp] = useState<boolean>(false);

    const onPrevisouClick = () => {
        if (viewReviewSongsInPlaylistComp){
            setViewReviewNewPlaylistNameComp(true);
            setviewReviewSongsInPlaylistComp(false);
        }
        else if (!viewReviewNewPlaylistNameComp && !viewReviewSongsInPlaylistComp){
            setviewReviewSongsInPlaylistComp(true);
            setViewReviewNewPlaylistNameComp(false);
        }
    }

    const onNextOrCreateClick = () => {
        if (viewReviewNewPlaylistNameComp){
            setviewReviewSongsInPlaylistComp(true)
            setViewReviewNewPlaylistNameComp(false);
        }
        else if (viewReviewSongsInPlaylistComp){
            setviewReviewSongsInPlaylistComp(false)
            setViewReviewNewPlaylistNameComp(false);
        }
        else {
            //call the api ?
        }
    }

    useEffect(() => {
        const localStorageService = new LocalStorageService();
        const artistName = localStorageService.getItemFromLocalStorage(
            WEB_APP_CONSTANTS.ArtistNameLocalStorageKey
        );

        setPlaylistName(`My Fave ${artistName} Songs`);
    }, []);

    return (
        <Box sx={{ maxHeight: '300px' }}>
            <Stack direction="column" spacing={2}>
                {
                    viewReviewNewPlaylistNameComp ?  <ReviewNewPlaylistName playlistName={playlistName}/> 
                        : viewReviewSongsInPlaylistComp ? <ReviewSongsInPlaylist />
                            :  <ReviewConfirmCreate />
                }
            </Stack>
            <Stack direction="row">
                <Button 
                    disabled={viewReviewNewPlaylistNameComp}
                    onClick={onPrevisouClick}
                    >
                    Previous
                </Button>
                <Button
                    onClick={onNextOrCreateClick}
                    >
                    {
                        !viewReviewNewPlaylistNameComp && !viewReviewSongsInPlaylistComp ?
                            "Create" : "Next"
                    }
                </Button>
            </Stack>
            
        </Box>
    );
}

export default MakePlaylist;

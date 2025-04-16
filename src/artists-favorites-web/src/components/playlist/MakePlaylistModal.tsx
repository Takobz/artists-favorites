import { useState } from "react";
import { Modal, Box, Button, Typography } from '@mui/material'
import CircularProgress from '@mui/material/CircularProgress';
import { ArtistsFavoritesApiService } from "../../services/ArtistsFavoritesApiService";

const style = {
    position: 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    width: 400,
    bgcolor: '#2a2b2a',
    border: '2px solid #000',
    boxShadow: 24,
    pt: 2,
    px: 4,
    pb: 3,
};

interface MakePlaylistModalProps {
    artistName: string;
    isOpen: boolean;
    onModalClose: () => void;
}

const MakePlaylistModal = (props: MakePlaylistModalProps) => {
    const [isAuthPending, setIsAuthPending] = useState<boolean>(false);
    const [modalMessage, setModdalMessage] = useState<string>(`This will create a playlist of songs you liked from ${props.artistName}`);
    const [disableConfirmBtn, setDisableConfirmBtn] = useState<boolean>(false);

    const handleConfirm = () => {
        setIsAuthPending(true);
        setDisableConfirmBtn(true);

        //TODO: fix this, gets called twice for some reason.
        new ArtistsFavoritesApiService().initiateAuthorizeRequest()
            .then(response => {
                if (response && response.spotifyAuthorizeUrl) {
                    setIsAuthPending(false);
                    window.location.href = response.spotifyAuthorizeUrl;
                } else {
                    setModdalMessage("Failed to initiate authorize please try again")
                }
            }).catch(err => console.log(err));
    }
  
    return (
        <>
            <Modal
                open={props.isOpen}
                onClose={props.onModalClose}
                >
                <Box sx={{ ...style, width: 400 }}>
                    {isAuthPending ? <CircularProgress /> :
                        <Box>
                            <Typography
                                variant="h6"
                                component="h2">
                                Create A Fave {props.artistName} Playlist?
                            </Typography>
                            <Typography sx={{mt: 2}}>
                                {modalMessage}
                            </Typography>
                            <Button 
                                onClick={handleConfirm}
                                disabled={disableConfirmBtn}>
                                    Yes, Create Playlist
                            </Button>
                        </Box>
                    }
                </Box>
            </Modal>
        </>
    );
}

export default MakePlaylistModal;

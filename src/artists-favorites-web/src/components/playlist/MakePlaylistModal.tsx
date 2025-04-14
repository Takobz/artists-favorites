import { useState } from "react";
import { Modal, Box, Button } from '@mui/material'
import CircularProgress from '@mui/material/CircularProgress';

const style = {
    position: 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    width: 400,
    bgcolor: 'background.paper',
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

    const handleConfirm = () => {
        setIsAuthPending(true);

        //this will prompt OAuth2.0 for user to authorize the creation of consent.
        //TODO to call backend to initiate then we waiting...
    }
  
    return (
        <>
            <Modal
                open={props.isOpen}
                onClose={props.onModalClose}
                >
                <Box sx={{ ...style, width: 400 }}>
                    {isAuthPending ? <CircularProgress /> :
                        <>
                            <h2>Create A Fave {props.artistName} Playlist?</h2>
                            <p>
                                This will create a playlist of songs you liked from {props.artistName}
                            </p>
                            <Button onClick={handleConfirm}>Yes, Create Playlist</Button>
                        </>
                    }
                </Box>
            </Modal>
        </>
    );
}

export default MakePlaylistModal;

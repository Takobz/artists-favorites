import { useState } from 'react';
import { SearchArtistResponse } from '../../models/DTOs/ArtistsFavoritesApi/Responses/SearchArtistResponse'
import 
{ 
    Card,
    CardMedia,
    Typography,
    CardContent, 
    CardActions,
    Button
} from '@mui/material';
import MakePlaylistModal from '../playlist/MakePlaylistModal';

interface IArtistCardProps {
    artist: SearchArtistResponse
}

const ArtistCard = (props: IArtistCardProps) => {
    const [isConfirmModalOpen, setIsConfirmModalOpen] = useState<boolean>(false);

    const handleMakePlayListClick = () => {
        setIsConfirmModalOpen(true);
    }

    return (
        <>
            <Card sx={{ maxWidth: 345, background: '#4d545e' }}>
            <CardMedia 
                sx={{ height: 240 }}
                image={ props.artist.imageUrl === '' ?
                     '/music-note-icon.png' : props.artist.imageUrl }
                title={ props.artist.name }/>
            <CardContent>
                <Typography gutterBottom variant='h5' component="div">
                    Name: { props.artist.name }
                </Typography>
                <Typography variant='body2' sx={{ color: 'text.secondary' }}>
                    Popularity Score: <b>{props.artist.artistPopularity} out of 100</b>
                </Typography>
            </CardContent>
            <CardActions>
                {/*TODO Add nice icon to button ?*/}
                <Button size='small' href={ props.artist.spotifyUrl }>Artist Spotify Page</Button>
                <Button size='small' onClick={handleMakePlayListClick}>Make A Fave Playlist</Button>
            </CardActions>
            </Card>

            <MakePlaylistModal
                artistName={props.artist.name} 
                artistEntityId={props.artist.artistEntityId}
                isOpen={isConfirmModalOpen}
                onModalClose={() => setIsConfirmModalOpen(false)}
                />
        </>
    );
};

export default ArtistCard;
import { useState } from 'react';
import { Box, Stack } from '@mui/material'
import SearchArtist from '../search/SearchArtist';
import ArtistsList from '../artists/ArtistsList';
import { SearchArtistResponse } from '../../models/DTOs/ArtistsFavoritesApi/Responses/SearchArtistResponse';

const Home = () => {
    const [artistSearchResults, setArtistSearchResults] = useState<SearchArtistResponse[]>([]);

    const onSearchDataReturned = (searchResponse: SearchArtistResponse[]) => {
      setArtistSearchResults(searchResponse);
    }

    return (
        <>
        <Box>
            <Stack>
                <SearchArtist onSearchDataReturned={onSearchDataReturned} />
                <ArtistsList artists={artistSearchResults}/>
            </Stack>
        </Box>
        </>
    );
}

export default Home;
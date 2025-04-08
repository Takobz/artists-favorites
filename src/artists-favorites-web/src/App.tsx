import { useState } from 'react'
import { SearchArtistResponse } from './models/DTOs/ArtistsFavoritesApi/Responses/SearchArtistResponse'
import './App.css'
import ArtistsList from './components/artists/ArtistsList'
import SearchArtist from './components/search/SearchArtist'
import { Box, Stack } from '@mui/material'

function App() {
  const [artistSearchResults, setArtistSearchResults] = useState<SearchArtistResponse[]>([]);

  const onSearchDataReturned = (searchResponse: SearchArtistResponse[]) => {
    setArtistSearchResults(searchResponse);
  }

  return (
    <Box>
      <Stack>
        <SearchArtist onSearchDataReturned={onSearchDataReturned} />
        <ArtistsList artists={artistSearchResults}/>
      </Stack>
    </Box>
  );
}

export default App

import { useEffect, useState } from 'react'
import { ArtistsFavoritesApiService } from './services/ArtistsFavoritesApiService'
import { SearchArtistResponse } from './models/DTOs/ArtistsFavoritesApi/Responses/SearchArtistResponse'
import { ErrorModel } from './models/ScreenModels/ErrorModel'
import './App.css'
import ArtistsList from './components/artists/ArtistsList'
import SearchArtist from './components/search/SearchArtist'

function App() {
  const [artistSearchResults, setArtistSearchResults] = useState<SearchArtistResponse[]>([])
  const [artistName, setArtistName] = useState<string>('');

  const onSearchDataReturned = (searchResponse: SearchArtistResponse[]) => {
    setArtistSearchResults(searchResponse);
  }

  return (
    <>
      <SearchArtist onSearchDataReturned={onSearchDataReturned} />
      <ArtistsList artists={artistSearchResults}/>
    </>
  );
}

export default App

import { useEffect, useState } from 'react'
import { ArtistsFavoritesApiService } from './services/ArtistsFavoritesApiService'
import { SearchArtistResponse } from './models/DTOs/ArtistsFavoritesApi/Responses/SearchArtistResponse'
import { ErrorModel } from './models/ScreenModels/ErrorModel'
import './App.css'
import ArtistsList from './components/artists/ArtistsList'

function App() {
  const [artistSearchResults, setArtistSearchResults] = useState<SearchArtistResponse[]>([])

  useEffect(() => {
    const getArtist = async () => {
      var response = await new ArtistsFavoritesApiService().searchArtistByName('mf doom');
      if (!response) {
        //TODO: some generic error displaying stuff!!
        return;
      }

      if (response instanceof ErrorModel) {
        // TODO: show error that happened
      }
      else {
        setArtistSearchResults(response);
      }
    };

    getArtist();

  }, []);

  return (
    <>
      <ArtistsList artists={artistSearchResults}/>
    </>
  );
}

export default App

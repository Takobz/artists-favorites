import 
{
    Box,
    Button,
    Stack,
    TextField 
} from '@mui/material'
import { useState } from 'react';
import { SearchArtistResponse } from '../../models/DTOs/ArtistsFavoritesApi/Responses/SearchArtistResponse'
import { ArtistsFavoritesApiService } from '../../services/ArtistsFavoritesApiService';

//TODO: FIX ALL THIS MESS HERE!

interface SearchProps {
    onSearchDataReturned: (searchResponse: SearchArtistResponse[]) => void;
}

const SearchArtist = (props: SearchProps) => {
    const [insertedValue, setInsertedValue] = useState<string>('');

    const handleSearchInsert = (event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        event.preventDefault();
        setInsertedValue(event.target.value);
    }
    
    const onSearchButtonClick = (artistName: string) => {
    const getArtist = async () => {
        //Have Results Pattern response object.
        var response = await new ArtistsFavoritesApiService().searchArtistByName(artistName);
            if (!response) {
            //TODO: some generic error displaying stuff!!
            return;
        }

        props.onSearchDataReturned(response)

        // if (response instanceof ErrorModel) {
        //     // TODO: show error that happened
        // }
        // else {
        //     props.onSearchDataReturned(response)
        // }
        };
    
        getArtist();
    }

    return (
        <Box sx={{ height: '100vh', display: 'flex', justifyContent: 'center' }}>
            <Stack direction='row' spacing={2}>
                <TextField 
                    value={insertedValue}
                    onChange={handleSearchInsert}/>

                <Button onClick={() => onSearchButtonClick(insertedValue)}>Search</Button>
            </Stack>
        </Box>
    );
}

export default SearchArtist;
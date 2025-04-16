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

interface SearchProps {
    onSearchDataReturned: (searchResponse: SearchArtistResponse[]) => void;
}

const SearchArtist = (props: SearchProps) => {
    const [insertedValue, setInsertedValue] = useState<string>('');

    const handleSearchInsert = (event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        event.preventDefault();
        setInsertedValue(event.target.value);
    }
    
    const onSearchButtonClick = () => {
        //Have Results Pattern response object.
        new ArtistsFavoritesApiService().searchArtistByName(insertedValue)
            .then(response => {
                if (!response) {
                    //TODO: some generic error displaying stuff!!
                    console.log(response);
                }
                else {
                    props.onSearchDataReturned(response);
                }
             });
    }

    return (
        <Box sx={{ alignContent: 'center', mb: 2 }}>
            <Stack direction='row' spacing={2}>
                <TextField 
                    value={insertedValue}
                    onChange={handleSearchInsert}/>
                <Button onClick={onSearchButtonClick}>Search</Button>
            </Stack>
        </Box>
    );
}

export default SearchArtist;
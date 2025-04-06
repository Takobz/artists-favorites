import { Box, Stack } from "@mui/material";
import { SearchArtistResponse } from "../../models/DTOs/ArtistsFavoritesApi/Responses/SearchArtistResponse";
import ArtistCard from "./ArtistCard";

interface ArtistsListProps {
    artists: SearchArtistResponse []
}

const ArtistsList = (props: ArtistsListProps) => {
    return (
        <>
            <Box sx={{ width: 500, minWidth: { xs: '90%', sm: 500 } }}>
                <Stack spacing={1}>
                    {props.artists.length ? 
                        props.artists.map((artist, index) => 
                            (<ArtistCard key={index} artist={artist}/>)) 
                            : <>No Artists</>
                    }
                </Stack>
            </Box>
        </>
    );
}

export default ArtistsList;
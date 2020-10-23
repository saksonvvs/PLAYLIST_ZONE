using Playlist.Zone.Dto.Artist;
using Playlist.Zone.Dto.Artist.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Playlist.Zone.Services.Helpers.Artist
{
    public static class ArtistHelper
    {


        public static List<AbstractArtistDto> GetPopularArtists()
        {
            List<AbstractArtistDto> artists = new List<AbstractArtistDto>();

            artists.Add(Add("Moby", "moby.jpg"));

            artists.Add(Add("Calvin Harris", "calvin-harris.jpg"));

            artists.Add(Add("David Guetta", "david-guetta.jpg"));

            artists.Add(Add("Demi Lovato", "demi-lovato.jpg"));

            artists.Add(Add("Jay Z", "jay-z.jpg"));

            artists.Add(Add("Snoop Dog", "snoop-dogg.jpg"));

            artists.Add(Add("Taylor Swift", "taylor-swift.jpg"));

            artists.Add(Add("Ed-Sheerann", "Ed-Sheerann.jpg"));

            artists.Add(Add("Katy Perry", "katy-perry.jpg"));

            artists.Add(Add("Bruno Mars", "bruno-mars.jpg"));

            artists.Add(Add("The Weeknd", "the-weeknd.jpg"));

            artists.Add(Add("Linkin Park", "linkin-park.jpg"));

            artists.Add(Add("Adele", "adele.jpg"));

            artists.Add(Add("Lady Gaga", "lady-gaga.jpg"));

            artists.Add(Add("Justin Timberlake", "justin-timberlake.jpg"));



            return artists;
        }


        private static AbstractArtistDto Add(string pName, string pThumbnail)
        {
            AbstractArtistDto ar = new ArtistDto();
            ar.Name = pName;
            ar.Thumbnail = pThumbnail;
            return ar;
        }

        

    }
}

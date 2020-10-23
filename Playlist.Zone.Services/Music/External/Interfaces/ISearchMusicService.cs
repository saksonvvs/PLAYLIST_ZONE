using Playlist.Zone.Dto.Music.SearchResult;
using Playlist.Zone.Dto.Music.Song;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Playlist.Zone.Services.Music.External
{

    public interface ISearchMusicService
    {
        Task<string> SearchFirstByKeyword(string p_searchKeyword);

        //Task<AbstractSearchResultDto> SearchListByKeyword(string p_searchKeyword);
        
        Task<AbstractSearchResultDto> SearchListByKeyword( string p_searchKeyword, string p_pageToken = "");


        Task<AbstractSongDto> GetByYoutubeId(string p_youtubeId);

        /*
        Task<List<AbstractSongDto>> GetByYoutubeId(string p_youtubeId);

        Task<AbstractSongDto> SearchSong(string pSongId);
        */

    }


}

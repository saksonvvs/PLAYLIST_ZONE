using Compiler.Abstractions.Dto;
using Compiler.Abstractions.Dto.Common.Tag;
using Playlist.Zone.Dto.Music.Song;
using System;
using System.Collections.Generic;
using System.Text;

namespace Playlist.Zone.Dto.Music.SearchResult
{
    public abstract class AbstractSearchResultDto : AbstractParameterDto
    {
        public string SearchTerm { get; set; }
        public string NextPageToken { get; set; }
        public List<AbstractSongDto> SongsList { get; set; }
        public List<AbstractTagDto> TagsList { get; set;}


        public AbstractSearchResultDto()
        {
            SearchTerm = string.Empty;
            NextPageToken = string.Empty;
            SongsList = new List<AbstractSongDto>();
            TagsList = new List<AbstractTagDto>();
        }



    }
}

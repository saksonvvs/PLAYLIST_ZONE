using Compiler.Abstractions.Dto;
using Compiler.Common.Music;
using System;
using System.Collections.Generic;
using System.Text;

namespace Playlist.Zone.Dto.Music.SongSource
{
    public abstract class AbstractSongSourceDto : AbstractParameterDto
    {
        public SongTypes SongType { get; set; }
        public string SongGuid { get; set; }



        public AbstractSongSourceDto()
        {
            SongType = SongTypes.Unknown;
            SongGuid = String.Empty;
        }


    }
}

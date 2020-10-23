using Compiler.Abstractions.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Playlist.Zone.Dto.Artist.Abstract
{
    public abstract class AbstractArtistDto : AbstractParameterDto
    {
        public string Name { get; set; }
        public string Thumbnail { get; set; }




        public AbstractArtistDto()
        {
            Name = string.Empty;
            Thumbnail = string.Empty;
        }


    }
}

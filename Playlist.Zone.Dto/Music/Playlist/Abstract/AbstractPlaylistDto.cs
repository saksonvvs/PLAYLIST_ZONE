using Compiler.Abstractions.Dto;
using Compiler.Abstractions.Dto.Common.Tag;
using Dapper.Contrib.Extensions;
using Playlist.Zone.Dto.Music.Song;
using System;
using System.Collections.Generic;
using System.Text;

namespace Playlist.Zone.Dto.Music.Playlist
{
    [Table("Playlist")]
    public abstract class AbstractPlaylistDto : AbstractParameterDto
    {
        public virtual string Name { get; set; }

        [Write(false)]
        public virtual string Description { get; set; }

        public int UserId { get; set; }



        public int ViewNum { get; set; }
        public int LikeNum { get; set; }


        public bool IsDefault { get; set; }


        [Write(false)]
        public virtual List<AbstractSongDto> SongsList { get; set; }


        [Write(false)]
        public virtual IEnumerable<AbstractTagDto> TagsList { get; set; }
        
        [Write(false)]
        public virtual IEnumerable<string> Icons { get; set; }

        public AbstractPlaylistDto()
        {
            Name = string.Empty;
            Description = string.Empty;


            SongsList = new List<AbstractSongDto>();
            IsDefault = false;


            TagsList = new List<AbstractTagDto>();

            Icons = new List<string>();
        }
        

    }


     /*
        CREATE TABLE `playlist` (
          `Id` int(11) NOT NULL AUTO_INCREMENT,
          `UserId` int(11) NOT NULL DEFAULT '0',
          `Name` varchar(100) DEFAULT NULL,
          `ViewNum` int(11) DEFAULT '0',
          `LikeNum` int(11) DEFAULT '0',
          `DteCreated` datetime DEFAULT CURRENT_TIMESTAMP,
          `IsDefault` bit(1) DEFAULT b'0',
          `IsDeleted` bit(1) DEFAULT b'0',
          `Uid` varchar(100) DEFAULT NULL,
          `CreatedBy` int(11) DEFAULT '0',
          PRIMARY KEY (`Id`)
        ) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8;
    */
}

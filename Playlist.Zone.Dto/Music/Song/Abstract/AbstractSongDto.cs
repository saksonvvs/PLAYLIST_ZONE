using Compiler.Abstractions.Dto;
using Dapper.Contrib.Extensions;
using Newtonsoft.Json;
using Playlist.Zone.Dto.Music.SongSource;
using System;
using System.Collections.Generic;
using System.Text;

namespace Playlist.Zone.Dto.Music.Song
{
    [Table("Playlist_Song")]
    public abstract class AbstractSongDto : AbstractParameterDto
    {

        public virtual string Name { get; set; }
        
        public string Artist { get; set; }
        
        public int UserId { get; set; }
        
        public int PlaylistId { get; set; }
        
        public List<AbstractSongSourceDto> ExternalIds;
        
        public string YouTubeId { get; set; }

        [Write(false)]
        public string ViewsNum { get; set; }
        [Write(false)]
        public string LikesNum { get; set; }
        [Write(false)]
        public string Duration { get; set; }


        public AbstractSongDto()
        {
            Id = 0;
            Name = string.Empty;

            DteCreated = new DateTime(1900,1,1);
            CreatedBy = 0;

            ViewsNum = string.Empty;
            LikesNum = string.Empty;
            Duration = string.Empty;
        }



    }



    /*
     CREATE TABLE `playlist_song` (
      `Id` int(11) NOT NULL AUTO_INCREMENT,
      `PlaylistId` int(11) NOT NULL,
      `UserId` int(11) NOT NULL,
      `Artist` varchar(100) NOT NULL DEFAULT '',
      `Name` varchar(100) NOT NULL DEFAULT '',
      `YouTubeId` varchar(50) NOT NULL DEFAULT '',
      `IsDeleted` bit(1) NOT NULL DEFAULT b'0',
      `Uid` varchar(100) DEFAULT NULL,
      `DteCreated` datetime DEFAULT CURRENT_TIMESTAMP,
      `CreatedBy` int(11) DEFAULT '0',
      PRIMARY KEY (`Id`)
    ) ENGINE=InnoDB AUTO_INCREMENT=261 DEFAULT CHARSET=utf8;

     */

}

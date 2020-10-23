using Compiler.Abstractions.Dto;
using Dapper.Contrib.Extensions;
using Newtonsoft.Json;
using Playlist.Zone.Dto.Music.SongSource;
using System;
using System.Collections.Generic;
using System.Text;

namespace Playlist.Zone.Dto.Music.Song
{
    [Table("Music_Chart_Song")]
    public abstract class AbstractChartSongDto : AbstractParameterDto
    {
        public virtual string Name { get; set; }

        [Write(false)]
        public virtual string Description { get; set; }

        public string Artist { get; set; }

        

        public int UserId { get; set; }

        
        public int ChartId { get; set; }
        
        public int Position { get; set; }
        
        
        public List<AbstractSongSourceDto> ExternalIds;
        

        public string YouTubeId { get; set; }



        public AbstractChartSongDto()
        {
            Id = 0;
            Name = string.Empty;
            Description = string.Empty;


            DteCreated = new DateTime(1900,1,1);
            CreatedBy = 0;
        }



    }


    /*
     CREATE TABLE `music_chart_song` (
      `Id` int(11) NOT NULL AUTO_INCREMENT,
      `ChartId` int(11) NOT NULL DEFAULT '0',
      `Artist` varchar(100) DEFAULT '',
      `Name` varchar(100) DEFAULT '',
      `Position` int(11) DEFAULT '0',
      `YouTubeId` varchar(50) DEFAULT '',
      `Uid` varchar(100) DEFAULT NULL,
      `DteCreated` datetime DEFAULT CURRENT_TIMESTAMP,
      `CreatedBy` int(11) DEFAULT '0',
      `UserId` int(11) DEFAULT '0',
      PRIMARY KEY (`Id`)
    ) ENGINE=InnoDB AUTO_INCREMENT=1268 DEFAULT CHARSET=utf8;

     */
}

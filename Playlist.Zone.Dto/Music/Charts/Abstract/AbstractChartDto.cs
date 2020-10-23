using Compiler.Abstractions.Dto;
using Compiler.Common.Music;
using Dapper.Contrib.Extensions;
using Playlist.Zone.Dto.Music.Song;
using System;
using System.Collections.Generic;
using System.Text;

namespace Playlist.Zone.Dto.Music.Charts
{
    [Table("Music_Chart")]
    public abstract class AbstractChartDto : AbstractParameterDto
    {
        public virtual string Name { get; set; }
        
        public virtual int SongNum { get; set; }

        
        public virtual DateTime Date { get; set; }
        public ChartType Type { get; set; }

        public int ViewNum { get; set; }
        public int LikeNum { get; set; }


        [Write(false)]
        public virtual List<AbstractChartSongDto> SongsList { get; set; }



        public AbstractChartDto()
        {
            Name = string.Empty;

            SongsList = new List<AbstractChartSongDto>();

            ViewNum = 0;
            LikeNum = 0;
        }
    }



    /*
     CREATE TABLE `music_chart` (
      `Id` int(11) NOT NULL AUTO_INCREMENT,
      `Name` varchar(100) DEFAULT '',
      `Uid` varchar(100) DEFAULT '',
      `SongNum` int(11) DEFAULT '0',
      `Date` datetime DEFAULT NULL,
      `Type` int(11) DEFAULT '0',
      `DteCreated` datetime DEFAULT CURRENT_TIMESTAMP,
      `CreatedBy` int(11) DEFAULT '0',
      `ViewNum` int(11) DEFAULT '0',
      `LikeNum` int(11) DEFAULT '0',
      PRIMARY KEY (`Id`)
    ) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8;

     */
}

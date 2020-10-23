using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using Playlist.Zone.Dto.Music.Song;
using Compiler.Services.Logging;
using Compiler.Abstractions.Datatier.Base;
using Compiler.Utility.Settings;

namespace Playlist.Zone.Datatier.Music.Charts
{
    public class ChartSongEntity : BaseEntity<AbstractChartSongDto, ChartSongDto, UnknownChartSongDto>, IChartSongEntity
    {
        
        public ChartSongEntity(
            IBaseSettings pBaseSettings) : base(pBaseSettings, nameof(Music_Chart_Song))
        {
        }

        public readonly string Music_Chart_Song = string.Empty;


        //need to implement
        public Task<List<AbstractChartSongDto>> GetPopular()
        {
            throw new NotImplementedException();
        }


        //need to implement
        public Task<List<AbstractChartSongDto>> GetRecent()
        {
            throw new NotImplementedException();
        }


        //need to implement
        public async Task<List<AbstractChartSongDto>> GetPlaylistSongs(int chartId)
        {
            List<AbstractChartSongDto> retObj = new List<AbstractChartSongDto>();
            
            if (chartId <= 0)
            {
                //throw new IndexOutOfRangeException();
                return retObj;
            }

            using (System.Data.IDbConnection db = new MySqlConnection(_baseSettings.GetConnString()))
            {
                
                string chartSongsQry = @" SELECT * FROM music_chart_song WHERE ChartId = @p_Id ";

                var schemePolicySongs = await db.QueryAsync<ChartSongDto>(chartSongsQry, new
                {
                    @p_Id = chartId
                });

                retObj = schemePolicySongs.ToList<AbstractChartSongDto>();

                retObj = retObj == null ? new List<AbstractChartSongDto>() : retObj;
            }

            return retObj;
        }





    }
}

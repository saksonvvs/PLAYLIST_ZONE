using Playlist.Zone.Music.Charts;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using Playlist.Zone.Dto.Music.Charts;
using Playlist.Zone.Music.Charts.Dto;
using Compiler.Utility.Settings;
using Compiler.Abstractions.Datatier.Base;

namespace Playlist.Zone.Datatier.Music.Charts
{
    
    public class ChartEntity : BaseEntity<AbstractChartDto, ChartDto, UnknownChartDto>, IChartEntity
    {
        protected readonly IChartSongEntity _chartSongManager;


        /*public ChartEntity(
            IChartSongEntity pChartSongManager, 
            IBaseSettings pBaseSettings) : base(pBaseSettings, nameof(Music_Chart))*/
        public ChartEntity(
            IChartSongEntity pChartSongManager,
            IBaseSettings pBaseSettings) : base(pBaseSettings, nameof(Music_Chart))
        {
            _chartSongManager = pChartSongManager;
        }

        public readonly string Music_Chart = string.Empty;

        



        public override async Task<int> Add(AbstractChartDto p_item)
        {
            int result = 0;

            string con_str = _baseSettings.GetConnString();

            result = await base.Add(p_item);
            
            p_item.SongsList.ForEach(i => i.ChartId = p_item.Id);
            
            await _chartSongManager.Add(p_item.SongsList);
            
            
            return result;
        }



        
        public override async Task<bool> Add(IEnumerable<AbstractChartDto> p_itemsList)
        {
            string con_str = _baseSettings.GetConnString();
            


            await base.Add(p_itemsList);


            // add chart songs
            foreach (var chartItem in p_itemsList)
            {
                chartItem.SongsList.ForEach(i => i.ChartId = chartItem.Id);
                
                await _chartSongManager.Add(chartItem.SongsList);
            }



            return true;
        }
        

        //!!! need to complete
        public override async Task<bool> Update(AbstractChartDto p_item)
        {
            throw new NotImplementedException();
        }
        
        //!!! need to complete
        public override async Task<AbstractChartDto> GetById(int p_id)
        {
            AbstractChartDto retChart = new UnknownChartDto();

            retChart = await base.GetById(p_id);

            retChart.SongsList = await _chartSongManager.GetPlaylistSongs(retChart.Id);

            return retChart;
        }

        //!!! need to complete
        public override Task<IEnumerable<AbstractChartDto>> GetAll(int pPage = 1, int pSize = 20)
        {
            throw new NotImplementedException();
        }

        

        
        //!!! need to complete
        public override async Task<AbstractChartDto> GetByUid(string p_uid)
        {
            AbstractChartDto retChart = new UnknownChartDto();
            
            retChart = await base.GetByUid(p_uid);

            retChart.SongsList = await _chartSongManager.GetPlaylistSongs(retChart.Id);
            
            return retChart;
        }






        public async Task<List<AbstractChartDto>> GetPopular()
        {
            throw new NotImplementedException();
        }


        
        public async Task<List<AbstractChartDto>> GetRecent()
        {
            List<AbstractChartDto> retObj = new List<AbstractChartDto>();

            
            using (System.Data.IDbConnection db = new MySqlConnection(_baseSettings.GetConnString()))
            {   
                string chartQry = @" SELECT * FROM "+ nameof(Music_Chart) +" ORDER BY Id DESC LIMIT 10 ";
                var schemePolicy = await db.QueryAsync<ChartDto>(chartQry);

                retObj = schemePolicy.ToList<AbstractChartDto>();

                retObj = retObj == null ? new List<AbstractChartDto>() : retObj;
            }
            
            return retObj;
        }



        
        public async Task<List<AbstractChartDto>> GetByType(int chartType)
        {
            List<AbstractChartDto> retObj = new List<AbstractChartDto>();

            
            using (System.Data.IDbConnection db = new MySqlConnection(_baseSettings.GetConnString()))
            {
                
                string chartQry = @" SELECT * FROM " + nameof(Music_Chart) + " WHERE Type = @p_Type ORDER BY Id DESC LIMIT 10 ";
                var schemePolicy = await db.QueryAsync<ChartDto>(chartQry, 
                    new {
                        @p_Type = (int)chartType
                    });

                retObj = schemePolicy.ToList<AbstractChartDto>();

                retObj = retObj == null ? new List<AbstractChartDto>() : retObj;
            }

            return retObj;
        }



        public async Task<bool> AddView(int p_chart_id)
        {
            using (System.Data.IDbConnection db = new MySqlConnection(_baseSettings.GetConnString()))
            {

                string sql_qry = @" UPDATE " + nameof(Music_Chart) + @" AS PS
                                        SET ViewNum = ViewNum + 1
                                        WHERE PS.Id = @p_chartID ";
                await db.ExecuteAsync(sql_qry, param: new { p_chartID = p_chart_id });
            }

            return true;
        }


        public async Task<bool> AddLike(int p_chart_id)
        {
            using (System.Data.IDbConnection db = new MySqlConnection(_baseSettings.GetConnString()))
            {

                string sql_qry = @" UPDATE " + nameof(Music_Chart) + @" AS PS
                                        SET LikeNum = LikeNum + 1
                                        WHERE PS.Id = @p_chartID ";
                await db.ExecuteAsync(sql_qry, param: new { p_chartID = p_chart_id });
            }

            return true;
        }




    }
}

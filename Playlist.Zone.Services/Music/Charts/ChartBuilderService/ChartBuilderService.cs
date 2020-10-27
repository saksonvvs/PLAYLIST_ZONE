using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Playlist.Zone.Music.Charts.ChartBuilderService;
using Playlist.Zone.Music.Charts;
using Playlist.Zone.Services.Music.External;
using Playlist.Zone.Services.Music.Charts.ChartParser;
using Playlist.Zone.Dto.Music.Charts;
using Compiler.Interfaces.Common.Datatier;

namespace Playlist.Zone.Services.Music.Charts.ChartBuilderService
{
    public class ChartBuilderService : IChartBuilderService
    {
        private readonly IBaseEntity<AbstractChartDto> _dbChartManager;
        private readonly ISearchMusicService _youtubeSearch;

        public ChartBuilderService()
        {
        }

        public ChartBuilderService(IBaseEntity<AbstractChartDto> p_dbCharManager, ISearchMusicService p_youtubeSearch)
        {
            _dbChartManager = p_dbCharManager;
            _youtubeSearch = p_youtubeSearch;
        }



       


        public async System.Threading.Tasks.Task<AbstractChartDto> BuildChart(string p_chartUrl, List<KeyValuePair<string, string>> p_chartHeaders, IChartParser p_chartParser)
        {
            AbstractChartDto retChart = new ChartDto();


            if(p_chartUrl == null ||p_chartUrl == "")
            {
                throw (new ArgumentException("Missing chart url"));
            }

            if (p_chartHeaders == null)
            {
                throw (new ArgumentException("Missing chart request headers"));
            }

            if (p_chartParser == null)
            {
                throw (new ArgumentException("Missing chart parser"));
            }




            using (var client = new HttpClient())
            {
                try
                {
                    
                    foreach(var item in p_chartHeaders)
                    {
                        client.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }

                    
                    var response = await client.GetAsync(p_chartUrl);
                    response.EnsureSuccessStatusCode();


                    var stringResponse = await response.Content.ReadAsStringAsync();
                    

                    if (p_chartParser.AnalyzeRawChartData(stringResponse))
                    {
                        retChart = p_chartParser.ParseChart(stringResponse);
                    }



                    //link chart with youtube ids
                    foreach(var item in retChart.SongsList)
                    {
                        //loop and assign youtube ids
                        item.YouTubeId = await _youtubeSearch.SearchFirstByKeyword(item.Artist + "-" + item.Name);
                        Task.Delay(100);
                    }



                    if (retChart.SongNum > 0)
                    {
                        //save chart to data storage
                        _dbChartManager.Add(retChart);
                    }


                }
                catch (HttpRequestException ex)
                {
                    // record error to log
                }
            }

            return retChart;
        }


        public async System.Threading.Tasks.Task<AbstractChartDto> BuildChart(string p_chartUrl, string p_post_data, List<KeyValuePair<string, string>> p_chartHeaders, IChartParser p_chartParser)
        {
            AbstractChartDto retChart = new ChartDto();


            if (p_chartUrl == null || p_chartUrl == "")
            {
                throw (new ArgumentException("Missing chart url"));
            }

            if (p_chartHeaders == null)
            {
                throw (new ArgumentException("Missing chart request headers"));
            }

            if (p_chartParser == null)
            {
                throw (new ArgumentException("Missing chart parser"));
            }




            using (var client = new HttpClient())
            {
                try
                {

                    foreach (var item in p_chartHeaders)
                    {
                        client.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    

                    var stringContent = new StringContent(p_post_data);

                    var response = await client.PostAsync(p_chartUrl, stringContent);
                    response.EnsureSuccessStatusCode();


                    var stringResponse = await response.Content.ReadAsStringAsync();


                    if (p_chartParser.AnalyzeRawChartData(stringResponse))
                    {
                        retChart = p_chartParser.ParseChart(stringResponse);
                    }



                    if (retChart.SongNum > 0)
                    {
                        // save chart to data storage
                        _dbChartManager.Add(retChart);
                    }


                }
                catch (HttpRequestException ex)
                {
                    // record error to log
                }
            }

            return retChart;
        }

        
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
using Playlist.Zone.Dto.Music.Charts;
using Playlist.Zone.Dto.Music.Playlist;
using Playlist.Zone.Datatier.Music.Charts;
using Playlist.Zone.Datatier.Music.Playlists;
using Playlist.Zone.Services.Music.Charts.ChartParser;
using Playlist.Zone.Services.Music.Charts.ChartParser.YoutubeParser;
using Playlist.Zone.Services.Music.Charts.ChartParser.iTunesParser;
using Playlist.Zone.Services.Music.External;
using Playlist.Zone.Music.Charts.ChartBuilderService;
using Playlist.Zone.Services.Music.Charts.ChartBuilderService;
using Playlist.Zone.Administration.Code.Settings;
using Playlist.Zone.Services.Music.Charts.ChartParser.ShazamParser;
using Playlist.Zone.Music.Charts.ChartParser.BillboardParser;
using Compiler.Utility.Settings;

namespace Playlist.Zone.Administration.Controllers
{
    [Authorize(Roles="Admin")]
    public class MusicController : Controller
    {
        
        protected readonly IChartEntity _chartManager;
        protected readonly IPlaylistEntity _playlistManager;
        protected readonly IBaseSettings _currSettings;
        protected readonly ISearchMusicService _youtubeSearchService;
        //protected readonly ILogManagement _logManager;

        public MusicController(
            IChartEntity pChartManager,
            IBaseSettings pCurrSettings, 
            ISearchMusicService pYoutubeSearchService,
            IPlaylistEntity pPlaylistManager)
        {
            _chartManager = pChartManager;
            _currSettings = pCurrSettings;
            _youtubeSearchService = pYoutubeSearchService;
            _playlistManager = pPlaylistManager;
        }




        public IActionResult Index()
        {
            return View();
        }



        //[Route("/Music/Charts")]
        public async Task<ActionResult> Charts()
        {
            List<AbstractChartDto> chartList = new List<AbstractChartDto>();
            
            chartList = await _chartManager.GetRecent();
            
            return View(chartList);
        }



        //[Route("/Music/Playlists")]
        public async Task<ActionResult> Playlists()
        {
            IEnumerable<AbstractPlaylistDto> retObj;

            retObj = await _playlistManager.GetAll();

            return View(retObj);
        }





        //[Route("/Music/Generate/{pChartType}")]
        public async Task<ActionResult> Generate(int Id) //pChartType)
        {
            Compiler.Common.Music.ChartType ChartType = (Compiler.Common.Music.ChartType)Id;


            IChartSongEntity chartSongManager = new ChartSongEntity(_currSettings);
            IChartEntity dbChartManager = new ChartEntity(chartSongManager, _currSettings);
            IChartBuilderService chartBuilderService = new ChartBuilderService(dbChartManager, _youtubeSearchService);




            List<System.Collections.Generic.KeyValuePair<string, string>> headers_list = new List<KeyValuePair<string, string>>();
            headers_list.Add(new KeyValuePair<string, string>("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.186 Safari/537.36"));
            headers_list.Add(new KeyValuePair<string, string>("upgrade-insecure-requests", "1"));
            headers_list.Add(new KeyValuePair<string, string>("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,* / *;q=0.8"));
            //headers_list.Add(new KeyValuePair<string, string>("accept-encoding", "gzip, deflate, br"));
            headers_list.Add(new KeyValuePair<string, string>("accept-language", "en-US,en;q=0.9"));
            headers_list.Add(new KeyValuePair<string, string>("cache-control", "max-age=0"));
            headers_list.Add(new KeyValuePair<string, string>("Proxy-Connection", "keep-alive"));

            //headers_list.Add(new KeyValuePair<string, string>("Host", "www.apple.com"));



            AbstractChartDto currChart = new ChartDto();

            switch (ChartType)
            {
                case Compiler.Common.Music.ChartType.Shazam:
                    {
                        headers_list.Add(new KeyValuePair<string, string>("authority", "www.shazam.com"));
                        headers_list.Add(new KeyValuePair<string, string>("method", "GET"));
                        headers_list.Add(new KeyValuePair<string, string>("path", "/shazam/v2/en-US/US/web/-/tracks/web_chart_us"));
                        headers_list.Add(new KeyValuePair<string, string>("scheme", "https"));

                        //:authority:www.shazam.com
                        //:method:GET
                        //:path:/shazam/v2/en-US/US/web/-/tracks/web_chart_us
                        //:scheme:https
                        //accept:text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,* / *;q=0.8
                        //accept-encoding:gzip, deflate, br
                        //accept-language:en-US,en;q=0.9
                        //cache-control:max-age=0
                        //cookie:geoip_country=US; geoip_lat=40.857; geoip_long=-74.228
                        //if-modified-since:Tue, 06 Mar 2018 14:22:48 GMT
                        //upgrade-insecure-requests:1
                        //user-agent:Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.186 Safari/537.36

                        IChartParser shazamParser = new ShazamParser();

                        currChart = await chartBuilderService.BuildChart("https://www.shazam.com/shazam/v2/en-US/US/web/-/tracks/web_chart_us", headers_list, shazamParser);
                    }
                    break;
                case Compiler.Common.Music.ChartType.Billboard:
                    {
                        headers_list.Add(new KeyValuePair<string, string>("Referer", "http://www.billboard.com/charts"));

                        IChartParser billboardParser = new BillboardParser();

                        currChart = await chartBuilderService.BuildChart("http://www.billboard.com/charts/hot-100", headers_list, billboardParser);
                    }
                    break;
                case Compiler.Common.Music.ChartType.iTunes:
                    {

                        headers_list.Add(new KeyValuePair<string, string>("Referer", "https://www.apple.com/itunes/charts/"));


                        IChartParser ichartParser = new iTunesParser();


                        currChart = await chartBuilderService.BuildChart("https://www.apple.com/itunes/charts/songs/", headers_list, ichartParser);
                    }
                    break;
                case Compiler.Common.Music.ChartType.YouTube:
                    {
                        IChartParser youtubeParser = new YoutubeParser();

                        currChart = await chartBuilderService.BuildChart("https://charts.youtube.com/charts/TopSongs/us ", headers_list, youtubeParser);
                    }
                    break;
                default:
                    break;
            }




            return Redirect("/Music/Charts");
        }



        /*
        [Route("/Music/Charts/GenerateChart/{pChartType}")]
        public async Task<ActionResult> GenerateChart(int pChartType) //ChartType ChartType)
        {

            ChartType ChartType = (ChartType)pChartType;
            

            IChartSongEntity chartSongManager = new ChartSongEntity(_currSettings);
            IChartEntity dbChartManager = new ChartEntity(chartSongManager, _currSettings);
            IChartBuilderService chartBuilderService = new ChartBuilderService(dbChartManager, _youtubeSearchService);

            


            List<System.Collections.Generic.KeyValuePair<string, string>> headers_list = new List<KeyValuePair<string, string>>();
            headers_list.Add(new KeyValuePair<string, string>("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.133 Safari/537.36"));
            headers_list.Add(new KeyValuePair<string, string>("Upgrade-Insecure-Requests", "1"));
            headers_list.Add(new KeyValuePair<string, string>("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,;q=0.8"));
            headers_list.Add(new KeyValuePair<string, string>("Accept-Encoding", "gzip, deflate, sdch, br"));
            headers_list.Add(new KeyValuePair<string, string>("Accept-Language", "en-US,en;q=0.8,ru;q=0.6,cy;q=0.4,zh-CN;q=0.2,zh;q=0.2"));
            headers_list.Add(new KeyValuePair<string, string>("Cache-Control", "max-age=0"));
            headers_list.Add(new KeyValuePair<string, string>("Host", "www.apple.com"));
            headers_list.Add(new KeyValuePair<string, string>("Proxy-Connection", "keep-alive"));



            AbstractChartDto currChart = new AbstractChartDto();

            switch (ChartType)
            {
                case ChartType.Shazam:
                    {
                        IChartParser shazamParser = new ShazamParser();
                        
                        currChart = await chartBuilderService.BuildChart("https://www.shazam.com/shazam/v2/en-US/US/web/-/tracks/web_chart_us", headers_list, shazamParser);
                    }
                    break;
                case ChartType.Billboard:
                    {
                        headers_list.Add(new KeyValuePair<string, string>("Referer", "http://www.billboard.com/charts"));

                        IChartParser billboardParser = new BillboardParser();

                        currChart = await chartBuilderService.BuildChart("http://www.billboard.com/charts/hot-100", headers_list, billboardParser);
                    }
                    break;
                case ChartType.ITunes:
                    {
                        
                        headers_list.Add(new KeyValuePair<string, string>("Referer", "https://www.apple.com/itunes/charts/"));


                        IChartParser ichartParser = new iTunesParser();

                        
                        currChart = await chartBuilderService.BuildChart("https://www.apple.com/itunes/charts/songs/", headers_list, ichartParser);
                    }
                    break;

                default:
                    break;
            }







            return RedirectPermanent("/Music/Charts/View");
        }*/


        
    }
}

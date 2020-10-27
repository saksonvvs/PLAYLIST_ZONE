using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Compiler.Web.Playlist.Zone.Code;
using Playlist.Zone.Datatier.Music.Charts;
using Playlist.Zone.Datatier.Music.Playlists;
using Playlist.Zone.Music.Charts;
using Playlist.Zone.Dto.Music.Playlist;
using Playlist.Zone.Dto.Music.Charts;
using Playlist.Zone.Dto.Music.Song;
using Playlist.Zone.Music.Charts.Dto;
using Playlist.Zone.Services.Music.External;
using Compiler.Services.Logging;

namespace Compiler.Web.Playlist.Zone.Controllers.Controllers
{
    //
    // Allow anonymous access to player
    //
    [CustomExceptionFilter]
    public class PlayerController : BaseController
    {
        
        protected readonly IChartEntity _chartManager;
        protected readonly IChartSongEntity _chartSongManager;
        protected readonly IPlaylistEntity _playlistManager;
        private readonly ISearchMusicService _youtubeSearch;


        public PlayerController(
            IChartEntity pChartManager, 
            IChartSongEntity pChartSongManager, 
            IPlaylistEntity pPlaylistManager,
            ISearchMusicService pYoutubeSearch,
            ILogService pLogger) : base (pLogger)
        {
            _chartSongManager = pChartSongManager;
            _chartManager     = pChartManager;
            _playlistManager  = pPlaylistManager;
            _youtubeSearch    = pYoutubeSearch;
        }
        

        



        public IActionResult Index()
        {
            return View();
        }


        
        [Route("/player/playlist/{playlist_id}")]
        public async Task<IActionResult> Playlist(int playlist_id)
        {
            if (playlist_id <= 0)
                return View(new UnknownPlaylistDto());

            AbstractPlaylistDto currPlaylist = new PlaylistDto();
            currPlaylist = await _playlistManager.GetById(playlist_id);
            await _playlistManager.AddView(currPlaylist.UserId, playlist_id);

            return View(currPlaylist);
        }




        [Route("/player/chart/{chart_guid}")]
        public async Task<IActionResult> Chart(string chart_guid)
        {
            if (String.IsNullOrEmpty(chart_guid))
                return View(new UnknownChartDto());

            AbstractChartDto modelChart = new ChartDto();
            modelChart = await _chartManager.GetByUid(chart_guid);
            await _chartManager.AddView(modelChart.Id);

            return View(modelChart);
        }





        [Route("/player/song/{songId}")]
        public async Task<IActionResult> Song(string songId)
        {
            if (String.IsNullOrEmpty(songId))
                return View(new UnknownSongDto());

            AbstractSongDto song;
            song = await _youtubeSearch.GetByYoutubeId(songId);
            return View(song);
        }


    }
}
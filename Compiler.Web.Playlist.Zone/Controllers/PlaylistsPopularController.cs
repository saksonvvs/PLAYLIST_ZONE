using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Compiler.Web.Playlist.Zone.Code;
using Playlist.Zone.Datatier.Music.Playlists;
using Playlist.Zone.Datatier.Music.Songs;
using Playlist.Zone.Services.Music.External;
using Playlist.Zone.Dto.Music.Playlist;
using Playlist.Zone.Dto.Music.Song;
using Playlist.Zone.Dto.Music.SearchResult;
using Compiler.Interfaces.Common.Datatier.Tag;

namespace Compiler.Web.Playlist.Zone.Controllers.Controllers
{
    [Authorize(Roles = "User")]
    [NoCache]
    [CustomExceptionFilter]
    public class PlaylistsPopularController : BaseController
    {
        
        protected readonly IPlaylistEntity _playlistEntity;
        private readonly ISearchMusicService _youtubeSearch;
        private readonly ISongsEntity _songManager;
        private readonly ITagEntity _tagEntity;


        public PlaylistsPopularController(
            IPlaylistEntity pPlaylistEntity, 
            ISearchMusicService pYoutubeSearch,
            ISongsEntity pSongManager,
            ITagEntity pTagEntity
            )
        {
            _playlistEntity = pPlaylistEntity;
            _youtubeSearch = pYoutubeSearch;
            _songManager = pSongManager;
            _tagEntity = pTagEntity;
        }
        





        [AllowAnonymous]
        [Route("/Playlists/Popular")]
        public IActionResult Popular()
        {
            return View();
        }



        [AllowAnonymous]
        [HttpGet]
        [Route("/Playlists/PopularPlaylists")]
        public async Task<ActionResult> PopularPlaylists()
        {
            List<AbstractPlaylistDto> retObj = new List<AbstractPlaylistDto>();
            retObj = await _playlistEntity.GetPopular();
            return View(retObj);
        }



        [AllowAnonymous]
        [HttpGet]
        [Route("/Playlists/MostViewed")]
        public async Task<IActionResult> MostViewed()
        {
            List<AbstractPlaylistDto> retObj = new List<AbstractPlaylistDto>();
            retObj = await _playlistEntity.GetMostViewed();
            return View(nameof(PlaylistsPopularController.PopularPlaylists), retObj);
        }


    }
}
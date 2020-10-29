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
using Compiler.Services.Logging;

namespace Compiler.Web.Playlist.Zone.Controllers.Controllers
{
    [Authorize(Roles = "User")]
    [NoCache]
    [CustomExceptionFilter]
    public class PlaylistsController : BaseController
    {
        
        protected readonly IPlaylistEntity _playlistEntity;
        private readonly ISearchMusicService _youtubeSearch;
        private readonly ISongsEntity _playlistSongsEntity;
        private readonly ITagEntity _tagEntity;


        public PlaylistsController(
            IPlaylistEntity pPlaylistEntity, 
            ISearchMusicService pYoutubeSearch,
            ISongsEntity pPlaylistSongsEntity,
            ITagEntity pTagEntity,
            ILogService pLogger) : base(pLogger)
        {
            _playlistEntity = pPlaylistEntity;
            _youtubeSearch = pYoutubeSearch;
            _playlistSongsEntity = pPlaylistSongsEntity;
            _tagEntity = pTagEntity;
        }
        



        [AllowAnonymous]
        public IActionResult Index()
        {   
            return View();
        }


        
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        
        
        [HttpGet]
        public async Task<IActionResult> Rename(int id)
        {
            AbstractPlaylistDto playlist;
            playlist = await _playlistEntity.GetById(id);
            return View(playlist);
        }



        [HttpGet]
        [Route("/Playlists/AddSong/{playlist_id}")]
        public IActionResult AddSong(int playlist_id)
        {
            if (playlist_id <= 0) 
                return View(-1);

            return View(playlist_id);
        }




        [HttpGet]
        [Route("/Playlists/AddSongToPlaylist/{song_guid}")]
        public async Task<IActionResult> AddSongToPlaylist(string song_guid)
        {
            ViewBag.SongGuid = song_guid;

            List<AbstractPlaylistDto> userPlaylists;
            userPlaylists = await _playlistEntity.GetAllUserPlaylists(SessionState.GetCurrUserID(User));

            return View(userPlaylists);
        }



        [HttpGet]
        [Route("/Playlists/ManagePlaylists")]
        public async Task<ActionResult> ManagePlaylists()
        {
            List<AbstractPlaylistDto> retObj;
            retObj = await _playlistEntity.GetAllUserPlaylists(SessionState.GetCurrUserID(User));
            return View(retObj);
        }




        [HttpGet]
        [Route("/Playlists/ManagePlaylistSongs/{playlist_id}")]
        public async Task<ActionResult> ManagePlaylistSongs(int playlist_id)
        {
            AbstractPlaylistDto retObj = new PlaylistDto();
            ViewBag.PlaylistId = playlist_id;
            retObj = await _playlistEntity.GetById(playlist_id);
            return View(retObj);
        }



        [AllowAnonymous]
        [HttpGet]
        [Route("/Playlists/LoadPlaylistDetails/{pPlaylistId}/{pUserId}")]
        public async Task<IActionResult> LoadPlaylistDetails(int pPlaylistId, int pUserId)
        {
            if (pPlaylistId <= 0) 
                return ErrorProblem();

            if (pUserId <= 0) 
                return ErrorProblem();

            AbstractPlaylistDto playlist = new PlaylistDto();
            playlist = await _playlistEntity.GetById(pPlaylistId);

            return View(playlist);
        }



    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Compiler.Services.Logging;
using Compiler.Web.Playlist.Zone.Code;
using Compiler.Web.Playlist.Zone.Controllers.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Playlist.Zone.Datatier.Music.Playlists;
using Playlist.Zone.Datatier.Music.Songs;
using Playlist.Zone.Dto.Music.Playlist;
using Playlist.Zone.Dto.Music.SearchResult;
using Playlist.Zone.Dto.Music.Song;
using Playlist.Zone.Services.Music.External;

namespace Compiler.Web.Playlist.Zone.Areas.Data.Controllers
{
    [Area("Data")]
    [Authorize(Roles = "User")]
    [NoCache]
    [CustomExceptionFilter]
    public class PlaylistsController : BaseController
    {

        protected readonly IPlaylistEntity _playlistEntity;
        private readonly ISearchMusicService _youtubeSearch;
        private readonly ISongsEntity _playlistSongEntity;


        public PlaylistsController(
            IPlaylistEntity pPlaylistEntity,
            ISearchMusicService pYoutubeSearch,
            ISongsEntity pPlaylistSongEntity,
            ILogService pLogger):base(pLogger)
        {
            _playlistEntity = pPlaylistEntity;
            _youtubeSearch = pYoutubeSearch;
            _playlistSongEntity = pPlaylistSongEntity;
        }



        [Route("/Data/Playlists/Index")]
        public IActionResult Index()
        {
            return View();
        }



        [HttpGet]
        [Route("/Data/Playlists/MyPlaylists")]
        public async Task<JsonResult> MyPlaylists()
        {
            List<AbstractPlaylistDto> retObj = new List<AbstractPlaylistDto>();
            retObj = await _playlistEntity.GetAllUserPlaylists(SessionManagement.GetSessionUserID(User));
            return Json(retObj);
        }


        [HttpGet]
        [Route("/Data/Playlists/PlaylistDetails/{pPlaylistId}")]
        public async Task<IActionResult> PlaylistDetails(int pPlaylistId)
        {
            if (pPlaylistId <= 0) 
                return BadRequest();

            List<AbstractSongDto> retObj = new List<AbstractSongDto>();
            retObj = await _playlistSongEntity.GetAllPlaylistSongs(SessionManagement.GetSessionUserID(User), pPlaylistId);
            return Ok(retObj);
        }



        [HttpGet]
        [Route("/Data/Playlists/DeleteSong/{pPlaylistId}/{pSongId}")]
        public async Task<IActionResult> DeleteSong(int pPlaylistId, int pSongId)
        {
            if (pSongId <= 0) 
                return BadRequest("Parameter Id is out of range");

            AbstractSongDto deleteSong;
            deleteSong = await _playlistSongEntity.GetById(pSongId);

            if (deleteSong.UserId != SessionManagement.GetSessionUserID(User))
                return BadRequest();

            deleteSong.Id = pSongId;
            bool deleteResult = await _playlistSongEntity.Delete(deleteSong);

            return Ok();
        }




        [HttpGet]
        [Route("/Data/Playlists/AddLike/{pPlaylistId}")]
        public async Task<IActionResult> AddLike(int pPlaylistId)
        {
            if (pPlaylistId <= 0) 
                return BadRequest();

            AbstractPlaylistDto currPlaylist = await _playlistEntity.GetById(pPlaylistId);
            await _playlistEntity.AddLike(currPlaylist.UserId, currPlaylist.Id);

            return Ok(currPlaylist.LikeNum + 1);
        }




        [HttpPost]
        [Route("/Data/Playlists/Add/{pPlaylistName}")]
        public async Task<IActionResult> Add(string pPlaylistName)
        {
            if (string.IsNullOrEmpty(pPlaylistName))
                return BadRequest();

            AbstractPlaylistDto playlist = new PlaylistDto();
            playlist.Name = pPlaylistName;
            playlist.DteCreated = DateTime.Now;
            playlist.UserId = SessionManagement.GetSessionUserID(User);
            playlist.Id = await _playlistEntity.Add(playlist);

            return Ok(playlist);
        }




        [HttpPost]
        [Route("/Data/Playlists/Rename")]
        public async Task<IActionResult> Rename(int Id, string PlaylistName)
        {
            if (Id <= 0) 
                return BadRequest();

            if (string.IsNullOrEmpty(PlaylistName))
                return BadRequest();

            AbstractPlaylistDto playlist;
            playlist = await _playlistEntity.GetById(Id);


            if (playlist.UserId != SessionManagement.GetSessionUserID(User)) 
                return BadRequest();


            playlist.Name = PlaylistName;
            await _playlistEntity.Update(playlist);
            

            return Ok(playlist);
        }




        [HttpPost]
        [Route("/Data/Playlists/AddSong/{pPlaylistId}/{pSongGuid}")]
        public async Task<IActionResult> AddSong(int pPlaylistId, string pSongGuid)
        {
            if (pPlaylistId <= 0) 
                return BadRequest();

            if (String.IsNullOrEmpty(pSongGuid)) 
                return BadRequest();


            AbstractSongDto video;
            video = await _youtubeSearch.GetByYoutubeId(pSongGuid);


            AbstractSongDto song = new PlaylistSongDto();
            if (video != null)
            {
                song.YouTubeId = pSongGuid;
                song.PlaylistId = pPlaylistId;
                song.Artist = video.Artist;
                song.Name = video.Name;
                song.UserId = SessionManagement.GetSessionUserID(User);

                song.Id = await _playlistSongEntity.Add(song);
            }

            return Ok(song);
        }



        [HttpGet]
        [Route("/Data/Playlists/MakeDefault/{pId}")]
        public async Task<IActionResult> MakeDefault(int pId)
        {
            await _playlistEntity.SetDefaultPlaylist(SessionManagement.GetSessionUserID(User), pId);
            return Ok("");
        }


        
        [HttpPost]
        [Route("/Data/Playlists/SearchSong/{pSongName}")]
        public async Task<IActionResult> SearchSong(string pSongName)
        {
            if (String.IsNullOrEmpty(pSongName))
                return BadRequest();

            AbstractSearchResultDto musicSearchModel;
            musicSearchModel = await _youtubeSearch.SearchListByKeyword(pSongName);
            return Ok(musicSearchModel);
        }


    }
}
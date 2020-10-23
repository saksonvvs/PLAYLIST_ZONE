using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Playlist.Zone.Datatier.Music.Playlists;

namespace Compiler.Web.Playlist.Zone.Areas.Data.Controllers
{
    public class PlaylistDetailsController : Controller
    {

        protected readonly IPlaylistEntity _playlistEntity;


        public PlaylistDetailsController(IPlaylistEntity playlistManager)
        {
            _playlistEntity = playlistManager;
        }



        [HttpGet]
        [Route("/Data/Playlist/Details/Icons/{pPlaylistId}")]
        public async Task<IActionResult> GetPlaylistIcons(int pPlaylistId)
        {
            if (pPlaylistId <= 0)
                return Json("");

            IEnumerable<string> icons;
            icons = await _playlistEntity.GetPlaylistIcons(pPlaylistId);

            return Content(JsonConvert.SerializeObject(icons));
        }



    }
}

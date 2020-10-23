using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Compiler.Web.Admin.Areas.Music.Controllers
{
    [Area("Music")]
    [Authorize(Roles = "Admin")]
    [Route("/[area]/[controller]/[action]")]
    public class HomeController : Controller
    {

        public HomeController()
        {
        }


        public IActionResult Index()
        {
            /*
            Compiler.Dto.Music.Playlist.AbstractPlaylistDto model = new PlaylistDto();

            model.Name = "Test";

            return View(model);*/

            return View();
        }


    }
}
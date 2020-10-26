using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Compiler.Web.Playlist.Zone.Models;
using Compiler.Web.Playlist.Zone.Code;
using Microsoft.Extensions.Configuration;

namespace Compiler.Web.Playlist.Zone.Controllers
{
    public class HomeController : BaseController
    {
        public IConfiguration _config;

        public HomeController(IConfiguration pConfig)
        {
            _config = pConfig;
        }



        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Start()
        {
            return View();
        }

    }
}

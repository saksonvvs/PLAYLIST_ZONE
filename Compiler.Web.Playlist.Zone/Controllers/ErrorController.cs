using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Compiler.Web.Playlist.Zone.Controllers
{
    public class ErrorController : Controller
    {
        

        [Route("/Error/Problem")]
        public IActionResult Problem()
        {
            return View();
        }


    }
}

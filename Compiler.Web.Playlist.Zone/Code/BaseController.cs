using Compiler.Services.Logging;
using Compiler.Web.Playlist.Zone.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Compiler.Web.Playlist.Zone.Code
{
    public class BaseController : Controller
    {
        public readonly ILogService _logger;


        public BaseController(ILogService _pLogger)
        {
            _logger = _pLogger;
        }


        

        public IActionResult ErrorProblem()
        {
            // !!! need some logging here
            //

            return Content("Problem occured while handling your request. Try again.");
        }


        public IActionResult ErrorFatal()
        {
            // !!! need some logging here
            //

            return Content("Oops, we are working on it. Try again.");
        }


    }
}

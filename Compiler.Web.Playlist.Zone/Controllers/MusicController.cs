    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Compiler.Web.Playlist.Zone.Code;
using Compiler.Services.Logging;

namespace Compiler.Web.Playlist.Zone.Controllers.Controllers
{
    [CustomExceptionFilter]
    public class MusicController : BaseController
    {
        public MusicController(ILogService pLogger) :base(pLogger)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
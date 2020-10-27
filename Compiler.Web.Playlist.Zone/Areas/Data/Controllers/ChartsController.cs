using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Compiler.Services.Logging;
using Compiler.Web.Playlist.Zone.Code;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Playlist.Zone.Datatier.Music.Charts;
using Playlist.Zone.Dto.Music.Charts;
using Playlist.Zone.Music.Charts;

namespace Compiler.Web.Playlist.Zone.Areas.Data.Controllers
{

    [Area("Data")]
    [Authorize(Roles = "User")]
    [NoCache]
    [CustomExceptionFilter]
    public class ChartsController : BaseController
    {

        protected readonly IChartEntity _chartManager;
        private readonly IChartSongEntity _songManager;

        public ChartsController(
            IChartEntity pChartManager,
            IChartSongEntity pSongManager,
            ILogService pLogger
            ):base(pLogger)
        {
            _chartManager = pChartManager;
            _songManager = pSongManager;
        }

        

        [HttpGet]
        [Route("/Data/Charts/AddLike/{pPlaylistId}")]
        public async Task<IActionResult> AddLike(int pPlaylistId)
        {
            if (pPlaylistId <= 0)
                return BadRequest();

            AbstractChartDto currChart = await _chartManager.GetById(pPlaylistId);

            if (currChart.Id <= 0)
                return BadRequest();

            await _chartManager.AddLike(currChart.Id);
            
            return Ok("");
        }


    }
}
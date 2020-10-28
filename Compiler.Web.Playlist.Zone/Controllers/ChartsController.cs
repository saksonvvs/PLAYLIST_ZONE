using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Compiler.Web.Playlist.Zone.Code;
using Microsoft.AspNetCore.Authorization;
using Playlist.Zone.Datatier.Music.Charts;
using Playlist.Zone.Music.Charts;
using Playlist.Zone.Dto.Music.Charts;
using Playlist.Zone.Music.Charts.Dto;
using Compiler.Services.Logging;

namespace Compiler.Web.Playlist.Zone.Controllers.Controllers
{
    [CustomExceptionFilter]
    public class ChartsController : BaseController
    {
        
        protected readonly IChartEntity _chartManager;

        public ChartsController(
            IChartEntity pChartManager,
            ILogService pLogger) : base(pLogger)
        {
            _chartManager = pChartManager;
        }
        


        public async Task<IActionResult> Index()
        {
            List<AbstractChartDto> chartList;
            chartList = await _chartManager.GetRecent();
            return View(chartList);
        }



        [Route("/Charts/Details/{chart_guid}")]
        public async Task<IActionResult> Details(string chart_guid)
        {
            if (String.IsNullOrEmpty(chart_guid))
                return View(new UnknownChartDto());

            AbstractChartDto chart;
            chart = await _chartManager.GetByUid(chart_guid);
            return View(chart);
        }



        [Route("/Charts/List/Type/{chartType}")]
        public async Task<IActionResult> GetChartListByType(int chartType)
        {
            if (chartType <= 0)
                return View(new List<ChartDto>());

            IList<AbstractChartDto> chart;
            chart = await _chartManager.GetByType(chartType);
            return View(chart);
        }


    }
}
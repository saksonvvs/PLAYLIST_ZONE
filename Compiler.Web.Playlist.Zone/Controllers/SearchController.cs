using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Playlist.Zone.Services.Music.External;
using Playlist.Zone.Dto.Music.SearchResult;
using Compiler.Web.Playlist.Zone.Code;
using Compiler.Interfaces.Common.Datatier.Tag;
using Compiler.Services.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Compiler.Web.Playlist.Zone.Controllers.Controllers
{
    public class SearchController : BaseController
    {

        public readonly ITagEntity _tagEntity;
        private readonly ISearchMusicService _youtubeSearch;


        public SearchController(
            ITagEntity pTagEntity,
            ISearchMusicService pYoutubeSearch,
            ILogService pLogger):base(pLogger)
        {
            _tagEntity = pTagEntity;
            _youtubeSearch = pYoutubeSearch;
        }





        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }




        [Route("/Search/Keyword/{pSearchKeyword}")]
        public async Task<IActionResult> Keyword(string pSearchKeyword)
        {
            if (string.IsNullOrEmpty(pSearchKeyword))
                return View(new SearchResultDto());

            if (pSearchKeyword.Length < 2) 
                return View(new SearchResultDto());


            AbstractSearchResultDto songsList = new SearchResultDto();
            songsList = await _youtubeSearch.SearchListByKeyword(pSearchKeyword);
            songsList.TagsList = await _tagEntity.GetRandomTags();

            return View(songsList);
        }


        

        [Route("/Search/Keyword/{pPageToken}/{pSearchKeyword}")]
        public async Task<IActionResult> Keyword(string pPageToken, string pSearchKeyword)
        {
            if (string.IsNullOrEmpty(pPageToken))
                return ErrorProblem();

            if (string.IsNullOrEmpty(pSearchKeyword))
                return ErrorProblem();

            if (pSearchKeyword.Length < 2)
                return View(new SearchResultDto());


            AbstractSearchResultDto songsList = new SearchResultDto();
            songsList = await _youtubeSearch.SearchListByKeyword(pSearchKeyword, pPageToken);
            songsList.TagsList = await _tagEntity.GetRandomTags();

            return View(songsList);
        }




    }
}

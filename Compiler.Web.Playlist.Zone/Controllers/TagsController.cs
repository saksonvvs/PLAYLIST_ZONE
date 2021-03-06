﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Compiler.Abstractions.Dto.Common.Tag;
using Compiler.Interfaces.Common.Datatier.Tag;
using Compiler.Services.Logging;
using Compiler.Web.Playlist.Zone.Code;
using Microsoft.AspNetCore.Mvc;
using Playlist.Zone.Datatier.Music.Playlists;
using Playlist.Zone.Dto.Common.Tag;
using Playlist.Zone.Dto.Music.Playlist;

namespace Compiler.Web.Playlist.Zone.Controllers
{
    [CustomExceptionFilter]
    public class TagsController : BaseController
    {
        public readonly ITagEntity _tagEntity;
        public readonly IPlaylistEntity _playlistEntity;


        public TagsController(
            ITagEntity pTagEntity,
            IPlaylistEntity pPlaylistEntity,
            ILogService pLogger) : base(pLogger)
        {
            _tagEntity = pTagEntity;
            _playlistEntity = pPlaylistEntity;
        }



        public async Task<IActionResult> Index()
        {
            List<AbstractTagDto> tags_list;
            tags_list = await _tagEntity.GetPopularTags();
            return View(tags_list);
        }



        [Route("/Tags/Details/{tag_id}")]
        public async Task<IActionResult> Details(int tag_id)
        {
            List<AbstractPlaylistDto> playlist_list;
            playlist_list = await _playlistEntity.GetByTagId(tag_id);
            return View(playlist_list);
        }



        [Route("/Tags/Details/Label/{tag_name}")]
        public async Task<IActionResult> LabelDetails(string tag_name)
        {
            List<AbstractTagDto> tags_list = new List<AbstractTagDto>();
            tags_list.Add(await _tagEntity.GetByName(tag_name));
            return View("Index", tags_list);
        }



        [HttpGet]
        [Route("/Tags/Add/{playlist_id}")]
        public async Task<IActionResult> Add(int playlist_id, string tag_name)
        {
            if (playlist_id <= 0) 
                return ErrorProblem();

            if (String.IsNullOrEmpty(tag_name)) 
                return ErrorProblem();

            AbstractTagDto checkTag = await _tagEntity.GetByName(tag_name);

            AbstractTagDto tagAdd = new TagDto();
            tagAdd.Name = tag_name;
            tagAdd.OwnerId = playlist_id;


            if (checkTag.Id > 0)
            {
                tagAdd = checkTag;
                tagAdd.OwnerId = playlist_id;
            }
            else
                tagAdd.Id = await _tagEntity.Add(tagAdd);

            await _playlistEntity.AddTag(tagAdd);

            return Content("");
        }



    }
}
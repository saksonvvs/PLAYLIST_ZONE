﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Compiler.Web.Playlist.Zone.Models;

namespace Compiler.Web.Playlist.Zone.Controllers
{
    public class HomeController : Controller
    {

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
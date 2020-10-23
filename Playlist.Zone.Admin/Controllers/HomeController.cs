using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Compiler.Web.Admin.Models;
using Microsoft.AspNetCore.Authorization;

namespace Compiler.Web.Admin.Controllers
{
    [Authorize(Roles="Admin")]
    public class HomeController : BaseController
    {


        public IActionResult Index()
        {
            return View();
        }


        
    }
}

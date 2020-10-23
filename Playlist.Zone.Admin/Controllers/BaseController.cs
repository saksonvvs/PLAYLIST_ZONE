using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Compiler.Web.Admin.Models;
using Compiler.Web.Admin.Code;
using Newtonsoft.Json;

namespace Compiler.Web.Admin.Controllers
{
    //[NoCacheAttribute]
    [Authorize(Roles="Admin")]
    public class BaseController : Controller
    {

        

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        





        protected void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }



        protected IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }




        protected string JsonResultEx(object pObject)
        {
            Response.ContentType = "application/json";
            return JsonConvert.SerializeObject(pObject);
        }




    }
}

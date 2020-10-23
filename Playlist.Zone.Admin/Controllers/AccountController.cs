using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Compiler.Web.Admin.Models;
using Compiler.Web.Admin.Models.AccountViewModels;
using Compiler.Web.Admin.Services;
using Compiler.Datatier.Users;
using Compiler.Utility.Config;
using Compiler.Interfaces.Common.Datatier.User;

namespace Compiler.Web.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]/[action]")]
    public class AccountController : BaseController
    {
        private readonly ILogger _logger;

        public readonly IUserEntity _userManager;
        


        public AccountController(
           ILogger<AccountController> logger)
        {
            _logger = logger;
        }






        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {   
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            
            
            //------------------------------------------------------------------------------
            if ( model.Email == ConfigManagement.GetConfig()["AdminEmail"] &&
                 model.Password == ConfigManagement.GetConfig()["AdminPassword"] )
            {
                var userIdentity = new ClaimsIdentity("Identity");
                userIdentity.Label = "Identity";
                userIdentity.AddClaim(new Claim(ClaimTypes.Name, "Admin"));
                userIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "1"));
                userIdentity.AddClaim(new Claim(ClaimTypes.Country, ""));
                userIdentity.AddClaim(new Claim(ClaimTypes.Email, ConfigManagement.GetConfig()["AdminEmail"]));
                userIdentity.AddClaim(new Claim(ClaimTypes.Spn, ""));
                userIdentity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                
                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);


                
                await HttpContext.SignInAsync(principal);
            }
            else
            {
                //ModelState.IsValid = false;
                ModelState.AddModelError("Unable to login", "Wrong username/password. Please try again.");

                return View();
            }
            //------------------------------------------------------------------------------


            

            return RedirectPermanent("/");
        }



        
        

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Signout()
        {
            await HttpContext.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }



        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }


        [TempData]
        public string ErrorMessage { get; set; }




    }
}

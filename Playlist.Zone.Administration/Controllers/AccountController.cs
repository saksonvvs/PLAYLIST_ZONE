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
using Compiler.Datatier.Users;
using Compiler.Utility.Config;
using Compiler.Interfaces.Common.Datatier.User;
using Playlist.Zone.Administration.Models.AccountViewModels;
using Playlist.Zone.Administration.Controllers;
using Microsoft.Extensions.Configuration;

namespace Compiler.Web.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]/[action]")]
    public class AccountController : Playlist.Zone.Administration.Controllers.BaseController
    {
        private readonly ILogger _logger;
        public readonly IUserEntity _userManager;
        private readonly IConfiguration _config;

        public AccountController(
           ILogger<AccountController> pLogger,
           IConfiguration pConfig)
        {
            _logger = pLogger;
            _config = pConfig;
        }


        public IActionResult Index()
        {
            return View();
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

            if ( model.Email == _config.GetSection("Admin").GetSection("Username").Value &&
                 model.Password == _config.GetSection("Admin").GetSection("Password").Value)
            {
                var userIdentity = new ClaimsIdentity("Identity");
                userIdentity.Label = "Identity";
                userIdentity.AddClaim(new Claim(ClaimTypes.Name, "Admin"));
                userIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "1"));
                userIdentity.AddClaim(new Claim(ClaimTypes.Country, ""));
                userIdentity.AddClaim(new Claim(ClaimTypes.Email, model.Email));
                userIdentity.AddClaim(new Claim(ClaimTypes.Spn, ""));
                userIdentity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                
                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

                await HttpContext.SignInAsync(principal);
            }
            else
            {
                ModelState.AddModelError("Unable to login", "Wrong username/password. Please try again.");
                return View();
            }

            return RedirectPermanent("/");
        }


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

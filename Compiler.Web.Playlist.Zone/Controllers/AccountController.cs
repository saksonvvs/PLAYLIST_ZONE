using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Security.Claims;
using Compiler.Web.Playlist.Zone.Code;
using Compiler.Web.Playlist.Zone.Code.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Compiler.Interfaces.Common.Datatier.User;
using Compiler.Utility.Settings;
using Compiler.Abstractions.Dto.System.User;
using Compiler.Abstractions.Dto.Person.User;
using Playlist.Zone.Datatier.Music.Playlists;
using Compiler.Web.Playlist.Zone.Models.Account;
using Playlist.Zone.Dto.Music.Playlist;
using Compiler.Abstractions.Dto.Common.Location;

namespace Compiler.Web.Playlist.Zone.Controllers.Controllers
{

    [Authorize(Roles = "User")]
    [NoCache]
    public class AccountController : Controller
    {
        protected readonly IBaseSettings _baseSettings;
        protected readonly IUserEntity _userEntity;
        protected readonly IPlaylistEntity _playlistEntity;

        
        public AccountController(
            IBaseSettings pBaseSettings,
            IUserEntity pUserEntity,
            IPlaylistEntity pPlaylistEntity)
        {
            _userEntity  = pUserEntity;
            _baseSettings = pBaseSettings;
            _playlistEntity = pPlaylistEntity;
        }
        




        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            AbstractUserDto currUserInfo = new UserDto();


            //admin check
            //-----------------------------------------------------------------
            bool isAdmin = User.HasClaim(ClaimTypes.Role, "Admin");
            bool IsAuthenticated = User.Identity.IsAuthenticated;
            string user_Name = User.Identity.Name;
            //-----------------------------------------------------------------


            //-----------------------------------------------------------------
            if (isAdmin) ViewData["isAdmin"] = "1";
            //-----------------------------------------------------------------


            //-----------------------------------------------------------------
            currUserInfo = await _userEntity.GetByUsername(user_Name);
            //-----------------------------------------------------------------


            // need to complete
            int sessionUserID = SessionManagement.GetSessionUserID(User);
            ViewData["sessionUserID"] = sessionUserID.ToString();


            return View(nameof(Info), currUserInfo);
        }



        public async Task<IActionResult> Info()
        {   
            AbstractUserDto currUserInfo = new UserDto();


            //admin check
            //-----------------------------------------------------------------
            bool isAdmin         = User.HasClaim(ClaimTypes.Role, "Admin");
            bool IsAuthenticated = User.Identity.IsAuthenticated;
            string user_Name     = User.Identity.Name;
            //-----------------------------------------------------------------


            if (isAdmin) ViewData["isAdmin"] = "1";
            
            currUserInfo = await _userEntity.GetByUsername(user_Name);
            
            return View(currUserInfo);
        }

        [Route("/Account/User/Stats")]
        public async Task<IActionResult> Stats()
        {
            StatsViewModel model = new StatsViewModel();

            IEnumerable<AbstractPlaylistDto> playlistResult = await _playlistEntity.GetAllUserPlaylists(SessionManagement.GetSessionUserID(User));

            model.playlists = playlistResult.ToList();

            return View(model);
        }








        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectPermanent("/Account/Login");
        }





        
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        
        
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                ViewData["LoginResult"] = "Please enter username.";
                return View();
            }

            if(string.IsNullOrEmpty(password))
            {
                ViewData["LoginResult"] = "Please enter password.";
                return View();
            }


            
            
            AbstractUserDto checkUser = new UserDto();
            checkUser = await _userEntity.SignInUser(username, password);

            if (checkUser == null || checkUser.Id <= 0)
            {
                ViewData["LoginResult"] = "Wrong username/password. Try again.";
                return View();
            }
            




            
            if (checkUser.Username == username && checkUser.Password == password && checkUser.IsActive == 1)
            {
                var userIdentity = new ClaimsIdentity("Identity");
                userIdentity.Label = "Identity";
                userIdentity.AddClaim(new Claim(ClaimTypes.Name, checkUser.Username));
                userIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, checkUser.Id.ToString()));
                userIdentity.AddClaim(new Claim(ClaimTypes.Country, ""));
                userIdentity.AddClaim(new Claim(ClaimTypes.Email, checkUser.Email));
                userIdentity.AddClaim(new Claim(ClaimTypes.Spn, checkUser.Image));


                userIdentity.AddClaim(new Claim(ClaimTypes.Role, "User"));
                if (checkUser.IsAdmin == 1)
                    userIdentity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));


                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);


                await HttpContext.SignInAsync(principal);
            }
            else
            {
                ViewData["LoginResult"] = "We are unable to log you in. Please contact administrators.";
                return View();
            }
            




            //------------------------------------------------------------------------------
            //_logger.LogInformation(10001, "User Login: User Details: " + username);
            //_signInManager.SignInAsync(appUs, isPersistent: false);   
            //_logger.LogInformation(3, "User logged in.");
            //_authz.AuthorizeAsync(User, "Admin");
            //------------------------------------------------------------------------------
            
            

            return RedirectPermanent("/");
        }
        








        
        
        [AllowAnonymous]
        public IActionResult Register()
        {
            RegisterViewModel model = new RegisterViewModel();
           
            return View(model);
        }
        

        
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            int new_user_id = 0;

            
            if (ModelState.IsValid)
            {
                //need to add password hasher
                // need to make all validations
                // if pass is the same 
                // user not registered yet
                // .....

                AbstractUserDto checkUser;
                checkUser = await _userEntity.GetByUsername(model.User.Username);
                if(checkUser != null && checkUser.Id > 0)
                {
                    ModelState.AddModelError("error", "Looks like user already exist. Please choose different username.");
                    return View(model);
                }

                checkUser = await _userEntity.GetByEmail(model.User.Email);
                if (checkUser != null && checkUser.Id > 0)
                {
                    ModelState.AddModelError("error", "Looks like user already exist. Please choose different email.");
                    return View(model);
                }

                
                model.User.IsActive = 1;
                model.User.IsConfirmed = 0;
                model.User.IsAdmin = 0;
                model.User.MembershipLevel = 1;
                model.User.RegistrationDate = DateTime.Now;
                model.User.Image = "noimage.png";
                model.User.LastLoginLocation = "";
                model.User.Address = new UnknownAddressDto();

                new_user_id = await _userEntity.Add(model.User);


                
                //need to send confirmation email
                // EmailService



                if (new_user_id > 0)
                {
                    return View("RegisterThanks");
                }
                else
                {
                    string user_object_to_log = JsonConvert.SerializeObject(model);
                    //_logger.LogInformation(10001, "Error during registration: User Details: " + user_object_to_log);
                    return View("RegisterProblem");
                }


            }
            

            return View(model);
        }




        [AllowAnonymous]
        public IActionResult RegisterThanks()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult RegisterProblem()
        {
            return View();
        }
        
        [AllowAnonymous]
        public IActionResult NotAuthorized()
        {
            return RedirectPermanent("/");
        }

    }
}
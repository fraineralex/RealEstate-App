using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.DTOs.Account;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Core.Application.Helpers;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Users;
using RealEstateApp.Presentation.WebApp.Middlewares;

namespace RealEstateApp.Presentation.WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult Index()
        {
            
            return View(new LoginViewModel());
        }


        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            AuthenticationResponse userVm = await _userService.LoginAsync(vm);
            if (userVm != null && userVm.HasError != true)
            {
                HttpContext.Session.Set<AuthenticationResponse>("user", userVm);


                if(userVm.Roles.FirstOrDefault() == Roles.Client.ToString())
                {
                    return RedirectToRoute(new { controller = "Client", action = "Index" });
                }

                else if (userVm.Roles.FirstOrDefault() == Roles.Agent.ToString())
                {
                    return RedirectToRoute(new { controller = "Agent", action = "Index" });
                }

                else if (userVm.Roles.FirstOrDefault() == Roles.Developer.ToString())
                {
                    // HAY QUE DECIRLE QUE NO PUEDE ENTRAR A LA APP
                    return RedirectToRoute(new { controller = "User", action = "AccessDenied" });
                }

                else
                {
                    return RedirectToRoute(new { controller = "Admin", action = "Index" });
                }

            }

            else
            {
                vm.HasError = userVm.HasError;
                vm.Error = userVm.Error;
                return View(vm);
            }

        }

        public async Task<IActionResult> Logout()
        {
            await _userService.SignOutAsync();
            HttpContext.Session.Remove("user");
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult Register()
        {
            List<string> userTypes = new List<string>();
            userTypes.Add(Roles.Client.ToString());
            userTypes.Add(Roles.Agent.ToString());
            ViewBag.UserTypes = userTypes;
            return View(new SaveUserViewModel());
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        public async Task<IActionResult> Register(SaveUserViewModel vm, string role)
        {
            if (!ModelState.IsValid)
            {
                List<string> userTypes = new List<string>();
                userTypes.Add(Roles.Client.ToString());
                userTypes.Add(Roles.Agent.ToString());
                ViewBag.UserTypes = userTypes;
                return View(vm);
            }

            vm.ImagePath = UploadImagesHelper.UploadUserImage(vm.File, vm.UserName);

            var origin = Request.Headers["origin"];
            RegisterResponse response = new RegisterResponse();


            if (role == Roles.Client.ToString())
            {
                response = await _userService.RegisterAsync(vm, origin, Roles.Client);
            }
            else if (role == Roles.Agent.ToString())
            {
                response = await _userService.RegisterAsync(vm, origin, Roles.Agent);
            }


            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;

                List<string> userTypes = new List<string>();
                userTypes.Add(Roles.Client.ToString());
                userTypes.Add(Roles.Agent.ToString());
                ViewBag.UserTypes = userTypes;

                return View(vm);
            }


            return RedirectToRoute(new { controller = "User", action = "Index" });
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            string response = await _userService.ConfirmEmailAsync(userId, token);
            return View("ConfirmEmail", response);
        }


        [ServiceFilter(typeof(LoginAuthorize))]
        public async Task<IActionResult> ForgotPassword()
        {
            return View(new ForgotPasswordViewModel());
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var origin = Request.Headers["origin"];
            ForgotPasswordResponse response = await _userService.ForgotPasswordAsync(vm, origin);
            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View(vm);
            }
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public async Task<IActionResult> ResetPassword(string token)
        {
            return View(new ResetPasswordViewModel { Token = token });
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            ResetPasswordResponse response = await _userService.ResetPasswordAsync(vm);
            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View(vm);
            }

            return RedirectToRoute(new { controller = "User", action = "Index" });

        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

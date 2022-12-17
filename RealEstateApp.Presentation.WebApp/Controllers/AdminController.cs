using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.DTOs.Account;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Core.Application.Helpers;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Properties;
using RealEstateApp.Core.Application.ViewModels.Admin;
using RealEstateApp.Core.Application.ViewModels.Users;
using RealEstateApp.Core.Application.Services;
using RealEstateApp.Core.Application.ViewModels.Improvements;
using System.Data;

namespace RealEstateApp.Presentation.WebApp.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IPropertiesService _propertiesService;
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse currentlyUser;
        private readonly IMapper _mapper;

        public AdminController(IUserService userService, IPropertiesService propertiesService, IAccountService accountService, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _propertiesService = propertiesService;
            _accountService = accountService;
            _httpContextAccessor = httpContextAccessor;
            currentlyUser = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            if (currentlyUser.Roles.FirstOrDefault() == Roles.Agent.ToString())
            {
                return RedirectToRoute(new { controller = "Agent", action = "Index" });
            }

            else if (currentlyUser.Roles.FirstOrDefault() == Roles.Client.ToString())
            {
                return RedirectToRoute(new { controller = "Client", action = "Index" });

            }

            else if (currentlyUser.Roles.FirstOrDefault() == Roles.Developer.ToString())
            {
                return RedirectToRoute(new { controller = "Home", action = "AccessDenied" });

            }

            HomeAdminViewModel homeAdminViewModel = new();
            homeAdminViewModel = await _userService.GetUsersQuantity();
            var propertiesVM = await _propertiesService.GetAllWithData();
            homeAdminViewModel.PropertiesQuantity = propertiesVM.Count();

            return View(homeAdminViewModel);
        }

        public async Task<IActionResult> AgentsList()
        {
            if (currentlyUser.Roles.FirstOrDefault() == Roles.Agent.ToString())
            {
                return RedirectToRoute(new { controller = "Agent", action = "Index" });
            }

            else if (currentlyUser.Roles.FirstOrDefault() == Roles.Client.ToString())
            {
                return RedirectToRoute(new { controller = "Client", action = "Index" });

            }

            else if (currentlyUser.Roles.FirstOrDefault() == Roles.Developer.ToString())
            {
                return RedirectToRoute(new { controller = "Home", action = "AccessDenied" });

            }

            var usersList = await _userService.GetAllUsersViewModels();
            List<UserViewModel> AgentsList = usersList.Where(user => user.Role == Roles.Agent.ToString()).ToList();

            List<PropertiesViewModel> propertiesList = await _propertiesService.GetAll();

            foreach (UserViewModel agent in AgentsList)
            {
                agent.PropertiesQuantity = propertiesList.Where(property => property.AgentId == agent.Id).Count();
            }

            return View(AgentsList);
        }

        public async Task<IActionResult> AdminManager()
        {
            if (currentlyUser.Roles.FirstOrDefault() == Roles.Agent.ToString())
            {
                return RedirectToRoute(new { controller = "Agent", action = "Index" });
            }

            else if (currentlyUser.Roles.FirstOrDefault() == Roles.Client.ToString())
            {
                return RedirectToRoute(new { controller = "Client", action = "Index" });

            }

            else if (currentlyUser.Roles.FirstOrDefault() == Roles.Developer.ToString())
            {
                return RedirectToRoute(new { controller = "Home", action = "AccessDenied" });

            }

            var usersList = await _userService.GetAllUsersViewModels();
            List<UserViewModel> adminList = usersList.Where(user => user.Role == Roles.Admin.ToString()).ToList();

            return View(adminList);
        }

        public async Task<IActionResult> DevsManager()
        {
            if (currentlyUser.Roles.FirstOrDefault() == Roles.Agent.ToString())
            {
                return RedirectToRoute(new { controller = "Agent", action = "Index" });
            }

            else if (currentlyUser.Roles.FirstOrDefault() == Roles.Client.ToString())
            {
                return RedirectToRoute(new { controller = "Client", action = "Index" });

            }

            else if (currentlyUser.Roles.FirstOrDefault() == Roles.Developer.ToString())
            {
                return RedirectToRoute(new { controller = "Home", action = "AccessDenied" });

            }

            var usersList = await _userService.GetAllUsersViewModels();
            List<UserViewModel> adminList = usersList.Where(user => user.Role == Roles.Developer.ToString()).ToList();

            return View(adminList);
        }

        public IActionResult RegisterUser(string role)
        {
            if (currentlyUser.Roles.FirstOrDefault() == Roles.Agent.ToString())
            {
                return RedirectToRoute(new { controller = "Agent", action = "Index" });
            }

            else if (currentlyUser.Roles.FirstOrDefault() == Roles.Client.ToString())
            {
                return RedirectToRoute(new { controller = "Client", action = "Index" });

            }

            else if (currentlyUser.Roles.FirstOrDefault() == Roles.Developer.ToString())
            {
                return RedirectToRoute(new { controller = "Home", action = "AccessDenied" });

            }

            if (role == "admin")
            {
                ViewBag.Role = Roles.Admin.ToString();
                return View("SaveUser", new UpdateUserViewModel());
            }
            else if(role == "dev")
            {
                ViewBag.Role = Roles.Developer.ToString();
                return View("SaveUser", new UpdateUserViewModel());
            }

            return RedirectToRoute(new { controller = "Admin", action = "Index" });

        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(UpdateUserViewModel vm, string role)
        {
            if (currentlyUser.Roles.FirstOrDefault() == Roles.Agent.ToString())
            {
                return RedirectToRoute(new { controller = "Agent", action = "Index" });
            }

            else if (currentlyUser.Roles.FirstOrDefault() == Roles.Client.ToString())
            {
                return RedirectToRoute(new { controller = "Client", action = "Index" });

            }

            else if (currentlyUser.Roles.FirstOrDefault() == Roles.Developer.ToString())
            {
                return RedirectToRoute(new { controller = "Home", action = "AccessDenied" });

            }


            if (!ModelState.IsValid)
            {
                ViewBag.Role = role;
                return View("SaveUser", vm);
            }


            //vm.ImagePath = UploadImagesHelper.UploadUserImage(vm.File, vm.UserName);

            SaveUserViewModel saveUserViewModel = _mapper.Map<SaveUserViewModel>(vm);

            var origin = Request.Headers["origin"];
            RegisterResponse response = new RegisterResponse();

            if (role == Roles.Admin.ToString())
            {
                response = await _userService.RegisterAsync(saveUserViewModel, origin, Roles.Admin);

                if (response.HasError)
                {
                    vm.HasError = response.HasError;
                    vm.Error = response.Error;

                    ViewBag.Role = role;
                    return View("SaveUser", vm);
                }

                return RedirectToRoute(new { controller = "Admin", action = "AdminManager" });
            }

            if (role == Roles.Developer.ToString())
            {
                response = await _userService.RegisterAsync(saveUserViewModel, origin, Roles.Developer);

                if (response.HasError)
                {
                    vm.HasError = response.HasError;
                    vm.Error = response.Error;

                    ViewBag.Role = role;
                    return View("SaveUser", vm);
                }

                return RedirectToRoute(new { controller = "Admin", action = "DevsManager" });
            }

            return RedirectToRoute(new { controller = "Admin", action = "Index" });
        }

        public async Task<IActionResult> UpdateUser(string username, string role)
        {
            if (currentlyUser.Roles.FirstOrDefault() == Roles.Agent.ToString())
            {
                return RedirectToRoute(new { controller = "Agent", action = "Index" });
            }

            else if (currentlyUser.Roles.FirstOrDefault() == Roles.Client.ToString())
            {
                return RedirectToRoute(new { controller = "Client", action = "Index" });

            }

            else if (currentlyUser.Roles.FirstOrDefault() == Roles.Developer.ToString())
            {
                return RedirectToRoute(new { controller = "Home", action = "AccessDenied" });

            }

            UpdateUserViewModel userSaveViewModel = await _userService.GetUserSaveViewModelByUsername(username);

            ViewBag.role = null;
            ViewBag.place = role;
            return View("SaveUser", userSaveViewModel);
        }

        [HttpPost]

        public async Task<IActionResult> UpdateUser(UpdateUserViewModel userSaveViewModel, string role)
        {
            if (currentlyUser.Roles.FirstOrDefault() == Roles.Agent.ToString())
            {
                return RedirectToRoute(new { controller = "Agent", action = "Index" });
            }

            else if (currentlyUser.Roles.FirstOrDefault() == Roles.Client.ToString())
            {
                return RedirectToRoute(new { controller = "Client", action = "Index" });

            }

            else if (currentlyUser.Roles.FirstOrDefault() == Roles.Developer.ToString())
            {
                return RedirectToRoute(new { controller = "Home", action = "AccessDenied" });

            }


            if (!ModelState.IsValid)
            {
                ViewBag.Role = null;
                return View("SaveUser", userSaveViewModel);
            }


            var origin = Request.Headers["origin"];

            if (role == Roles.Admin.ToString())
            {
                var response = await _userService.Update(userSaveViewModel);

                if (response.HasError)
                {
                    userSaveViewModel.HasError = response.HasError;
                    userSaveViewModel.Error = response.Error;

                    ViewBag.Role = role;
                    return View(userSaveViewModel);
                }

                return RedirectToRoute(new { controller = "Admin", action = "AdminManager" });
            }

            if (role == Roles.Developer.ToString())
            {
                var response = await _userService.Update(userSaveViewModel);

                if (response.HasError)
                {
                    userSaveViewModel.HasError = response.HasError;
                    userSaveViewModel.Error = response.Error;

                    ViewBag.Role = role;
                    return View(userSaveViewModel);
                }

                return RedirectToRoute(new { controller = "Admin", action = "DevsManager" });
            }

            return RedirectToRoute(new { controller = "User", action = "Index" });
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserStatus(string id, string Role)
        {
            if (currentlyUser.Roles.FirstOrDefault() == Roles.Agent.ToString())
            {
                return RedirectToRoute(new { controller = "Agent", action = "Index" });
            }

            else if (currentlyUser.Roles.FirstOrDefault() == Roles.Client.ToString())
            {
                return RedirectToRoute(new { controller = "Client", action = "Index" });

            }

            else if (currentlyUser.Roles.FirstOrDefault() == Roles.Developer.ToString())
            {
                return RedirectToRoute(new { controller = "Home", action = "AccessDenied" });

            }

            var response = await _accountService.ChageUserStatusAsync(id);

            if(Role == "Developer")
            {
                return RedirectToRoute(new { controller = "Admin", action = "DevsManager" });
            }


            if (Role == "Agent")
            {
                return RedirectToRoute(new { controller = "Admin", action = "AgentsList" });
            }

            return RedirectToRoute(new { controller = "Admin", action = "AdminManager" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (currentlyUser.Roles.FirstOrDefault() == Roles.Agent.ToString())
            {
                return RedirectToRoute(new { controller = "Agent", action = "Index" });
            }

            else if (currentlyUser.Roles.FirstOrDefault() == Roles.Client.ToString())
            {
                return RedirectToRoute(new { controller = "Client", action = "Index" });

            }

            else if (currentlyUser.Roles.FirstOrDefault() == Roles.Developer.ToString())
            {
                return RedirectToRoute(new { controller = "Home", action = "AccessDenied" });

            }

            var response = await _accountService.DeleteUserAsync(id);

            return RedirectToRoute(new { controller = "Admin", action = "AgentsList" });
        }
    }
}

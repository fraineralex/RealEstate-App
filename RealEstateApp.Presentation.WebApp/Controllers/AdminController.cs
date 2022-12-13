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
            HomeAdminViewModel homeAdminViewModel = new();
            homeAdminViewModel = await _userService.GetUsersQuantity();
            var propertiesVM = await _propertiesService.GetAllWithData();
            homeAdminViewModel.PropertiesQuantity = propertiesVM.Count();

            return View(homeAdminViewModel);
        }

        public async Task<IActionResult> AgentsList()
        {
            var usersList = await _userService.GetAllUsersViewModels();
            List<UserViewModel> AgentsList = usersList.Where(user => user.Role == Roles.Agent.ToString()).ToList();

            List<PropertiesViewModel> propertiesList = await _propertiesService.GetAll();

            foreach (UserViewModel agent in AgentsList)
            {
                agent.PropertiesQuantity = propertiesList.Where(property => property.AgentId == agent.Id).Count();
            }

            return View(AgentsList);
        }
    }
}

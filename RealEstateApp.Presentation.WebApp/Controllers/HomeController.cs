using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Services;
using RealEstateApp.Core.Application.ViewModels.Admin;
using RealEstateApp.Core.Application.ViewModels.Properties;
using RealEstateApp.Presentation.WebApp.Models;
using System.Diagnostics;

namespace RealEstateApp.Presentation.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPropertiesService _propertiesService;
        private readonly ITypeOfPropertiesService _typeOfPropertiesService;
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, IPropertiesService propertiesService, IAccountService accountService, IUserService userService, ITypeOfPropertiesService typeOfPropertiesService)
        {
            _logger = logger;
            _propertiesService = propertiesService;
            _accountService = accountService;
            _userService = userService;
            _typeOfPropertiesService = typeOfPropertiesService;
        }

        public async Task<IActionResult> Index()
        {
            var properties = await _propertiesService.GetAllWithInclude();
            ViewBag.TypeOfPropertiesList = await _typeOfPropertiesService.GetAllViewModel();
            return View(properties);
        }

        [HttpPost]
        public async Task<IActionResult> Filters(string? propertyCode, List<int>? propertyIds, decimal minPrice, decimal maxPrice, int bathroomsQuantity, int roomsQuantity)
        {
            FilterPropertiesViewModel filterPropertiesViewModel = new()
            {
                Code = propertyCode,
                Ids = propertyIds,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                NumberOfBathrooms = bathroomsQuantity,
                NumberOfRooms = roomsQuantity
            };

            var properties = await _propertiesService.GetAllWithFilters(filterPropertiesViewModel);
            ViewBag.TypeOfPropertiesList = await _typeOfPropertiesService.GetAllViewModel();
            return View("Index", properties);
        }

        public async Task<IActionResult> Details(int id)
        {

            var propertyDetail = await _propertiesService.GetPropertyDetailsAsync(id);
            return View(propertyDetail);
        }



        public async Task<IActionResult> Agents()
        {
            var usersList = await _userService.GetAllUsersViewModels();
            List<UserViewModel> AgentsList = usersList.Where(user => user.Role == Roles.Agent.ToString()).ToList();

            List<PropertiesViewModel> propertiesList = await _propertiesService.GetAll();

            foreach (UserViewModel agent in AgentsList)
            {
                agent.PropertiesQuantity = propertiesList.Where(property => property.AgentId == agent.Id).Count();
            }

            return View(AgentsList.OrderBy(x => x.FirstName).ToList());
        }

        public async Task<IActionResult> FilterAgent(string agentName)
        {
            var usersList = await _userService.GetAllUsersViewModels();
            List<UserViewModel> AgentsList = usersList.Where(user => user.Role == Roles.Agent.ToString()).ToList();

            agentName = agentName.ToLower();

            AgentsList = AgentsList.Where(agent => agent.FirstName.ToLower() == agentName || agent.LastName.ToLower() == agentName || agent.FirstName.ToLower() + " " + agent.LastName.ToLower() == agentName).ToList();

            List<PropertiesViewModel> propertiesList = await _propertiesService.GetAll();

            foreach (UserViewModel agent in AgentsList)
            {
                agent.PropertiesQuantity = propertiesList.Where(property => property.AgentId == agent.Id).Count();
            }

            return View("Agents", AgentsList.OrderBy(x => x.FirstName).ToList());
        }

        public async Task<IActionResult> AgentProperties(string id, string userName)
        {
            ViewBag.UserName = userName;
            var agentProperties = await _propertiesService.GetAllByAgentIdWithInclude(id);
            return View(agentProperties);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
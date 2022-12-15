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
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, IPropertiesService propertiesService, IAccountService accountService, IUserService userService)
        {
            _logger = logger;
            _propertiesService = propertiesService;
            _accountService = accountService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var properties = await _propertiesService.GetAllWithInclude();
            return View(properties);
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Services;
using RealEstateApp.Core.Application.ViewModels.Admin;
using RealEstateApp.Core.Application.ViewModels.Properties;

namespace RealEstateApp.Presentation.WebApp.Controllers
{
    //[Authorize(Roles = "Client")]                   //Hay que arreglarlo
    public class ClientController : Controller
    {
        private readonly IPropertiesService _propertiesService;
        private readonly IAccountService _accountService;
        private readonly ITypeOfPropertiesService _typeOfPropertiesService;
        private readonly IUserService _userService;


        public ClientController(IPropertiesService propertiesService, IAccountService accountService, ITypeOfPropertiesService typeOfPropertiesService, IUserService userService)
        {
            _propertiesService = propertiesService;
            _accountService = accountService;
            _typeOfPropertiesService = typeOfPropertiesService;
            _userService = userService;
        }



        public async Task<IActionResult> Index()
        {
            var properties = await _propertiesService.GetAllWithInclude();
            ViewBag.TypeOfPropertiesList = await _typeOfPropertiesService.GetAllViewModel();
            return View(properties);
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


        public async Task<IActionResult> MyProperties()
        {
            var properties = await _propertiesService.GetAllWithInclude();
            ViewBag.TypeOfPropertiesList = await _typeOfPropertiesService.GetAllViewModel();
            return View(properties);
        }
    }
}

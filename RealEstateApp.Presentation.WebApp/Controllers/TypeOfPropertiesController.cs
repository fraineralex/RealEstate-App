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
using RealEstateApp.Core.Application.ViewModels.Improvements;
using RealEstateApp.Core.Application.ViewModels.TypeOfProperties;

namespace RealEstateApp.Presentation.WebApp.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class TypeOfPropertiesController : Controller
    {
        private readonly ITypeOfPropertiesService _typeOfPropertiesService;
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse currentlyUser;
        private readonly IMapper _mapper;

        public TypeOfPropertiesController(IUserService userService, ITypeOfPropertiesService typeOfPropertiesService, IAccountService accountService, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _typeOfPropertiesService = typeOfPropertiesService;
            _accountService = accountService;
            _httpContextAccessor = httpContextAccessor;
            currentlyUser = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            List<TypeOfPropertiesViewModel> typeOfPropertiesList = await _typeOfPropertiesService.GetAllViewModel();
            return View(typeOfPropertiesList);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string Name, string Description)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("userVaidation", "Something was wrong");
                return RedirectToRoute(new { controller = "TypeOfProperties", action = "Index" });
            }

            SaveTypeOfPropertiesViewModel typeOfPropertiesSaveViewModel = new()
            {
                Name = Name,
                Description = Description
            };

            await _typeOfPropertiesService.Add(typeOfPropertiesSaveViewModel);

            return RedirectToRoute(new { controller = "TypeOfProperties", action = "Index" });
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _typeOfPropertiesService.Delete(id);
            return RedirectToRoute(new { controller = "TypeOfProperties", action = "Index" });
        }

        public async Task<IActionResult> Update(int id)
        {
            SaveTypeOfPropertiesViewModel typeOfPropertiesSaveViewModel = await _typeOfPropertiesService.GetByIdSaveViewModel(id);

            return View("UpdateTypeOfProperties", typeOfPropertiesSaveViewModel);
        }

        [HttpPost]

        public async Task<IActionResult> Update(SaveTypeOfPropertiesViewModel typeOfPropertiesSaveViewModel)
        {
            await _typeOfPropertiesService.Update(typeOfPropertiesSaveViewModel, typeOfPropertiesSaveViewModel.Id);

            return RedirectToRoute(new { controller = "TypeOfProperties", action = "Index" });
        }

    }
}

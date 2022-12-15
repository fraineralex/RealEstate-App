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
using RealEstateApp.Core.Application.ViewModels.TypeOfSales;

namespace RealEstateApp.Presentation.WebApp.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class TypeOfSalesController : Controller
    {
        private readonly ITypeOfSalesService _typeOfSalesService;
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse currentlyUser;
        private readonly IMapper _mapper;

        public TypeOfSalesController(IUserService userService, ITypeOfSalesService typeOfSalesService, IAccountService accountService, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _typeOfSalesService = typeOfSalesService;
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

            List<TypeOfSalesViewModel> typeOfSalesList = await _typeOfSalesService.GetAllViewModel();
            return View(typeOfSalesList);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string Name, string Description)
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
                ModelState.AddModelError("userVaidation", "Something was wrong");
                return RedirectToRoute(new { controller = "TypeOfSales", action = "Index" });
            }

            SaveTypeOfSalesViewModel typeOfSalesSaveViewModel = new()
            {
                Name = Name,
                Description = Description
            };

            await _typeOfSalesService.Add(typeOfSalesSaveViewModel);

            return RedirectToRoute(new { controller = "TypeOfSales", action = "Index" });
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
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

            await _typeOfSalesService.Delete(id);
            return RedirectToRoute(new { controller = "TypeOfSales", action = "Index" });
        }

        public async Task<IActionResult> Update(int id)
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

            SaveTypeOfSalesViewModel typeOfSalesSaveViewModel = await _typeOfSalesService.GetByIdSaveViewModel(id);

            return View("UpdateTypeOfSales", typeOfSalesSaveViewModel);
        }

        [HttpPost]

        public async Task<IActionResult> Update(SaveTypeOfSalesViewModel typeOfSalesSaveViewModel)
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

            await _typeOfSalesService.Update(typeOfSalesSaveViewModel, typeOfSalesSaveViewModel.Id);

            return RedirectToRoute(new { controller = "TypeOfSales", action = "Index" });
        }

    }
}

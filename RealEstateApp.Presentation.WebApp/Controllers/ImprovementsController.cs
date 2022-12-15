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

namespace RealEstateApp.Presentation.WebApp.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class ImprovementsController : Controller
    {
        private readonly IImprovementsService _improvementsService;
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse currentlyUser;
        private readonly IMapper _mapper;

        public ImprovementsController(IUserService userService, IImprovementsService improvementsService, IAccountService accountService, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _improvementsService = improvementsService;
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

            List<ImprovementsViewModel> improvementsList = await _improvementsService.GetAllViewModel();
            return View(improvementsList);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string ImprovementName, string ImprovementDescription)
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
                return RedirectToRoute(new { controller = "Improvements", action = "Index" });
            }

            SaveImprovementsViewModel improvementsSaveViewModel = new()
            {
                Name = ImprovementName,
                Description = ImprovementDescription
            };

            await _improvementsService.Add(improvementsSaveViewModel);

            return RedirectToRoute(new { controller = "Improvements", action = "Index" });
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

            await _improvementsService.Delete(id);
            return RedirectToRoute(new { controller = "Improvements", action = "Index" });
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

            SaveImprovementsViewModel improvementsSaveViewModel = await _improvementsService.GetByIdSaveViewModel(id);

            return View("UpdateImprovement", improvementsSaveViewModel);
        }

        [HttpPost]

        public async Task<IActionResult> Update(SaveImprovementsViewModel improvementsSaveViewModel)
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

            await _improvementsService.Update(improvementsSaveViewModel, improvementsSaveViewModel.Id);

            return RedirectToRoute(new { controller = "Improvements", action = "Index" });
        }

    }
}

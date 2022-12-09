using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.DTOs.Account;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Core.Application.Helpers;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Properties;

namespace RealEstateApp.Presentation.WebApp.Controllers
{
    //[Authorize(Roles = "Agent")]
    public class AgentController : Controller
    {
        private readonly IPropertiesService _propertiesService;
        private readonly IAccountService _accountService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse userviewModel;
        private readonly IMapper _mapper;

        public AgentController(IPropertiesService propertiesService, IAccountService accountService, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _propertiesService = propertiesService;
            _accountService = accountService;
            _httpContextAccessor = httpContextAccessor;
            userviewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }



        public async Task<IActionResult> MyProfile()
        {
            return View("UpdateAgentProfile", await _propertiesService.GetAgentUserByUserNameAsync(userviewModel.UserName));
        }

        [HttpPost]
        public async Task<IActionResult> MyProfile(SaveAgentProfileViewModel agentProfileViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("UpdateAgentProfile", await _propertiesService.GetAgentUserByUserNameAsync(userviewModel.UserName));

            }

            SaveAgentProfileViewModel agentProfile = await _propertiesService.GetAgentUserByUserNameAsync(userviewModel.UserName);

            if (agentProfileViewModel.File != null)
            {
                if (agentProfile.ImagePath == null || agentProfile.ImagePath == "")
                {
                    agentProfileViewModel.ImagePath = UploadImagesHelper.UploadAgentUserImage(agentProfileViewModel.File, userviewModel.UserName);
                }

                else if (agentProfile.ImagePath != null && agentProfile.ImagePath != "")
                {
                    agentProfileViewModel.ImagePath = UploadImagesHelper.UploadAgentUserImage(agentProfileViewModel.File, userviewModel.UserName, true, agentProfile.ImagePath);
                }
            }

            var result = await _propertiesService.UpdateAgentProfile(agentProfileViewModel);

            if (result.HasError)
            {
                return View("UpdateAgentProfile", await _propertiesService.GetAgentUserByUserNameAsync(userviewModel.UserName));
            }



            return RedirectToRoute(new { controller = "Agent", action = "Index" });

        }




    }
}

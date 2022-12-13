using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.DTOs.Account;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Core.Application.Helpers;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Properties;
using System.Runtime.CompilerServices;

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
        private readonly ITypeOfPropertiesService _typeOfPropertiesService;
        private readonly ITypeOfSalesService _typeOfSalesService;
        private readonly IImprovementsService _improvementsService;

        public AgentController(IPropertiesService propertiesService, IAccountService accountService, IHttpContextAccessor httpContextAccessor, IMapper mapper, ITypeOfPropertiesService typeOfPropertiesService, IImprovementsService improvementsService, ITypeOfSalesService typeOfSalesService)
        {
            _propertiesService = propertiesService;
            _accountService = accountService;
            _httpContextAccessor = httpContextAccessor;
            userviewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _mapper = mapper;
            _typeOfPropertiesService = typeOfPropertiesService;
            _improvementsService = improvementsService;
            _typeOfSalesService = typeOfSalesService;
        }

        public IActionResult Index()
        {
            

            return View();
        }

        public async Task<IActionResult> Create()
        {
            SavePropertiesViewModel vm = new SavePropertiesViewModel();
            vm.TypeOfProperties = await _typeOfPropertiesService.GetAllViewModel();
            vm.TypeOfSales = await _typeOfSalesService.GetAllViewModel();
            vm.Improvements = await _improvementsService.GetAllViewModel();
            return View("SaveProperty", vm);

        }

        [HttpPost]
        public async Task<IActionResult> Create(SavePropertiesViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.TypeOfProperties = await _typeOfPropertiesService.GetAllViewModel();
                vm.TypeOfSales = await _typeOfSalesService.GetAllViewModel();
                vm.Improvements = await _improvementsService.GetAllViewModel();
                return View("SaveProperty", vm);
            }
            vm.AgentId = userviewModel.Id;
            SavePropertiesViewModel savePropertiesVmAdded = await _propertiesService.CustomAdd(vm);

            if (savePropertiesVmAdded != null && savePropertiesVmAdded.Id != 0)
            {
                savePropertiesVmAdded.ImagePathOne = UploadImagesHelper.UploadPropertyImage(vm.ImageFileOne, savePropertiesVmAdded.Id);

                if (vm.ImageFileTwo != null)
                {
                    savePropertiesVmAdded.ImagePathTwo = UploadImagesHelper.UploadPropertyImage(vm.ImageFileTwo, savePropertiesVmAdded.Id);
                }

                else if (vm.ImageFileThree != null)
                {
                    savePropertiesVmAdded.ImagePathThree = UploadImagesHelper.UploadPropertyImage(vm.ImageFileThree, savePropertiesVmAdded.Id);

                }

                else if(vm.ImageFileFour != null)
                {
                    savePropertiesVmAdded.ImagePathFour = UploadImagesHelper.UploadPropertyImage(vm.ImageFileFour, savePropertiesVmAdded.Id);
                }
            }


            
            await _propertiesService.Update(savePropertiesVmAdded, savePropertiesVmAdded.Id);
            return RedirectToRoute(new { controller = "Agent", action = "Index" });

        }

        public async Task<IActionResult> Edit(int id)
        {
            var vm = await _propertiesService.GetByIdWithData(id);
            return View("SaveProperty", vm);
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

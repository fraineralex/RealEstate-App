using AutoMapper;
using Microsoft.AspNetCore.Http;
using RealEstateApp.Core.Application.DTOs.Account;
using RealEstateApp.Core.Application.Helpers;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Properties;
using RealEstateApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Services
{
    public class PropertiesService : GenericService<SavePropertiesViewModel, PropertiesViewModel, Properties>, IPropertiesService
    {
        private readonly IPropertiesRepository _propertiesRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse userviewModel;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        public PropertiesService(IPropertiesRepository propertiesRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper, IAccountService accountService) : base(propertiesRepository, mapper)
        {
            _propertiesRepository = propertiesRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            userviewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _accountService = accountService;
        }


        public async Task<SaveAgentProfileViewModel> UpdateAgentProfile (SaveAgentProfileViewModel agentProfileViewModel)
        {
            var agentProfileToUpdate = _mapper.Map<UpdateAgentUserRequest>(agentProfileViewModel);
            agentProfileToUpdate.UserName = userviewModel.UserName;


            var response = await _accountService.UpdateAgentUserByUserNameAsync(agentProfileToUpdate);
            return  _mapper.Map<SaveAgentProfileViewModel>(response);
        }

        public async Task<SaveAgentProfileViewModel> GetAgentUserByUserNameAsync(string userName)
        {
            var agentUser = await _accountService.GetAgentUserByUserNameAsync(userName);
            return _mapper.Map<SaveAgentProfileViewModel>(agentUser);
        }


    }
}

using RealEstateApp.Core.Application.ViewModels.Properties;
using RealEstateApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IPropertiesService : IGenericService<SavePropertiesViewModel, PropertiesViewModel, Properties>
    {
        Task<SaveAgentProfileViewModel> UpdateAgentProfile(SaveAgentProfileViewModel agentProfileViewModel);

        Task<SaveAgentProfileViewModel> GetAgentUserByUserNameAsync(string userName);
    }
}

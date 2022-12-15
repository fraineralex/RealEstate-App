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
        Task<PropertiesViewModel> GetByCode(string code);
        Task<PropertiesViewModel> GetByIdWithData(int id);
        Task<List<PropertiesViewModel>> GetAllWithData();
        Task<SavePropertiesViewModel> CustomAdd(SavePropertiesViewModel savePropertiesViewModel);
        Task<SaveAgentProfileViewModel> UpdateAgentProfile(SaveAgentProfileViewModel agentProfileViewModel);
        Task<SaveAgentProfileViewModel> GetAgentUserByUserNameAsync(string userName);
        Task<List<PropertiesViewModel>> GetAll();

        Task<SavePropertiesViewModel> AddWithImprovementsAsync(SavePropertiesViewModel savePropertiesViewModel);

        Task AddImprovementsAsync(SavePropertiesViewModel savePropertiesViewModel);
        Task<List<PropertiesViewModel>> GetAllWithInclude();
        Task<SavePropertiesViewModel> GetByIdWithInclude(int id);
        Task<List<PropertiesViewModel>> GetAllByAgentIdWithInclude(string agentId);

        Task UpdatePropertyWithImprovementsAsync(SavePropertiesViewModel savePropertiesViewModel, int id);
        Task<PropertyDetailsViewModel> GetPropertyDetailsAsync(int propertyId);
        Task<List<PropertiesViewModel>> GetAllWithFilters(FilterPropertiesViewModel filterPropertiesViewModel);

        Task DeleteImprovementsToProperties(int id);

    }
}

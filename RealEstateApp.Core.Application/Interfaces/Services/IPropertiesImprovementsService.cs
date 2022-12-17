using RealEstateApp.Core.Application.ViewModels.PropertiesImprovements;
using RealEstateApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IPropertiesImprovementsService: IGenericService<SavePropertiesImprovementsViewModel, PropertiesImprovementsViewModel, PropertiesImprovements>
    {
    }
}

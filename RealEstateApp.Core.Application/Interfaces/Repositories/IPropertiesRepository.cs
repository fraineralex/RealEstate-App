using RealEstateApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Interfaces.Repositories
{
    public interface IPropertiesRepository : IGenericRepository<Properties>
    {
        Task AddImprovementsToProperties(Properties property);
        Task UpdateImprovementsToProperties(Properties property);
        Task DeleteImprovementsToProperties(int id);

    }
}

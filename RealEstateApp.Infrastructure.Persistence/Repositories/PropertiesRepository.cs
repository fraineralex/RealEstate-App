using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Infrastructure.Persistence.Contexts;
using RealEstateApp.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Infrastructure.Persistence.Repositories
{
    public class PropertiesRepository : GenericRepository<Properties>, IPropertiesRepository
    {
        private readonly ApplicationContext _dbContext;

        public PropertiesRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddImprovementsToProperties(Properties property)
        {
            PropertiesImprovements propertiesImprovements = new PropertiesImprovements();
            propertiesImprovements.PropertyId = property.Id;

            List<PropertiesImprovements> propertiesImprovementsList = new List<PropertiesImprovements>();

            foreach (var item in property.Improvements)
            {
                propertiesImprovements.ImprovementId = item.Id;
                propertiesImprovementsList.Add(propertiesImprovements);
            }

           
            await _dbContext.PropertiesImprovements.AddRangeAsync(propertiesImprovementsList);
            await _dbContext.SaveChangesAsync();

        }

        


    }
}

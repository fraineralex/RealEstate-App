using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Infrastructure.Persistence.Contexts;
using RealEstateApp.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
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



    }
}

using AutoMapper;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.PropertiesImprovements;
using RealEstateApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Services
{
    public class PropertiesImprovementsService : GenericService<SavePropertiesImprovementsViewModel, PropertiesImprovementsViewModel, PropertiesImprovements>, IPropertiesImprovementsService
    {
        private readonly IGenericRepository<PropertiesImprovements> _repository;
        private readonly IMapper _mapper;
        public PropertiesImprovementsService(IGenericRepository<PropertiesImprovements> repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
    }
}

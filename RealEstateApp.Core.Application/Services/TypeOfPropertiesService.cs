using AutoMapper;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.TypeOfProperties;
using RealEstateApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Services
{
    public class TypeOfPropertiesService : GenericService<SaveTypeOfPropertiesViewModel, TypeOfPropertiesViewModel, TypeOfProperties>, ITypeOfPropertiesService
    {
        private readonly IGenericRepository<TypeOfProperties> _repository;
        private readonly IMapper _mapper;
        public TypeOfPropertiesService(IGenericRepository<TypeOfProperties> repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
    }
}

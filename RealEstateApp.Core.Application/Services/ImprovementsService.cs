using AutoMapper;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Improvements;
using RealEstateApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Services
{
    public class ImprovementsService: GenericService<SaveImprovementsViewModel, ImprovementsViewModel, Improvements>, IImprovementsService
    {
        private readonly IGenericRepository<Improvements> _repository;
        private readonly IMapper _mapper;
        public ImprovementsService(IGenericRepository<Improvements> repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
    }
}

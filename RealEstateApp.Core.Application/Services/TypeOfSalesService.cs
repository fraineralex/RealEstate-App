using AutoMapper;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.TypeOfSales;
using RealEstateApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Services
{
    public class TypeOfSalesService: GenericService<SaveTypeOfSalesViewModel, TypeOfSalesViewModel, TypeOfSales>, ITypeOfSalesService
    {
        private readonly IGenericRepository<TypeOfSales> _repository;
        private readonly IMapper _mapper;
        public TypeOfSalesService(IGenericRepository<TypeOfSales> repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
    }
}

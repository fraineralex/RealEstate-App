using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.ViewModels.TypeOfProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Features.TypeOfProperties.Queries.GetAllTypeOfProperties
{
    public class GetAllTypeOfPropertiesQuery : IRequest<IEnumerable<TypeOfPropertiesViewModel>>
    {
        public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllTypeOfPropertiesQuery, IEnumerable<TypeOfPropertiesViewModel>>
        {

            private readonly ITypeOfPropertiesRepository _TypeOfPropertiesRepository;
            private readonly IMapper _mapper;
            public GetAllCategoriesQueryHandler(ITypeOfPropertiesRepository TypeOfPropertiesRepository, IMapper mapper)
            {
                _TypeOfPropertiesRepository = TypeOfPropertiesRepository;
                _mapper = mapper;
            }

            public async Task<IEnumerable<TypeOfPropertiesViewModel>> Handle(GetAllTypeOfPropertiesQuery request, CancellationToken cancellationToken)
            {
                var typeOfPropertiesViewModel = await GetAllViewModel();
                return typeOfPropertiesViewModel;
            }

            private async Task<List<TypeOfPropertiesViewModel>> GetAllViewModel()
            {
                var typeOfPropertiesList = await _TypeOfPropertiesRepository.GetAllAsync();
                if (typeOfPropertiesList.Count() == 0) throw new Exception("No existen tipos de propiedades.");
                var result = _mapper.Map<List<TypeOfPropertiesViewModel>>(typeOfPropertiesList);
                return result;
            }
        }
    }
}

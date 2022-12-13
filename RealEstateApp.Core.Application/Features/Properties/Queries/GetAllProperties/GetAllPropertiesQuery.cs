using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.ViewModels.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Features.Properties.Queries.GetAllProperties
{
    public class GetAllPropertiesQuery : IRequest<IEnumerable<PropertiesViewModel>>
    {
        public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllPropertiesQuery, IEnumerable<PropertiesViewModel>>
        {

            private readonly IPropertiesRepository _PropertiesRepository;
            private readonly IMapper _mapper;
            public GetAllCategoriesQueryHandler(IPropertiesRepository PropertiesRepository, IMapper mapper)
            {
                _PropertiesRepository = PropertiesRepository;
                _mapper = mapper;
            }

            public async Task<IEnumerable<PropertiesViewModel>> Handle(GetAllPropertiesQuery request, CancellationToken cancellationToken)
            {
                var PropertiesViewModel = await GetAllViewModel();
                return PropertiesViewModel;
            }

            private async Task<List<PropertiesViewModel>> GetAllViewModel()
            {
                var propertiesList = await _PropertiesRepository.GetAllAsync();
                if (propertiesList.Count() == 0) throw new Exception("No existen propiedades.");
                var result = await _PropertiesRepository.GetAllWithIncludeAsync(new List<string> { "Improvements", "TypeOfProperty", "TypeOfSale" });
                return _mapper.Map<List<PropertiesViewModel>>(result);
            }
        }
    }
}


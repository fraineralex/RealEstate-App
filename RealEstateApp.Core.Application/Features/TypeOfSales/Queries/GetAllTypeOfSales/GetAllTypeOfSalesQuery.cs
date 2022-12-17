using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.ViewModels.TypeOfSales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Features.TypeOfSales.Queries.GetAllTypeOfSales
{
    public class GetAllTypeOfSalesQuery : IRequest<IEnumerable<TypeOfSalesViewModel>>
    {
        public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllTypeOfSalesQuery, IEnumerable<TypeOfSalesViewModel>>
        {

            private readonly ITypeOfSalesRepository _TypeOfSalesRepository;
            private readonly IMapper _mapper;
            public GetAllCategoriesQueryHandler(ITypeOfSalesRepository TypeOfSalesRepository, IMapper mapper)
            {
                _TypeOfSalesRepository = TypeOfSalesRepository;
                _mapper = mapper;
            }

            public async Task<IEnumerable<TypeOfSalesViewModel>> Handle(GetAllTypeOfSalesQuery request, CancellationToken cancellationToken)
            {
                var typeOfSalesViewModel = await GetAllViewModel();
                return typeOfSalesViewModel;
            }

            private async Task<List<TypeOfSalesViewModel>> GetAllViewModel()
            {
                var typeOfSalesList = await _TypeOfSalesRepository.GetAllAsync();
                if (typeOfSalesList.Count() == 0) throw new Exception("No existen tipos de ventas.");
                var result = _mapper.Map<List<TypeOfSalesViewModel>>(typeOfSalesList);
                return result;
            }
        }
    }
}

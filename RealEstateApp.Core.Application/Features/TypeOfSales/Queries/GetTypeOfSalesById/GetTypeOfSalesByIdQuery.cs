using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.ViewModels.TypeOfSales;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Features.TypeOfSales.Queries.GetTypeOfSalesById
{
    public class GetTypeOfSalesByIdQuery : IRequest<TypeOfSalesViewModel>
    {
        [SwaggerParameter(Description = "El Id del tipo de venta que se quiere consultar")]
        public int Id { get; set; }
    }
    public class GetTypeOfSalesByIdQueryHandler : IRequestHandler<GetTypeOfSalesByIdQuery, TypeOfSalesViewModel>
    {
        private readonly ITypeOfSalesRepository _TypeOfSalesRepository;
        private readonly IMapper _mapper;

        public GetTypeOfSalesByIdQueryHandler(ITypeOfSalesRepository TypeOfSalesRepository, IMapper mapper)
        {
            _TypeOfSalesRepository = TypeOfSalesRepository;
            _mapper = mapper;
        }

        public async Task<TypeOfSalesViewModel> Handle(GetTypeOfSalesByIdQuery query, CancellationToken cancellationToken)
        {
            var TypeOfSale = await _TypeOfSalesRepository.GetByIdAsync(query.Id);
            if (TypeOfSale is null) throw new Exception("No existe el tipo de venta.");
            var result = _mapper.Map<TypeOfSalesViewModel>(TypeOfSale);
            return result;
        }
    }
    
}

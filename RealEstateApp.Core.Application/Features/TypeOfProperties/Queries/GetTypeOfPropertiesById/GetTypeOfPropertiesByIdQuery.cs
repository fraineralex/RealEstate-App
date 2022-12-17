using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.ViewModels.TypeOfProperties;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Features.TypeOfProperties.Queries.GetTypeOfPropertiesById
{
    public class GetTypeOfPropertiesByIdQuery : IRequest<TypeOfPropertiesViewModel>
    {
        [SwaggerParameter(Description = "El Id del tipo de propiedad que desea consultar")]
        public int Id { get; set; }
    }
    public class GetTypeOfPropertiesByIdQueryHandler : IRequestHandler<GetTypeOfPropertiesByIdQuery, TypeOfPropertiesViewModel>
    {
        private readonly ITypeOfPropertiesRepository _TypeOfPropertiesRepository;
        private readonly IMapper _mapper;

        public GetTypeOfPropertiesByIdQueryHandler(ITypeOfPropertiesRepository TypeOfPropertiesRepository, IMapper mapper)
        {
            _TypeOfPropertiesRepository = TypeOfPropertiesRepository;
            _mapper = mapper;
        }

        public async Task<TypeOfPropertiesViewModel> Handle(GetTypeOfPropertiesByIdQuery query, CancellationToken cancellationToken)
        {
            var typeOfProperty = await _TypeOfPropertiesRepository.GetByIdAsync(query.Id);
            if (typeOfProperty is null) throw new Exception("No existe el tipo de propiedad.");
            var result = _mapper.Map<TypeOfPropertiesViewModel>(typeOfProperty);
            return result;
        }
    }
   
}

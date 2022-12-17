using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Features.TypeOfSales.Commands.UpdateTypeOfSales
{
    using RealEstateApp.Core.Domain.Entities;
    using Swashbuckle.AspNetCore.Annotations;
    using System.ComponentModel.DataAnnotations;

    public class TypeOfSalesUpdateCommand : IRequest<TypeOfSalesUpdateResponse>
    {
        public int Id { get; set; }
        [SwaggerParameter(Description = "El nombre del tipo de venta")]
        [Required(ErrorMessage = "El nombre es requerido.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "La descripcion es requerida.")]
        [SwaggerParameter(Description = "La descripcion del tipo de venta")]
        public string Description { get; set; }
    }

    public class UpdateTypeOfSalesCommandHandler : IRequestHandler<TypeOfSalesUpdateCommand, TypeOfSalesUpdateResponse>
    {
        private readonly ITypeOfSalesRepository _typeOfSalesRepository;
        private readonly IMapper _mapper;
        public UpdateTypeOfSalesCommandHandler(ITypeOfSalesRepository improvementRepository, IMapper mapper)
        {
            _typeOfSalesRepository = improvementRepository;
            _mapper = mapper;
        }
        public async Task<TypeOfSalesUpdateResponse> Handle(TypeOfSalesUpdateCommand command, CancellationToken cancellationToken)
        {
            var improvement = await _typeOfSalesRepository.GetByIdAsync(command.Id);

            if (improvement == null) throw new Exception("El tipo de venta no ha sido encontrada.");

            improvement = _mapper.Map<TypeOfSales>(command);

            await _typeOfSalesRepository.UpdateAsync(improvement, improvement.Id);

            var improvementResponse = _mapper.Map<TypeOfSalesUpdateResponse>(improvement);

            return improvementResponse;
        }
    }
    
}

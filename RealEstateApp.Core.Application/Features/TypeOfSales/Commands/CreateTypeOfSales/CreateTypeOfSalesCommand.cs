using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;

namespace RealEstateApp.Core.Application.Features.TypeOfSales.Commands.CreateTypeOfSales
{
    using RealEstateApp.Core.Domain.Entities;
   

    public class CreateTypeOfSalesCommand : IRequest<int>
    {
        //public int Id { get; set; }
        [SwaggerParameter(Description = "El nombre del tipo de venta")]
        [Required(ErrorMessage = "El nombre es requerido.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "La descripcion es requerida.")]
        [SwaggerParameter(Description = "La descripcion del tipo de venta")]
        public string Description { get; set; }
    }

    public class CreateTypeOfSalesCommandHandler : IRequestHandler<CreateTypeOfSalesCommand, int>
    {
        private readonly ITypeOfSalesRepository _improvementsRepository;
        private readonly IMapper _mapper;
        public CreateTypeOfSalesCommandHandler(ITypeOfSalesRepository improvementsRepository, IMapper mapper)
        {
            _improvementsRepository = improvementsRepository;
            _mapper = mapper;
        }
        public async Task<int> Handle(CreateTypeOfSalesCommand command, CancellationToken cancellationToken)
        {
            var improvements = _mapper.Map<TypeOfSales>(command);
            improvements = await _improvementsRepository.AddAsync(improvements);
            return improvements.Id;
        }
    }
}

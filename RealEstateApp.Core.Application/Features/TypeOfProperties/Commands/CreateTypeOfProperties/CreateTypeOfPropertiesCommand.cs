using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Features.TypeOfProperties.Commands.CreateTypeOfProperties
{
    using RealEstateApp.Core.Domain.Entities;
    public class CreateTypeOfPropertiesCommand : IRequest<int>
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es requerido.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "La descripcion es requerida.")]
        public string Description { get; set; }
    }
     
    public class CreateTypeOfPropertiesCommandHandler : IRequestHandler<CreateTypeOfPropertiesCommand, int>
    {
        private readonly ITypeOfPropertiesRepository _typeOfPropertiesRepository;
        private readonly IMapper _mapper;
        public CreateTypeOfPropertiesCommandHandler(ITypeOfPropertiesRepository typeOfPropertiesRepository, IMapper mapper)
        {
            _typeOfPropertiesRepository = typeOfPropertiesRepository;
            _mapper = mapper;
        }
        public async Task<int> Handle(CreateTypeOfPropertiesCommand command, CancellationToken cancellationToken)
        {
            var property = _mapper.Map<TypeOfProperties>(command);
            property = await _typeOfPropertiesRepository.AddAsync(property);
            return property.Id;
        }
    }
}

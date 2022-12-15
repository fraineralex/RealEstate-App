using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Features.TypeOfProperties.Commands.UpdateTypeOfProperties
{
    using RealEstateApp.Core.Domain.Entities;
    using Swashbuckle.AspNetCore.Annotations;
    using System.ComponentModel.DataAnnotations;

    public class UpdateTypeOfPropertiesCommand : IRequest<UpdateTypeOfPropertiesResponse>
    {
        public int Id { get; set; }
        [SwaggerParameter(Description = "El nombre del tipo de propiedad")]
        [Required(ErrorMessage = "El nombre es requerido.")]
        public string Name { get; set; }
        [SwaggerParameter(Description = "La descripcion del tipo de propiedad")]
        [Required(ErrorMessage = "La descripcion es requerida.")]
        public string Description { get; set; }
    }

    public class UpdateTypeOfPropertiesCommandHandler : IRequestHandler<UpdateTypeOfPropertiesCommand, UpdateTypeOfPropertiesResponse>
    {
        private readonly ITypeOfPropertiesRepository _improvementsRepository;
        private readonly IMapper _mapper;
        public UpdateTypeOfPropertiesCommandHandler(ITypeOfPropertiesRepository improvementRepository, IMapper mapper)
        {
            _improvementsRepository = improvementRepository;
            _mapper = mapper;
        }
        public async Task<UpdateTypeOfPropertiesResponse> Handle(UpdateTypeOfPropertiesCommand command, CancellationToken cancellationToken)
        {
            var improvement = await _improvementsRepository.GetByIdAsync(command.Id);

            if (improvement == null) throw new Exception("El tipo de propiedad no ha sido encontrada.");

            improvement = _mapper.Map<TypeOfProperties>(command);

            await _improvementsRepository.UpdateAsync(improvement, improvement.Id);

            var improvementResponse = _mapper.Map<UpdateTypeOfPropertiesResponse>(improvement);

            return improvementResponse;
        }
    }
    
}

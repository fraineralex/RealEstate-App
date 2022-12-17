using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using System.ComponentModel.DataAnnotations;


namespace RealEstateApp.Core.Application.Features.Improvements.Commands.CreateImprovements
{
    using RealEstateApp.Core.Domain.Entities;
    using Swashbuckle.AspNetCore.Annotations;

    public class CreateImprovementsCommand : IRequest<int>
    {
        //public int Id { get; set; }
        [SwaggerParameter(Description = "El nombre de la mejora")]
        [Required(ErrorMessage = "El nombre es requerido.")]
        public string Name { get; set; }
        [SwaggerParameter(Description = "La descripcion de la mejora")]
        [Required(ErrorMessage = "La descripcion es requerida.")]
        public string Description { get; set; }
    }

    public class CreateImprovementsCommandHandler : IRequestHandler<CreateImprovementsCommand, int>
    {
        private readonly IImprovementsRepository _improvementsRepository;
        private readonly IMapper _mapper;
        public CreateImprovementsCommandHandler(IImprovementsRepository improvementsRepository, IMapper mapper)
        {
            _improvementsRepository = improvementsRepository;
            _mapper = mapper;
        }
        public async Task<int> Handle(CreateImprovementsCommand command, CancellationToken cancellationToken)
        {
            var improvements = _mapper.Map<Improvements>(command);
            improvements = await _improvementsRepository.AddAsync(improvements);
            return improvements.Id;
        }
    }

}

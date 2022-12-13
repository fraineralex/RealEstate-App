using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Features.Improvements.Commands.UpdateImprovements
{
    using RealEstateApp.Core.Domain.Entities;
    public class UpdateImprovementsCommand : IRequest<UpdateImprovementsResponse>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class UpdateImprovementsCommandHandler : IRequestHandler<UpdateImprovementsCommand, UpdateImprovementsResponse>
    {
        private readonly IImprovementsRepository _improvementsRepository;
        private readonly IMapper _mapper;
        public UpdateImprovementsCommandHandler(IImprovementsRepository improvementRepository, IMapper mapper)
        {
            _improvementsRepository = improvementRepository;
            _mapper = mapper;
        }
        public async Task<UpdateImprovementsResponse> Handle(UpdateImprovementsCommand command, CancellationToken cancellationToken)
        {
            var improvement = await _improvementsRepository.GetByIdAsync(command.Id);

            if (improvement == null) throw new Exception("La mejora no ha sido encontrada.");

            improvement = _mapper.Map<Improvements>(command);

            await _improvementsRepository.UpdateAsync(improvement, improvement.Id);

            var improvementResponse = _mapper.Map<UpdateImprovementsResponse>(improvement);

            return improvementResponse;
        }
    }
}

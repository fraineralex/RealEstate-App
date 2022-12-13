using MediatR;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Features.Improvements.Commands.DeleteImprovementsById
{
    public class DeleteImprovementsByIdCommand : IRequest<int>
    {
        public int Id { get; set; }
    }

    public class DeleteImprovementsByIdCommandHandler : IRequestHandler<DeleteImprovementsByIdCommand, int>
    {
        private readonly IImprovementsRepository _improvementsRepository;
        public DeleteImprovementsByIdCommandHandler(IImprovementsRepository improvementsRepository)
        {
            _improvementsRepository = improvementsRepository;
        }
        public async Task<int> Handle(DeleteImprovementsByIdCommand command, CancellationToken cancellationToken)
        {
            var improvements = await _improvementsRepository.GetByIdAsync(command.Id);

            if (improvements == null) throw new Exception("La mejora no fue encontrado.");

            await _improvementsRepository.DeleteAsync(improvements);

            return improvements.Id;
        }
    }
}

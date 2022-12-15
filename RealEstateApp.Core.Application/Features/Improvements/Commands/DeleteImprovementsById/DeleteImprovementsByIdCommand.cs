using MediatR;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Features.Improvements.Commands.DeleteImprovementsById
{
    public class DeleteImprovementsByIdCommand : IRequest<int>
    {
        [SwaggerParameter(Description = "El Id de la mejora que desea eliminar")]
        public int Id { get; set; }
    }

    public class DeleteImprovementsByIdCommandHandler : IRequestHandler<DeleteImprovementsByIdCommand, int>
    {
        private readonly IImprovementsRepository _improvementsRepository;
        private readonly IPropertiesRepository _propertiesRepository;
        public DeleteImprovementsByIdCommandHandler(IImprovementsRepository improvementsRepository, IPropertiesRepository propertiesRepository)
        {
            _improvementsRepository = improvementsRepository;
            _propertiesRepository = propertiesRepository;
        }
        public async Task<int> Handle(DeleteImprovementsByIdCommand command, CancellationToken cancellationToken)
        {
            var improvements = await _improvementsRepository.GetByIdAsync(command.Id);

            if (improvements == null) throw new Exception("La mejora no fue encontrado.");

            var properties = await _propertiesRepository.GetAllAsync();

            var propertiesRelational = properties.Where(x => x.ImprovementsId == command.Id).ToList();

            if (propertiesRelational.Count() != 0)
            {
                foreach (var property in propertiesRelational)
                {
                    await _propertiesRepository.DeleteAsync(property);
                }
            }

            await _improvementsRepository.DeleteAsync(improvements);

            return improvements.Id;
        }
    }
}

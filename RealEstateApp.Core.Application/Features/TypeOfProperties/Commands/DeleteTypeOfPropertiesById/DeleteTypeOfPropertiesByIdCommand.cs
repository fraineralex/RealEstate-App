using MediatR;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Features.TypeOfProperties.Commands.DeleteTypeOfPropertiesById
{
    public class DeleteTypeOfPropertiesByIdCommand : IRequest<int>
    {
        public int Id { get; set; }
    }

    public class DeleteTypeOfPropertiesByIdCommandHandler : IRequestHandler<DeleteTypeOfPropertiesByIdCommand, int>
    {
        private readonly ITypeOfPropertiesRepository _typeOfPropertiesRepository;
        public DeleteTypeOfPropertiesByIdCommandHandler(ITypeOfPropertiesRepository typeOfPropertiesRepository)
        {
            _typeOfPropertiesRepository = typeOfPropertiesRepository;
        }
        public async Task<int> Handle(DeleteTypeOfPropertiesByIdCommand command, CancellationToken cancellationToken)
        {
            var typeOfProperties = await _typeOfPropertiesRepository.GetByIdAsync(command.Id);

            if (typeOfProperties == null) throw new Exception("El tipo de propiedad no fue encontrado.");

            await _typeOfPropertiesRepository.DeleteAsync(typeOfProperties);

            return typeOfProperties.Id;
        }
    }
 
}

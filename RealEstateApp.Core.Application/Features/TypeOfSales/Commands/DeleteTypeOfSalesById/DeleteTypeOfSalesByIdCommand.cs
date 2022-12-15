using MediatR;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using Swashbuckle.AspNetCore.Annotations;

namespace RealEstateApp.Core.Application.Features.TypeOfSales.Commands.DeleteTypeOfSales
{
    public class DeleteTypeOfSalesByIdCommand : IRequest<int>
    {
        [SwaggerParameter(Description = "El Id del tipo de venta que se quiere eliminar")]
        public int Id { get; set; }
    }

    public class DeleteTypeOfSalesByIdCommandHandler : IRequestHandler<DeleteTypeOfSalesByIdCommand, int>
    {
        private readonly ITypeOfSalesRepository _typeOfSalesRepository;
        private readonly IPropertiesRepository _propertiesRepository;
        public DeleteTypeOfSalesByIdCommandHandler(ITypeOfSalesRepository typeOfSalesRepository, IPropertiesRepository propertiesRepository)
        {
            _typeOfSalesRepository = typeOfSalesRepository;
            _propertiesRepository = propertiesRepository;
        }
        public async Task<int> Handle(DeleteTypeOfSalesByIdCommand command, CancellationToken cancellationToken)
        {
            var typeOfSales = await _typeOfSalesRepository.GetByIdAsync(command.Id);

            if (typeOfSales == null) throw new Exception("El tipo de venta no fue encontrado.");

            var properties = await _propertiesRepository.GetAllAsync();
                                                            
            var propertiesRelational = properties.Where(x => x.TypeOfSaleId == command.Id).ToList();

            if (propertiesRelational.Count() != 0)
            {
                foreach (var property in propertiesRelational)
                {
                    await _propertiesRepository.DeleteAsync(property);
                }
            }

            await _typeOfSalesRepository.DeleteAsync(typeOfSales);

            return typeOfSales.Id;
        }
    }
}

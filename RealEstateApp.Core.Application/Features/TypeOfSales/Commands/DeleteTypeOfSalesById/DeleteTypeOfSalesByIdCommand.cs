using MediatR;
using RealEstateApp.Core.Application.Interfaces.Repositories;

namespace RealEstateApp.Core.Application.Features.TypeOfSales.Commands.DeleteTypeOfSales
{
    public class DeleteTypeOfSalesByIdCommand : IRequest<int>
    {
        public int Id { get; set; }
    }

    public class DeleteTypeOfSalesByIdCommandHandler : IRequestHandler<DeleteTypeOfSalesByIdCommand, int>
    {
        private readonly ITypeOfSalesRepository _typeOfSalesRepository;
        public DeleteTypeOfSalesByIdCommandHandler(ITypeOfSalesRepository typeOfSalesRepository)
        {
            _typeOfSalesRepository = typeOfSalesRepository;
        }
        public async Task<int> Handle(DeleteTypeOfSalesByIdCommand command, CancellationToken cancellationToken)
        {
            var typeOfSales = await _typeOfSalesRepository.GetByIdAsync(command.Id);

            if (typeOfSales == null) throw new Exception("El tipo de venta no fue encontrado.");

            await _typeOfSalesRepository.DeleteAsync(typeOfSales);

            return typeOfSales.Id;
        }
    }
}

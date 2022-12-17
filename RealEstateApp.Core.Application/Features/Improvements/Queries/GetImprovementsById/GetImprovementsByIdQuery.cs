using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.ViewModels.Improvements;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Features.Improvements.Queries.GetImprovementsById
{
    public class GetImprovementsByIdQuery : IRequest<ImprovementsViewModel>
    {
        [SwaggerParameter(Description = "El Id de la mejora que desea consultar")]
        public int Id { get; set; }
    }
    public class GetImprovementsByIdQueryHandler : IRequestHandler<GetImprovementsByIdQuery, ImprovementsViewModel>
    {
        private readonly IImprovementsRepository _ImprovementsRepository;
        private readonly IMapper _mapper;

        public GetImprovementsByIdQueryHandler(IImprovementsRepository ImprovementsRepository, IMapper mapper)
        {
            _ImprovementsRepository = ImprovementsRepository;
            _mapper = mapper;
        }

        public async Task<ImprovementsViewModel> Handle(GetImprovementsByIdQuery query, CancellationToken cancellationToken)
        {
            var improvement = await _ImprovementsRepository.GetByIdAsync(query.Id);
            if (improvement is null) throw new Exception("No existe la mejora.");
            var result = _mapper.Map<ImprovementsViewModel>(improvement);
            return result;
        }
    }
}
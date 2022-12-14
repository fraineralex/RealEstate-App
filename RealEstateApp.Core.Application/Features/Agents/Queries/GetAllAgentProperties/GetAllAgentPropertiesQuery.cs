using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.ViewModels.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Features.Agents.Queries.GetAllAgentProperties
{
    public class GetAllAgentPropertiesQuery : IRequest<IEnumerable<PropertiesViewModel>>
    {
        public string Id { get; set; } 
    }
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllAgentPropertiesQuery, IEnumerable<PropertiesViewModel>>
    {
        private readonly IPropertiesRepository _propertiesRepository;
        private readonly IMapper _mapper;
        public GetAllCategoriesQueryHandler(IPropertiesRepository propertiesRepository, IMapper mapper)
        {
            _propertiesRepository = propertiesRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PropertiesViewModel>> Handle(GetAllAgentPropertiesQuery query, CancellationToken cancellationToken)
        {
            var properties = await _propertiesRepository.GetAllWithIncludeAsync(new List<string> { "Improvements", "TypeOfProperty", "TypeOfSale" });
            var propertiesByAgentId = properties.Where(x => x.AgentId == query.Id);
            if (propertiesByAgentId == null || propertiesByAgentId.Count() == 0) throw new Exception("No existen las propiedades.");
            var result = _mapper.Map<List<PropertiesViewModel>>(propertiesByAgentId);
            return result;
        }
    }
    }


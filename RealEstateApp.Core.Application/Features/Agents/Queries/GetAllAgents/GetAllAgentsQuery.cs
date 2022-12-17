using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Agents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Features.Agents.Queries.GetAllAgents
{
    public class GetAllAgentsQuery : IRequest<IEnumerable<AgentsViewModel>>
    {
    }
    public class GetAllAgentsQueryHandler : IRequestHandler<GetAllAgentsQuery, IEnumerable<AgentsViewModel>>
    {
        private readonly IAccountService _accountService;
        private readonly IPropertiesRepository _propertiesRepository;
        private readonly IMapper _mapper;
        public GetAllAgentsQueryHandler(IAccountService accountService, IPropertiesRepository propertiesRepository, IMapper mapper)
        {
            _accountService = accountService;
            _propertiesRepository = propertiesRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AgentsViewModel>> Handle(GetAllAgentsQuery query, CancellationToken cancellationToken)
        {
            var agents = await _accountService.GetAllAgents();
            if (agents.Count() == 0) throw new Exception("No existen las propiedades.");
            var properties = await _propertiesRepository.GetAllAsync();
            foreach (var agent in agents)
            {
                agent.PropertiesQuantity = properties.Where(x => x.AgentId == agent.Id).Count();
            }
            return agents;

        }
    }
}

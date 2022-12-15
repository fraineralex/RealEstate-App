using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Agents;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Features.Agents.Queries.GetAgentsById
{
    public class GetAgentsByIdQuery : IRequest<AgentsViewModel>
    {
        [SwaggerParameter(Description = "El Id del agente que desea consultar")]
        public string Id { get; set; }
    }
    public class GetAgentsByIdQueryHandler : IRequestHandler<GetAgentsByIdQuery, AgentsViewModel>
    {
        private readonly IAccountService _accountService;
        private readonly IPropertiesRepository _propertiesRepository;
        private readonly IMapper _mapper;

        public GetAgentsByIdQueryHandler(IAccountService accountService, IPropertiesRepository PropertiesRepository, IMapper mapper)
        {
            _accountService = accountService;
            _propertiesRepository = PropertiesRepository;
            _mapper = mapper;
        }

        public async Task<AgentsViewModel> Handle(GetAgentsByIdQuery query, CancellationToken cancellationToken)
        {
            var users = await _accountService.GetAllUsers();
            var userFiltered = users.FirstOrDefault(x => x.Id == query.Id);
            if (userFiltered is null) throw new Exception("No existe el agente.");
            var properties = await _propertiesRepository.GetAllAsync();
            var agentPropeties = properties.Where(x => x.AgentId == query.Id);
            userFiltered.PropertiesQuantity = agentPropeties.Count();
            return userFiltered;
        }
    }
}

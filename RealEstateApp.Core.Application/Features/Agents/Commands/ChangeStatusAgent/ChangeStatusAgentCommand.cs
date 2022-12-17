using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Features.Agents.Commands.ChangeStatusAgent;
using RealEstateApp.Core.Application.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Features.Agents.Commands.CreateAgents
{
    public class ChangeStatusAgentCommand: IRequest<bool>
    {
        [SwaggerParameter(Description = "El Id del agente")]
        public string Id { get; set; }
        [SwaggerParameter(Description = "El estado a cambiar")]
        public bool Status { get; set; }
    }
    public class ChangeStatusAgentCommandHandler : IRequestHandler<ChangeStatusAgentCommand, bool>
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        public ChangeStatusAgentCommandHandler(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }
        public async Task<bool> Handle(ChangeStatusAgentCommand command, CancellationToken cancellationToken)
        {
            var users = await _accountService.GetAllUsers();
            var user = users.FirstOrDefault(x => x.Id == command.Id);
            if (user is null) throw new Exception("El agente no existe");
            var result = await _accountService.ChangesStatusUser(command.Id, command.Status);
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Features.Agents.Commands.ChangeStatusAgent
{
    public class ChangeStatusAgentResponse
    {
        public string Id { get; set; }
        public bool Status { get; set; }
    }
}

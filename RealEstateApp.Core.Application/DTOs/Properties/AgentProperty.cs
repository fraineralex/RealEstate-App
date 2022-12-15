using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.DTOs.Properties
{
    public class AgentProperty
    {
        public string Id { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string UserName { get; set; }
        public string? IDCard { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? ImagePath { get; set; }


    }
}

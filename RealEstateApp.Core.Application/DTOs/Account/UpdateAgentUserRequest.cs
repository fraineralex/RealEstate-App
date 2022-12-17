using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.DTOs.Account
{
    public class UpdateAgentUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string? ImagePath { get; set; }
        public IFormFile? File { get; set; }

        public bool HasError { get; set; }
        public string? Error { get; set; }

        public string? UserName { get; set; }

    }
}

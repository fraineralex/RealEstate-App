
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.DTOs.Account
{
    public class UpdateUserRequest
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? UserName { get; set; }

        public string? Password { get; set; }

        public string? Email { get; set; }

        public string? IDCard { get; set; }

        public bool HasError { get; set; }
        public string? Error { get; set; }

    }
}

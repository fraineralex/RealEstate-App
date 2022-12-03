using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.DTOs.Account
{
    public class ResetPasswordRequest
    {
        public string? Email { get; set; }
        public string? Token { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.ViewModels.Users
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "You must type the email")]
        [DataType(DataType.Text)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "You must type the password")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}

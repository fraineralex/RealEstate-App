using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.ViewModels.Properties
{
    public class SaveAgentProfileViewModel
    {
        [Required(ErrorMessage = "You must type your First Name")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "You must type your Last Name")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "You must type the phone number")]
        [DataType(DataType.Text)]
        public string Phone { get; set; }

        public string? ImagePath { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? File { get; set; }

        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}

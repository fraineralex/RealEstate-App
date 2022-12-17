using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.ViewModels.Users
{
    public class SaveUserViewModel
    {

        [Required(ErrorMessage = "You must type your First Name")]
        [DataType(DataType.Text)]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "You must type your Last Name")]
        [DataType(DataType.Text)]
        public string? LastName { get; set; }

        //[Required(ErrorMessage = "You must type your ID Card")]
        //[DataType(DataType.Text)]
        //public string? IDCard { get; set; }


        [Required(ErrorMessage = "You must type the User Name")]
        [DataType(DataType.Text)]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "You must type the Password")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Passwords must be the same")]
        [Required(ErrorMessage = "You must type the password")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }


        [Required(ErrorMessage = "You must type the Email")]
        [DataType(DataType.Text)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "You must type the Phone Number")]
        [DataType(DataType.Text)]
        public string? Phone { get; set; }

        public string? ImagePath { get; set; }
        [Required(ErrorMessage = "You must type the ID card")]
        [DataType(DataType.Text)]
        public string? IDCard { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? File { get; set; }

        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}

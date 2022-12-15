
namespace RealEstateApp.Core.Application.ViewModels.Admin
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Role { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string UserName { get; set; }
        public string? IDCard { get; set; }
        public string? Email { get; set; }
        public bool Status { get; set; }
        public string? ImagePath { get; set; }
        public int PropertiesQuantity { get; set; }

    }
}

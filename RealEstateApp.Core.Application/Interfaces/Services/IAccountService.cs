using RealEstateApp.Core.Application.DTOs.Account;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Core.Application.ViewModels.Admin;
using RealEstateApp.Core.Application.ViewModels.Users;

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<RegisterResponse> RegisterUserAsync(RegisterRequest request, string origin, Roles tyoeOfUser);
        Task SignOutAsync();
        Task<string> ConfirmAccountAsync(string userId, string token);
        Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request);

        Task<UpdateAgentUserResponse> UpdateAgentUserByUserNameAsync(UpdateAgentUserRequest request);
        Task<UpdateAgentUserResponse> GetAgentUserByUserNameAsync(string userName);
        Task<HomeAdminViewModel> GetUsersQuantity();
        Task<List<UserViewModel>> GetAllUserViewModels();
    }
}

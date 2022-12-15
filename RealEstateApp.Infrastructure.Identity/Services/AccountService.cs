using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RealEstateApp.Core.Application.DTOs.Account;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Domain.Settings;
using RealEstateApp.Infrastructure.Identity.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.WebUtilities;
using RealEstateApp.Core.Application.DTOs.Email;
using RealEstateApp.Core.Application.ViewModels.Users;
using RealEstateApp.Core.Application.ViewModels.Admin;
using RealEstateApp.Core.Application.ViewModels.Agents;
using RealEstateApp.Core.Application.DTOs.Properties;
using MediatR;

namespace RealEstateApp.Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JWTSettings _jwtSettings;
        private readonly IEmailService _emailService;


        public AccountService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<JWTSettings> jwtSettings,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
            _emailService = emailService;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            AuthenticationResponse response = new();

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No Accounts registered with {request.Email}";
                return response;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Invalid credentials for {request.Email}";
                return response;
            }
            if (!user.EmailConfirmed)
            {
                response.HasError = true;
                response.Error = $"Account no confirmed for {request.Email}";
                return response;
            }

            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);

            response.Id = user.Id;
            response.Email = user.Email;
            response.UserName = user.UserName;
            response.ImagePath = user.ImagePath;
            response.FirstName = user.FirstName;
            response.LastName = user.LastName;

            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            response.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            var refreshToken = GenerateRefreshToken();
            response.RefreshToken = refreshToken.Token;

            return response;
        }

        public async Task<RegisterResponse> RegisterUserAsync(RegisterRequest request, string origin, Roles typeOfUser)
        {
            RegisterResponse response = new()
            {
                HasError = false
            };

            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.Error = $"username '{request.UserName}' is already taken.";
                return response;
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail != null)
            {
                response.HasError = true;
                response.Error = $"Email '{request.Email}' is already registered.";
                return response;
            }

            var user = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                ImagePath = request.ImagePath,
                IDCard = request.IDCard,
                EmailConfirmed = request.EmailConfirmed

            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded == false)
            {
                response.HasError = true;
                foreach (var error in result.Errors)
                {
                    response.Error = error.Description;
                }
                return response;
            }

            await _userManager.AddToRoleAsync(user, typeOfUser.ToString());

            if (typeOfUser.ToString() == Roles.Client.ToString()) 
            {
                var verificationUri = await SendVerificationEmailUri(user, origin);
                await _emailService.SendEmailAsync(new EmailRequest()
                {
                    To = user.Email,
                    Body = $"Please confirm your account by clicking this URL {verificationUri}",
                    Subject = "Confirm Account of Real State App"
                });
            }

            return response;
        }
        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<string> ConfirmAccountAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return $"There is no account registered with this email {user.Email}";
            }

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return $"Account confirmed for {user.Email}. You can now use the Real State App";
            }
            else
            {
                return $"An error has occurred while trying to confirm the email '{user.Email}'";
            }
        }

        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
        {
            ForgotPasswordResponse response = new()
            {
                HasError = false
            };

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                response.HasError = true;
                response.Error = $"There is no account registered with the email '{request.Email}'";
                return response;
            }

            var verificationUri = await SendForgotPasswordUri(user, origin);
            await _emailService.SendEmailAsync(new EmailRequest()
            {
                To = user.Email,
                Body = $"Please reset your password by clicking this URL {verificationUri} from the Real State App",
                Subject = "Reset Password"
            });

            return response;
        }

        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            ResetPasswordResponse response = new()
            {
                HasError = false
            };

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                response.HasError = true;
                response.Error = $"There is no account registered with this email '{request.Email}'";
                return response;
            }

            request.Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"An error occurred while reset password";
                return response;
            }
            return response;

        }

        public async Task<UpdateAgentUserResponse> UpdateAgentUserByUserNameAsync(UpdateAgentUserRequest request)
        {
            UpdateAgentUserResponse response = new() { HasError = false};

            var user = _userManager.FindByNameAsync(request.UserName).Result;

            if (user == null)
            {
                response.HasError = true;
                response.Error = "Agent User Not Found";
                return response;
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.Phone;
            user.ImagePath = request.ImagePath;
            //user.IDCard = request.IDCard;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = "An Error Ocurred While Changing The User Data";
                return response;
            }

            return response;
        }

        public async Task<UpdateAgentUserResponse> UpdateUserAsync(UpdateUserViewModel request)
        {
            UpdateAgentUserResponse response = new() { HasError = false };

            var user = _userManager.FindByNameAsync(request.UserName).Result;

            if (user == null)
            {
                response.HasError = true;
                response.Error = "User Not Found";
                return response;
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.IDCard = request.IDCard;
            user.UserName = request.UserName;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = "An Error Ocurred While Changing The User Data";
                return response;
            }

            if(request.Password != null && request.ConfirmPassword != null && request.CurrentPassword != null)
            {
                var change = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.Password);

                if (!change.Succeeded)
                {
                    response.HasError = true;
                    response.Error = "Incorrect current password";
                    return response;
                }

            }

            return response;
        }

        public async Task<UpdateAgentUserResponse>GetAgentUserByUserNameAsync(string userName)
        {
            UpdateAgentUserResponse response = new() { HasError = false };

            var user = _userManager.FindByNameAsync(userName).Result;

            if (user == null)
            {
                response.HasError = true;
                response.Error = "Agent User Not Found";
                return response;
            }

            response.FirstName = user.FirstName;
            response.LastName = user.LastName;
            response.Phone = user.PhoneNumber;
            response.ImagePath = user.ImagePath;

            return response;
        }

        public async Task<HomeAdminViewModel> GetUsersQuantity()
        {
            HomeAdminViewModel response = new();

            var users = _userManager.Users.ToList();
            foreach (var user in users) 
            {
                var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                if (user.EmailConfirmed)
                {
                    if (rolesList.Contains(Roles.Agent.ToString()))
                    {
                        response.ActiveAgentsQuantity += 1;
                    }

                    if (rolesList.Contains(Roles.Client.ToString()))
                    {
                        response.ActiveClientsQuantity += 1;
                    }

                    if (rolesList.Contains(Roles.Developer.ToString()))
                    {
                        response.ActiveDevsQuantity += 1;
                    }
                }
                else
                {
                    if (rolesList.Contains(Roles.Agent.ToString()))
                    {
                        response.UnactiveAgentsQuantity += 1;
                    }

                    if (rolesList.Contains(Roles.Client.ToString()))
                    {
                        response.UnactiveClientsQuantity += 1;
                    }

                    if (rolesList.Contains(Roles.Developer.ToString()))
                    {
                        response.UnactiveDevsQuantity += 1;
                    }
                }

            }

            return response;
        }
        public async Task<bool> ChangesStatusUser(string id, bool status)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) throw new Exception("El usuario no existe.");
            if(status == false)
            {
                user.EmailConfirmed = false;
            } else
            {
                user.EmailConfirmed = true;
            }
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return false;
            else return true;
        }

        public async Task<List<AgentsViewModel>> GetAllAgents()
        {
            var agents = await _userManager.GetUsersInRoleAsync("Agent");
            List<AgentsViewModel> response = new();
            foreach (var user in agents)
            {
                AgentsViewModel agent = new();
                agent.Id = user.Id;
                agent.FirstName = user.FirstName;
                agent.LastName = user.LastName;
                agent.Email = user.Email;
                agent.Phone = user.PhoneNumber;
                response.Add(agent);
            }
            return response;
        }
        public async Task<List<AgentsViewModel>> GetAllUsers()
        {
            List<AgentsViewModel> response = new();
            var users = _userManager.Users.ToList();
            foreach (var user in users)
            {
                AgentsViewModel agent = new();
                agent.Id = user.Id;
                agent.FirstName = user.FirstName;
                agent.LastName = user.LastName;
                agent.Email = user.Email;
                agent.Phone = user.PhoneNumber;
                response.Add(agent);
            }
            return response;
        }
        public async Task<List<UserViewModel>> GetAllUserViewModels() 
        {
            List<UserViewModel> response = new();

            var users = _userManager.Users.ToList();
            foreach (var user in users)
            {
                UserViewModel userViewModel = new UserViewModel();

                var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

                    if (rolesList.Contains(Roles.Agent.ToString()))
                    {
                        userViewModel.Role = Roles.Agent.ToString();
                        userViewModel.Id = user.Id;
                        userViewModel.FirstName = user.FirstName;
                        userViewModel.LastName = user.LastName;
                        userViewModel.UserName = user.UserName;
                        userViewModel.Email = user.Email;
                        userViewModel.Status = user.EmailConfirmed;
                        userViewModel.IDCard = user.IDCard;
                        userViewModel.ImagePath = user.ImagePath;

                    }

                    if (rolesList.Contains(Roles.Client.ToString()))
                    {
                        userViewModel.Role = Roles.Client.ToString();
                        userViewModel.Id = user.Id;
                        userViewModel.FirstName = user.FirstName;
                        userViewModel.LastName = user.LastName;
                        userViewModel.UserName = user.UserName;
                        userViewModel.Email = user.Email;
                        userViewModel.Status = user.EmailConfirmed;
                        userViewModel.IDCard = user.IDCard;
                        userViewModel.ImagePath = user.ImagePath;
                    }

                    if (rolesList.Contains(Roles.Developer.ToString()))
                    {
                        userViewModel.Role = Roles.Developer.ToString();
                        userViewModel.Id = user.Id;
                        userViewModel.FirstName = user.FirstName;
                        userViewModel.LastName = user.LastName;
                        userViewModel.UserName = user.UserName;
                        userViewModel.Email = user.Email;
                        userViewModel.Status = user.EmailConfirmed;
                        userViewModel.IDCard = user.IDCard;
                        userViewModel.ImagePath = user.ImagePath;
                    }

                    if (rolesList.Contains(Roles.Admin.ToString()))
                    {
                        userViewModel.Role = Roles.Admin.ToString();
                        userViewModel.Id = user.Id;
                        userViewModel.FirstName = user.FirstName;
                        userViewModel.LastName = user.LastName;
                        userViewModel.UserName = user.UserName;
                        userViewModel.Email = user.Email;
                        userViewModel.Status = user.EmailConfirmed;
                        userViewModel.IDCard = user.IDCard;
                        userViewModel.ImagePath = user.ImagePath;
                    }

                response.Add(userViewModel);

            }

            return response;

        }

        public async Task<ChangeUserStatusResponse> ChageUserStatusAsync(string id)
        {
            ChangeUserStatusResponse response = new() { HasError = false };

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                response.HasError = true;
                response.Error = "User Not Found";
                return response;
            }

            if (user.EmailConfirmed)
            {
                user.EmailConfirmed = false;
            }
            else
            {
                user.EmailConfirmed = true;
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = "An Error Ocurred While Changing the User Status";
                return response;
            }

            return response;
        }

        public async Task<ChangeUserStatusResponse> DeleteUserAsync(string id)
        {
            ChangeUserStatusResponse response = new() { HasError = false };

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                response.HasError = true;
                response.Error = "User Not Found";
                return response;
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = "An Error Ocurred While Changing the User Status";
                return response;
            }

            return response;
        }

        public async Task<AgentProperty> GetAgentPropertyByIdAsync(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            AgentProperty agentProperty = new AgentProperty();

            if (user != null)
            {
                agentProperty.Id = user.Id;
                agentProperty.FirstName = user.FirstName;
                agentProperty.LastName = user.LastName;
                agentProperty.UserName = user.UserName;
                agentProperty.IDCard = user.IDCard;
                agentProperty.Email = user.Email;
                agentProperty.Phone = user.PhoneNumber;
                agentProperty.ImagePath = user.ImagePath;
            }


            return agentProperty;

        }



        #region "Private Methods"

        private async Task<JwtSecurityToken> GenerateJWToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmectricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredetials = new SigningCredentials(symmectricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredetials);

            return jwtSecurityToken;
        }

        private RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow
            };
        }

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var ramdomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(ramdomBytes);

            return BitConverter.ToString(ramdomBytes).Replace("-", "");
        }

        private async Task<string> SendVerificationEmailUri(ApplicationUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "User/ConfirmEmail";
            var Uri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(Uri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "token", code);

            return verificationUri;
        }

        private async Task<string> SendForgotPasswordUri(ApplicationUser user, string origin)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "User/ResetPassword";
            var Uri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(Uri.ToString(), "token", code);

            return verificationUri;
        }


        #endregion
    }
}

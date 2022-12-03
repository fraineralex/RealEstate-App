using Microsoft.AspNetCore.Http;
using RealEstateApp.Core.Application.DTOs.Account;
using RealEstateApp.Core.Application.Helpers;

namespace RealEstateApp.Presentation.WebApp.Middlewares
{
    public class ValidateUserSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ValidateUserSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool HasUser()
        {
            AuthenticationResponse userVm = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");

            if (userVm == null)
            {
                return false;
            }
            return true;

        }
    }
}

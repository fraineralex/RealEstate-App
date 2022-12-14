using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.DTOs.Account;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Core.Application.Features.Accounts.Commands.RegisterAdminUser;
using RealEstateApp.Core.Application.Features.Accounts.Commands.RegisterDeveloperUser;
using RealEstateApp.Core.Application.Features.Accounts.Queries.Authenticate;
using RealEstateApp.Core.Application.Interfaces.Services;

namespace RealEstateApp.Presentation.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseApiController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("Authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
        {
            return Ok(await Mediator.Send(new AuthenticateUserQuery { Email = request.Email, Password = request.Password }));
        }

        [HttpPost("RegisterAdminUser")]
        public async Task<IActionResult> RegisterAdminAsync(RegisterAdminUserCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("RegisterDeveloperUser")]
        public async Task<IActionResult> RegisterDeveloperAsync(RegisterDeveloperUserCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}

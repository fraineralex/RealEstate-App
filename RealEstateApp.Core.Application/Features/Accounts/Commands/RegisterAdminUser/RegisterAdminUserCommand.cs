using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.DTOs.Account;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Core.Application.Interfaces.Services;

namespace RealEstateApp.Core.Application.Features.Accounts.Commands.RegisterAdminUser
{
    //public class RegisterAdminUserCommand : IRequest<RegisterRequest>
    //{
    //    public string? FirstName { get; set; }
    //    public string? LastName { get; set; }
    //    public string? Email { get; set; }
    //    public string? UserName { get; set; }
    //    public string? Password { get; set; }
    //    public string? ConfirmPassword { get; set; }
    //    public string? Phone { get; set; }
    //    public string? ImagePath { get; set; }
    //}

    //public class RegisterAdminUserCommandHandler : IRequestHandler<RegisterAdminUserCommand, RegisterRequest>
    //{
    //    private readonly IAccountService _accountService;
    //    private readonly IMapper _mapper;

    //    public RegisterAdminUserCommandHandler(IAccountService accountService, IMapper mapper)
    //    {
    //        _accountService = accountService;
    //        _mapper = mapper;
    //    }
    //    public async Task<RegisterRequest> Handle(RegisterAdminUserCommand command, CancellationToken cancellationToken)
    //    {
    //        var request = _mapper.Map<RegisterRequest>(command);
    //        //return await _accountService.RegisterUserAsync(request, ", Roles.Admin);
    //    }
    //}
}

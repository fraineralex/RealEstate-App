using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.DTOs.Account;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Core.Application.Features.Accounts.Commands.RegisterAdminUser;
using RealEstateApp.Core.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Features.Accounts.Commands.RegisterDeveloperUser
{
    public class RegisterDeveloperUserCommand : IRequest<RegisterResponse>
    {
        [Required(ErrorMessage = "El nombre es requerido.")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "El apellido es requerido.")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "El correo es requerido.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "La contraseña es requerida.")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "La confirmacion de contraseña es requerida.")]
        [Compare(nameof(Password), ErrorMessage = "Las contraseña deben de ser iguales.")]
        public string? ConfirmPassword { get; set; }
        public string? Phone { get; set; }
        public string? ImagePath { get; set; }
        public bool EmailConfirmed { get; set; }
    }
    public class RegisterDeveloperUserCommandHandler : IRequestHandler<RegisterDeveloperUserCommand, RegisterResponse>
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public RegisterDeveloperUserCommandHandler(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }
        public async Task<RegisterResponse> Handle(RegisterDeveloperUserCommand command, CancellationToken cancellationToken)
        {
            command.EmailConfirmed = true;
            var request = _mapper.Map<RegisterRequest>(command);
            return await _accountService.RegisterUserAsync(request, "", Roles.Developer);
        }
    }
}

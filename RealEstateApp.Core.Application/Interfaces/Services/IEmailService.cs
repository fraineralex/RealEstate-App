using RealEstateApp.Core.Application.DTOs.Email;
using RealEstateApp.Core.Domain.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IEmailService
    {
        public MailSettings _mailSettings { get; }
        Task SendEmailAsync(EmailRequest request);
    }
}

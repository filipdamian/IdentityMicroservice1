using IdentityMicroservice.Application.ViewModels.External.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMicroservice.Infrastructure.Services.HttpClients.EmailSender
{
    public  interface IEmailSender
    {
        //void SendEmail(MessageUsers message);
        Task SendEmailAsync(MessageUsers message);
    }
}

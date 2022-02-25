using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Text;
using System.Threading.Tasks;
using IdentityMicroservice.Application.Common.Configurations;

namespace IdentityMicroservice.Infrastructure.Services.HttpClients.EmailSender
{
    public class EmailSenderSetting 
    {
        public const string NAME = "EmailSender";
        public string From { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }
}

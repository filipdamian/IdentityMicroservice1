using IdentityMicroservice.Application.Common.Exceptions;
using IdentityMicroservice.Application.Common.Interfaces;
using IdentityMicroservice.Application.ViewModels.External.Email;
using IdentityMicroservice.Domain.Entities;
using IdentityMicroservice.Infrastructure.Persistence.DbContexts.Identity;
using IdentityMicroservice.Infrastructure.Services.HttpClients.EmailSender;
using System.Threading.Tasks;

namespace IdentityMicroservice.Infrastructure.Services.Managers.Email
{
    public class EmailManager : IEmailManager
    {
        private readonly IEmailSender _emailSender;
        private readonly IdentityDbContext _context;
        public EmailManager(IEmailSender emailSender, IdentityDbContext context)
        {
            _emailSender = emailSender; _context = context;
        }

        public async Task<bool> IsEmailConfirmed(string userIntroducedToken, IdentityUserTokenConfirmation obj,IdentityUser user)
        {
            if (userIntroducedToken == obj.ConfirmationToken)
            {
                obj.IsUsed = true;
                user.EmailConfirmed = true;
                int updateResult = await _context.SaveChangesAsync();
                return updateResult == 2;

            }
            return false;

        }

        public async Task SendEmailConfirmation(IdentityUser user,IdentityUserTokenConfirmation token)
        {
            try
            {
                //generate random code or link to send to body
               // var message = new MessageUsers(new string[] { user.Email }, "Email Confirmation", $"This is the confirmation code:{token.ConfirmationToken}");
                var message = new MessageUsers(new string[] { user.Email }, "Email Confirmation", $"This is the confirmation code:https://localhost:4200/auth/email-confirmation?token={token.ConfirmationToken}");
                await _emailSender.SendEmailAsync(message); 
             
            }
            catch 
            {

                throw new MailConfirmationFailedToSendException("Failed to send mail confirmation");
            }

        }
    }
}

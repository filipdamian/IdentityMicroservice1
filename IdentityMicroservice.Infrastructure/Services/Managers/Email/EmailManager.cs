using IdentityMicroservice.Application.Common.Exceptions;
using IdentityMicroservice.Application.Common.Interfaces;
using IdentityMicroservice.Application.ViewModels.External.Email;
using IdentityMicroservice.Domain.Entities;
using IdentityMicroservice.Infrastructure.Persistence.DbContexts.Identity;
using IdentityMicroservice.Infrastructure.Services.HttpClients.EmailSender;
using System;
using System.Text.RegularExpressions;
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

        public async Task<bool> IsEmailConfirmed(string userIntroducedToken, IdentityUserTokenConfirmation obj, IdentityUser user)
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

        public bool IsValidEmail(string email)
        {
            if (email != null)
            {
                return Regex.IsMatch(email, "^(([^<>()\\[\\]\\\\.,;:\\s@\"]+(\\.[^<> ()\\[\\]\\\\.,;:\\s@\"]+)*)|(\".+ \"))@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}])|(([a-zA-Z\\-0-9]+\\.)+[a-zA-Z]{2,}))$");
            }

            return false;
        }

        public async Task SendEmailConfirmation(IdentityUser user, IdentityUserTokenConfirmation token)
        {
            try
            {

                //generate random code or link to send to body
                // var message = new MessageUsers(new string[] { user.Email }, "Email Confirmation", $"This is the confirmation code:{token.ConfirmationToken}");
                var message = new MessageUsers(new string[] { user.Email }, "Email Confirmation", $"This is the confirmation code {token.ConfirmationToken}");
                //var message = new MessageUsers(new string[] { user.Email }, "Email Confirmation", $"This is the confirmation code for desktop:http://localhost:4200/unauthenticated/auth/email-confirmation/{token.ConfirmationToken} and this for mobile ${sixCodeDigit}");
                //EmailMessageModel message = new EmailMessageModel
                //{
                //    Email = user.Email,
                //    Subject = "Email Confirmation",
                //    Content = $"This is the confirmation code:http://localhost:4200/auth/email-confirmation/{token.ConfirmationToken}"
                //};
                await _emailSender.SendEmailAsync(message); 


            }
            catch
            {

                throw new MailConfirmationFailedToSendException("Failed to send mail confirmation");
            }

        }
        public async Task<bool> SendPasswordRecoveryEmail(IdentityUser user, IdentityUserTokenConfirmation token)
        {
            try
            {
                var message = new MessageUsers(new string[] { user.Email }, "Password Recovery Link", $"This is the link for password recovery http://localhost:4200/unauthenticated/auth/forgot-password/{token.ConfirmationToken}");
                //EmailMessageModel message = new EmailMessageModel
                //{
                //    Email = user.Email,
                //    Subject = "Password Recovery Link",
                //    Content = $"This is the link for password recovery http://localhost:4200/auth/forgot-password/{token.ConfirmationToken}"
                //};
                await _emailSender.SendEmailAsync(message);
                return true;
            }
            catch
            {
                return false;

            }

        }
    }
}

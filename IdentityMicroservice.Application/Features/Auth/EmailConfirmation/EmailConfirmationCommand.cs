using IdentityMicroservice.Application.Common.Exceptions;
using IdentityMicroservice.Application.Common.Interfaces;
using IdentityMicroservice.Domain.Entities;
using IdentityMicroservice.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityMicroservice.Application.Features.Auth.EmailConfirmation
{
    public class EmailConfirmationCommand:IRequest<bool>
    {

        public string ConfirmationToken { get; set; }
        
    }

    internal class EmailConfirmationCommandHandler : IRequestHandler<EmailConfirmationCommand, bool>
    {
        private readonly IUserManager _userManager;
        private readonly IEmailManager _emailManager;
        private readonly ITokenManager _tokenManager;
        public EmailConfirmationCommandHandler(IUserManager userManager, IEmailManager emailManager, ITokenManager tokenManager)
        {
            _userManager = userManager;
            _emailManager = emailManager;
            _tokenManager = tokenManager;
        }
        public async Task<bool> Handle(EmailConfirmationCommand request, CancellationToken cancellationToken)
        {

            //   var userTokenProps = await _userManager.GetUserTokensSelectedProperties(request.ConfirmationToken, usertoken => new { usertoken.ConfirmationToken });
            //   string tokenFromDb = userTokenProps.ConfirmationToken;

            //   await _emailManager.IsEmailConfirmed(request.ConfirmationToken, tokenFromDb);
           
                var userTokenProps = await _userManager.GetIdentityUserActiveTokenConfirmationByToken(request.ConfirmationToken, ConfirmationTokenType.EMAIL_CONFIRMATION);
                //tratare exceptie 
                var userTokenPropsId = userTokenProps.UserId;
                var user = await _userManager.GetUserById(userTokenPropsId);
                var res=await _emailManager.IsEmailConfirmed(request.ConfirmationToken, userTokenProps, user);
                if(res==false)
                {
                    Console.WriteLine("Resending Email Confirmation");
                    var token = await _tokenManager.MailTokenConfig(user);
                    await _emailManager.SendEmailConfirmation(user, token);
                }
                
              
           
              

            return true;
        }
    }
}

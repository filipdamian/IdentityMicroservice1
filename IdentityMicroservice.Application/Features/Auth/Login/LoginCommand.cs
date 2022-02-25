using IdentityMicroservice.Application.Common.Exceptions;
using IdentityMicroservice.Application.Common.Interfaces;
using IdentityMicroservice.Application.ViewModels.External.Auth;
using IdentityMicroservice.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityMicroservice.Application.Features.Auth.Login
{
    public class LoginCommand : IRequest<TokenWrapper>
    {
        public string UniqueIdentifier { get; set; }
        public string Password { get; set; }
    }

    internal class LoginCommandHandler : IRequestHandler<LoginCommand, TokenWrapper>
    {
        private readonly IUserManager _userManager;
        private readonly IHashAlgo _hashAlgo;
        private readonly ITokenManager _tokenManager;
        public LoginCommandHandler(IUserManager userManager, IHashAlgo hashAlgo, ITokenManager tokenManager)
        {
            _userManager = userManager;
            _hashAlgo = hashAlgo;
            _tokenManager = tokenManager;
        }

        public async Task<TokenWrapper> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            //mut in appsettings
            const int maxLoginAttempts = 5;


            var userProps = await  _userManager.GetUserSelectedProperties(
                request.UniqueIdentifier,
                user => new { user.Id, user.EmailConfirmed, user.PasswordHash, user.LockoutEnabled, user.LockoutEnd });
            
  
            if (userProps == null)
            {
                string message = $"User with username or password = {request.UniqueIdentifier}  was not found";
                throw new NotFoundException(nameof(IdentityUser), message);
            }

            if (userProps.EmailConfirmed == false)
            {
                throw new EmailConfirmationException("Email not confirmed");
            }

            if (userProps.LockoutEnabled == true && userProps.LockoutEnd > DateTime.Now)
            {
                throw new AccountStillLockedException("Account still locked!");
            }
           

            string initialsalt=userProps.PasswordHash.Split('.')[1];
            bool isPasswordVerified = _hashAlgo.IsPasswordVerified(userProps.PasswordHash, initialsalt, request.Password);


            var user = await _userManager.GetUserById(userProps.Id);

            if (userProps.LockoutEnabled == true && userProps.LockoutEnd < DateTime.Now)
            {
                user.LockoutEnabled = false;
                user.LockoutEnd = null;
            }


            if (isPasswordVerified==false)
            {

                if (user.NumberOfFailLoginAttempts >= maxLoginAttempts)
                {
                    user.LockoutEnabled = true;
                    var timeLocked = DateTime.Now.AddMinutes(59); //nr minute in appsettings 
                    user.LockoutEnd = timeLocked;
                    await _userManager.updateUser(user);
                    throw new ExceededMaximumAmountOfLoginAttemptsException($"Exceeded maximum amount of login attemtps. {user.LockoutEnd} minutes left");
                }
                


                user.NumberOfFailLoginAttempts += 1;
                await _userManager.updateUser(user);
                throw new IncorrectPasswordException("Wrong Password");
            }
            else
            {
                //get identityusertokenbyuserid
                //if date.now< obj.expiredate: login else: generete new login and refresh token
                user.NumberOfFailLoginAttempts = 0;
                await _userManager.updateUser(user);
                var result = _userManager.Login(request);
                return await result;

            }
        }
    }
}

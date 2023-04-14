using IdentityMicroservice.Application.Common.Interfaces;
using IdentityMicroservice.Application.Common.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using IdentityMicroservice.Domain.Entities;
using IdentityMicroservice.Domain.Enums;

namespace IdentityMicroservice.Application.ViewModels.AppInternal
{
    public class RegisterCommand : IRequest<bool>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumberCountryPrefix { get; set; }
    }
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, bool>
    {
        private readonly IUserManager _userManager;
        private readonly IEmailManager _emailManager;
        private readonly ITokenManager _tokenManager;
        public RegisterCommandHandler(IUserManager userManager, IEmailManager emailManager, ITokenManager tokenManager)
        {
            _userManager = userManager;
            _emailManager = emailManager;
            _tokenManager = tokenManager;
        }
        public async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            bool ok = false;
            var userProps = await _userManager.GetUserSelectedProperties(request.Email, user => new { user.Id, user.Email }, cancellationToken);
            if (userProps != null)
            {

                string message = $"Username={request.Email} already registered";
                throw new UserAlreadyRegisteredException(nameof(IdentityUser), message);
            }
            else
            {
                var emailValidation = _emailManager.IsValidEmail(request.Email);
                if (emailValidation == true)
                {
                    var result = await _userManager.Register(request);
                    var tokenFilterAndCreate = await _tokenManager.CreateConfirmationToken(result.Id, ConfirmationTokenType.EMAIL_CONFIRMATION);
                    await _emailManager.SendEmailConfirmation(result, tokenFilterAndCreate);
                    ok = true;
                }
                else
                {
                    string message = $"Username={request.Email} already registered";
                    throw new UserAlreadyRegisteredException(nameof(IdentityUser), message);
                }

            }
            return ok;
        }
    }
}

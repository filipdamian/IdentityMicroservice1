using IdentityMicroservice.Application.Common.Exceptions;
using IdentityMicroservice.Application.Common.Interfaces;
using IdentityMicroservice.Domain.Enums;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityMicroservice.Application.Features.Auth.PasswordRecovery
{
    public class UpdateUserPasswordCommand:IRequest<bool>
    {
        public string PasswordToken { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmedNewPassword { get; set; }

    }
    internal class UpdateUserPasswordCommandHandler : IRequestHandler<UpdateUserPasswordCommand, bool>
    {
        private readonly IUserManager _userManager;
        private readonly IEmailManager _emailManager;
        private readonly ITokenManager _tokenManager;
        public UpdateUserPasswordCommandHandler(IUserManager userManager, IEmailManager emailManager, ITokenManager tokenManager)
        {
            _userManager = userManager; _emailManager = emailManager;
            _tokenManager = tokenManager;
        }
        public async Task<bool> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserByIdentityUserTokenConfirmation(request.PasswordToken);
          
            if (user != null)
            {
                var userId = user.Id;
                var tokenobj = await _tokenManager.GetUserActiveConfirmationToken(userId, request.PasswordToken, ConfirmationTokenType.RESET_PASSWORD);
                if (tokenobj != null)
                {
                    if (request.NewPassword == request.ConfirmedNewPassword)
                    {
                        var result = await _userManager.UpdateUserPassword(user, request.NewPassword);
                        if (result != null)
                        {
                            await _tokenManager.MarkRecoveryTokenAsUsed(tokenobj);
                            return true;
                        }
                    }
                    else
                    {
                        throw new IncorrectPasswordException("passwords are not matching!");
                    }

                }
                else
                {
                    throw new PasswordRecoveryTokenAlreadyUsedException("token already used");
                }
            }
            else
            {
                throw new UserNotFoundException("failed at finding the user. please try again later or register.");
            }
            return false;
        }
    }
}

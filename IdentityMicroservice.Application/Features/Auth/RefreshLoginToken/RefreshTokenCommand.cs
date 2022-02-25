using IdentityMicroservice.Application.Common.Exceptions;
using IdentityMicroservice.Application.Common.Interfaces;
using IdentityMicroservice.Application.ViewModels.External.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityMicroservice.Application.Features.Auth.RefreshLoginToken
{
    public class RefreshTokenCommand: IRequest<bool>
    {
        public string LoginToken { get; set; }
        public string RefreshLoginToken { get; set; }

    }
    internal class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, bool>
    {
        private readonly IUserManager _userManager;
        

        public RefreshTokenCommandHandler(IUserManager usermanager) { _userManager = usermanager; }

        public async Task<bool> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var IdentityUserTokenObj = await _userManager.GetUserTokenByRefreshToken(request.RefreshLoginToken);
            if(IdentityUserTokenObj==null)
            {
                throw new WasNotAbleToRefreshTokenException("");
            }
            // verific daca user este activ apoi fac refreshul de token (dar trebuie setat undeva la login ca user.isactive=true;)
            var IdentityUserObj = await _userManager.GetUserById(IdentityUserTokenObj.UserId);
            if (IdentityUserObj == null)
            {
                throw new SessionExpiredException("Session Expired. Please login again");
            }
            if (IdentityUserObj.IsActive == true)
            {
                if (IdentityUserTokenObj.ExpirationDate > DateTime.Now)
                {
                    IdentityUserTokenObj.ExpirationDate = DateTime.Now.AddMinutes(15);
                    IdentityUserObj.IsActive = false;
                    var response= await _userManager.saveAsync();
                }
            }
           
            //acum dupa ce am facut refreshul, presupun ca setez user.isdelted=true ? iar apoi in alt httpget daca se acceseaza endpointul in timpul in care tokenu
            //nu e expirat setez inapoi isactive=true?

            throw new NotImplementedException();
        }
    }
}

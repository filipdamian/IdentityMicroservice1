using IdentityMicroservice.Application.Common.Configurations;
using IdentityMicroservice.Application.Common.Interfaces;
using IdentityMicroservice.Application.Common.Interfaces.HttpRequests;
using IdentityMicroservice.Application.ViewModels.External.Auth;
using MediatR;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityMicroservice.Application.Features.Auth.Google
{
    public class GoogleLoginApiCommand : IRequest<TokenWrapper>
    {
        public string AccessToken { get; set; }
    }

    public class GoogleLoginApiCommandHandler : IRequestHandler<GoogleLoginApiCommand, TokenWrapper>
    {
        private readonly IUserManager _userManager;
        private readonly IHashAlgo _hashAlgo;
        private readonly ITokenManager _tokenManager;
        private readonly LoginTokenConfig _loginTokenConfig;
        private readonly IGoogleApiLoginRequestService _googleApiLoginRequestService;

        public GoogleLoginApiCommandHandler(IUserManager userManager, ITokenManager tokenManager, IHashAlgo hashAlgo, LoginTokenConfig loginTokenConfig, IGoogleApiLoginRequestService googleApiLoginRequestService)
        {
            _userManager = userManager;
            _tokenManager = tokenManager;
            _hashAlgo = hashAlgo;
            _loginTokenConfig = loginTokenConfig;
            _googleApiLoginRequestService = googleApiLoginRequestService;
        }

        public async Task<TokenWrapper> Handle(GoogleLoginApiCommand request, CancellationToken cancellationToken)
        {
            var res = await _googleApiLoginRequestService.GetAsync(request.AccessToken, cancellationToken);

            if (res.IsSuccessStatusCode)
            {
                var googleLogin = JObject.Parse(await res.Content.ReadAsStringAsync(cancellationToken)).ToObject<GoogleLoginResponseModel>();

                var userProps = await _userManager.GetUserSelectedProperties(googleLogin.Email, user => new { user.Id, user.Email }, cancellationToken);
                //if user is already in db with the email that was retrieved from the api, then generate token
                if (userProps != null)
                {
                    var result = await _userManager.Login(googleLogin.Email);
                    return result;
                }
                //otherwise add it to db and generate token

                var registerGoogleUser = await _userManager.RegisterGoogleUser(googleLogin);

                return registerGoogleUser;

            }
            throw new System.Exception("bad attempt");
        }
    }
}

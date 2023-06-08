using IdentityMicroservice.Application.Common.Configurations;
using IdentityMicroservice.Application.Common.Interfaces;
using IdentityMicroservice.Application.Common.Interfaces.HttpRequests;
using IdentityMicroservice.Application.ViewModels.External.Auth;
using MediatR;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityMicroservice.Application.Features.Auth.Facebook
{
    public class FacebookLoginApiCommand : IRequest<TokenWrapper>
    {
        public string AccessToken { get; set; }
    }

    public class FacebookLoginApiCommandHandler : IRequestHandler<FacebookLoginApiCommand, TokenWrapper>
    {
        private readonly IUserManager _userManager;
        private readonly IHashAlgo _hashAlgo;
        private readonly ITokenManager _tokenManager;
        private readonly LoginTokenConfig _loginTokenConfig;
        private readonly IFacebookApiLoginRequestService _facebookApiLoginRequestService;

        public FacebookLoginApiCommandHandler(IFacebookApiLoginRequestService facebookApiLoginRequestService, IUserManager userManager, IHashAlgo hashAlgo, ITokenManager tokenManager, LoginTokenConfig loginTokenConfig)
        {
            _facebookApiLoginRequestService = facebookApiLoginRequestService;
            _userManager = userManager;
            _hashAlgo = hashAlgo;
            _tokenManager = tokenManager;
            _loginTokenConfig = loginTokenConfig;
        }

        public async Task<TokenWrapper> Handle(FacebookLoginApiCommand request, CancellationToken cancellationToken)
        {
            var res = await _facebookApiLoginRequestService.GetAsync(request.AccessToken, cancellationToken);

            if (res.IsSuccessStatusCode)
            {
                var facebookLogin = JObject.Parse(await res.Content.ReadAsStringAsync(cancellationToken)).ToObject<FacebookLoginResponseModel>();

                var userProps = await _userManager.GetUserSelectedProperties(facebookLogin.Email, user => new { user.Id, user.Email }, cancellationToken);
                if (userProps != null)
                {
                    var result = _userManager.Login(facebookLogin.Email);
                    return await result;
                }
                else
                {
                    var registerFacebookUser = await _userManager.RegisterFacebookUser(facebookLogin);

                    return registerFacebookUser;
                }

            }
            throw new System.Exception("bad attempt");
        }
    }
}

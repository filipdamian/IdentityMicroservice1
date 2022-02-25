using IdentityMicroservice.Application.Features.Auth.Login;
using IdentityMicroservice.Application.ViewModels.AppInternal;
using IdentityMicroservice.Application.ViewModels.External.Auth;
using IdentityMicroservice.Domain.Entities;
using IdentityMicroservice.Domain.Enums;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityMicroservice.Application.Common.Interfaces
{
    public interface IUserManager
    {
        Task<T> GetUserSelectedProperties<T>(string uniqueIdentifier, Expression<Func<IdentityUser, T>> selector, CancellationToken cancellationToken = default);
        Task<T> GetUserTokensSelectedProperties<T>(string tokenValue, Expression<Func<IdentityUserTokenConfirmation, T>> selector, CancellationToken cancellationToken = default);
        Task<IdentityUserTokenConfirmation> GetIdentityUserActiveTokenConfirmationByToken(string token, ConfirmationTokenType confirmationTokenType);
        Task<IdentityUser> GetUserById(Guid id);
        Task<TokenWrapper> Login(LoginCommand loginCommand);
        Task<IdentityUser> Register(RegisterCommand registerCommand);
        Task<IdentityUserToken> GetUserTokenByRefreshToken(string refreshtoken);
        Task<IdentityUser> updateUser(IdentityUser user);
        Task<bool> saveAsync();

       
    }
}

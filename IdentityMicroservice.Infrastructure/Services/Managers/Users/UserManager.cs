﻿using IdentityMicroservice.Application.Common.Configurations;
using IdentityMicroservice.Application.Common.Exceptions;
using IdentityMicroservice.Application.Common.Interfaces;
using IdentityMicroservice.Application.Features.Auth.Login;
using IdentityMicroservice.Application.Features.PasswordHashing;
using IdentityMicroservice.Application.ViewModels.AppInternal;
using IdentityMicroservice.Application.ViewModels.External.Auth;
using IdentityMicroservice.Domain.Constants;
using IdentityMicroservice.Domain.Entities;
using IdentityMicroservice.Domain.Enums;
using IdentityMicroservice.Infrastructure.Persistence.DbContexts.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityMicroservice.Infrastructure.Services.Managers.Users
{
    public class UserManager : IUserManager
    {
        private readonly IHashAlgo _hashAlgo;
        private readonly IdentityDbContext _context;
        private readonly ITokenManager _tokenManager;
        private readonly SignInKeySetting _signInKeySetting;
        public UserManager(IHashAlgo hashAlgo, IdentityDbContext context, ITokenManager tokenManager, SignInKeySetting signInKeySetting)
        {
            _hashAlgo = hashAlgo;
            _context = context;
            _tokenManager = tokenManager;
            _signInKeySetting = signInKeySetting;
        }
        //imporv2
        public async Task<IdentityUserTokenConfirmation> GetIdentityUserActiveTokenConfirmationByToken(string token, ConfirmationTokenType confirmationTokenType)
        {
            //daca token ul este inca valid si daca este pentru email confirmation 
            var utc = await _context.IdentityUserTokenConfirmations
                .Where(prop =>
                    prop.ConfirmationToken.Equals(token) &&
                    prop.IsUsed == false &&
                    prop.ConfirmationTypeId == confirmationTokenType &&
                    prop.ExpireDate > DateTime.UtcNow)
                .SingleOrDefaultAsync();

            return utc;
        }
        //improv1
        public async Task<IdentityUser> GetUserById(Guid id)
        {
            var user = await _context.IdentityUsers.Where(u => u.Id.Equals(id)).SingleOrDefaultAsync();
            return user;
        }

        public async Task<IdentityUser> GetUserByEmail(string email)
        {
            var user = await _context.IdentityUsers.Where(u => u.Email.Equals(email)).SingleOrDefaultAsync();
            return user;
        }

        public Task<T> GetUserSelectedProperties<T>(string uniqueIdentifier, Expression<Func<IdentityUser, T>> selector, CancellationToken cancellationToken = default)
        {
            var selectedUserPropertiesObject = _context.IdentityUsers
              .AsNoTracking()
              .Where(u => u.Username.Equals(uniqueIdentifier) || u.Email.Equals(uniqueIdentifier))
              .Select(selector)
              .SingleOrDefaultAsync(cancellationToken);

            return selectedUserPropertiesObject;
        }

        public async Task<IdentityUserToken> GetUserTokenByRefreshToken(string refreshtoken)
        {
            ///where refreshtokentime is still valid/ where numberofrefreshes < maxallowedtokenrefresh?
            var userTokenObj = await _context.IdentityUserTokens.Where(ut => ut.RefreshTokenValue.Equals(refreshtoken)).SingleOrDefaultAsync();
            return userTokenObj;
        }

        public async Task<T> GetUserTokensSelectedProperties<T>(string tokenValue, Expression<Func<IdentityUserTokenConfirmation, T>> selector, CancellationToken cancellationToken = default)
        {
            var selectedUserTokenPropertiesObject = await _context.IdentityUserTokenConfirmations.AsNoTracking().Where(utc => utc.ConfirmationToken.Equals(tokenValue)).Select(selector).SingleOrDefaultAsync(cancellationToken);
            return selectedUserTokenPropertiesObject;
        }

        public async Task<TokenWrapper> Login(string UniqueIdentifier)
        {



            IdentityUser user = await _context.IdentityUsers.Where(u => u.Username.Equals(UniqueIdentifier) || u.Email.Equals(UniqueIdentifier)).SingleOrDefaultAsync();
            user = await _context.IdentityUsers.Include(u => u.IdentityUserRoles).ThenInclude(ur => ur.IdentityRole).Where(u => u.Id.Equals(user.Id)).SingleOrDefaultAsync();
            List<string> roles = user.IdentityUserRoles.Select(ur => ur.IdentityRole.Name).ToList();

            var newJti = Guid.NewGuid().ToString();
            var tokenHandler = new JwtSecurityTokenHandler();
            //usersecret
            var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_signInKeySetting.SecretSignInKeyForJwtToken));
            //int usedRefreshes = -1;
            var tokenresult = await _tokenManager.GenerateTokenAndRefreshToken(signinKey, user, roles, tokenHandler, newJti);
            //var refreshToken = _tokenManager.GenerateRefreshToken();


            //????

            TokenWrapper tokenwrapper = new TokenWrapper();
            tokenwrapper.Token = tokenresult.Item1;
            tokenwrapper.RefreshToken = tokenresult.Item2;


            return tokenwrapper;



        }

        public async Task<IdentityUser> Register(RegisterCommand registerCommand)
        {
            var user = await _context.IdentityUsers.Where(u => u.Email.Equals(registerCommand.Email)).SingleOrDefaultAsync();
            if (user != null)
            {
                string message = "Username=" + registerCommand.Email + " already registered";
                throw new UserAlreadyRegisteredException(nameof(IdentityUser), message);
            }
            else
            {
                var registerUser = new IdentityUser();
                registerUser.Id = Guid.NewGuid();
                registerUser.Email = registerCommand.Email;
                registerUser.PhoneNumber = registerCommand.PhoneNumber;
                registerUser.Username = registerCommand.FirstName + registerCommand.LastName;
                registerUser.PhoneNumberCountryPrefix = registerCommand.PhoneNumberCountryPrefix;
                string result = _hashAlgo.CalculateHashValueWithInput(registerCommand.Password);
                if (result != null)
                {
                    registerUser.PasswordHash = result;
                    _context.Set<IdentityUser>().Add(registerUser);


                    //  var id = await _context.IdentityUsers.Where(u => u.Email.Equals(registerCommand.Email)).Select(u => u.Id).SingleOrDefaultAsync();
                    var roleid = Guid.NewGuid();
                    _context.Set<IdentityRole>().Add(new IdentityRole(UserRoleType.Admin, roleid));
                    _context.Set<IdentityUserIdentityRole>().Add(new IdentityUserIdentityRole(registerUser.Id, roleid));
                    await _context.SaveChangesAsync();

                    return registerUser;
                }

            }

            return null;
        }
        public async Task<bool> saveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<IdentityUser> updateUser(IdentityUser user)
        {

            _context.Set<IdentityUser>().Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<IdentityUser> UpdateUserPassword(IdentityUser user, string password)
        {
            string updatedPassword = _hashAlgo.CalculateHashValueWithInput(password);
            if (updatedPassword != null)
            {
                user.PasswordHash = updatedPassword;
                await _context.SaveChangesAsync();

                return user;
            }
            return null;
        }

        public async Task<IdentityUser> GetUserByIdentityUserTokenConfirmation(string token)
        {
            var identityUserTokenConfirmationObj = await _context.IdentityUserTokenConfirmations.Where(utc => utc.ConfirmationToken.Equals(token) && utc.ConfirmationTypeId.Equals(ConfirmationTokenType.RESET_PASSWORD)).SingleOrDefaultAsync();
            var user = await _context.IdentityUsers.Where(u => u.Id.Equals(identityUserTokenConfirmationObj.UserId)).SingleOrDefaultAsync();
            return user;
        }

        public async Task<TokenWrapper> RegisterGoogleUser(GoogleLoginResponseModel googleLogin)
        {
            var user = new IdentityUser
            {
                Email = googleLogin.Email,
                EmailConfirmed = true,
                Username = googleLogin.Email,
                PasswordHash = "googleLogin",
                PhoneNumber = "googleLogin",
                PhoneNumberCountryPrefix="gglph",

            };

            _context.IdentityUsers.Add(user);
            await _context.SaveChangesAsync();

            var token = await Login(user.Email);

            return token;

        }

        public async Task<TokenWrapper> RegisterFacebookUser(FacebookLoginResponseModel facebookLogin)
        {
            var user = new IdentityUser
            {
                Email = facebookLogin.Email,
                EmailConfirmed = true,
                Username = facebookLogin.Name,
                PasswordHash = "facebookLogin",
                PhoneNumber = "facebookLogin",
                PhoneNumberCountryPrefix = "fbph"
            };

            _context.IdentityUsers.Add(user);
            await _context.SaveChangesAsync();

            var token = await Login(user.Email);

            return token;
        }
        /* public async Task<IdentityUserTokenConfirmation> GetPasswordRecoveryTokenByUser(IdentityUser user)
{
    var obj = await _context.IdentityUserTokenConfirmations.Where(utc => utc.UserId.Equals(user.Id)).FirstOrDefaultAsync();
    return obj;
}*/
    }
}

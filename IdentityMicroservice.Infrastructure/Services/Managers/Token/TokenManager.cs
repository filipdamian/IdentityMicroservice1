using IdentityMicroservice.Application.Common.Interfaces;
using IdentityMicroservice.Domain.Entities;
using IdentityMicroservice.Domain.Enums;
using IdentityMicroservice.Infrastructure.Persistence.DbContexts.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMicroservice.Infrastructure.Services.Managers.Token
{
    public class TokenManager : ITokenManager
    {
        private readonly IdentityDbContext _context;
        public TokenManager(IdentityDbContext context)
        {
            _context = context;
        }

       

        public async Task<Tuple<string,string>> TokenConfig(SymmetricSecurityKey signinKey, IdentityUser user, List<string> roles, JwtSecurityTokenHandler tokenHandler, string newJti)
        {
            
            var token = GenerateJwtToken(signinKey, user, roles, tokenHandler, newJti);
            string tokenStringValue = tokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            _context.Set<IdentityUserToken>().Add(new IdentityUserToken(user.Id, tokenStringValue,refreshToken, token.ValidFrom, token.ValidTo));
            
            await _context.SaveChangesAsync();
            var tuple = new Tuple<string, string>(tokenStringValue, refreshToken);
            return tuple;
        
        }
       
        private SecurityToken GenerateJwtToken(SymmetricSecurityKey signinKey, IdentityUser user, List<string> roles, JwtSecurityTokenHandler tokenHandler, string newJti)
        {
            var subject = new ClaimsIdentity(new Claim[] {
                     new Claim(ClaimTypes.Email,user.Email),
                     new Claim(ClaimTypes.Name,user.Username),
                     new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                     new Claim(JwtRegisteredClaimNames.Jti,newJti)
             });
             foreach (var role in roles)
             {
                 subject.AddClaim(new Claim(ClaimTypes.Role, role));
             }
             var tokenDescriptor = new SecurityTokenDescriptor
             {
                 Subject = subject,
                 Expires = DateTime.Now.AddDays(1),
                 SigningCredentials = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)

             };
             var token = tokenHandler.CreateToken(tokenDescriptor);
             return token;
        }
        public async Task<IdentityUserTokenConfirmation> MailTokenConfig(IdentityUser user)
        {
            var confirmationToken = new IdentityUserTokenConfirmation();
            confirmationToken.Id = Guid.NewGuid();
            confirmationToken.UserId = user.Id;
            confirmationToken.ConfirmationTypeId = ConfirmationTokenType.EMAIL_CONFIRMATION;
            confirmationToken.CreationDate = DateTime.Now;
            confirmationToken.ExpireDate = DateTime.Now.AddHours(12);
            confirmationToken.ConfirmationToken = "abc"; //schimb to guid.newguid().tostring() ?
            _context.Set<IdentityUserTokenConfirmation>().Add(confirmationToken);
            await _context.SaveChangesAsync();
            return confirmationToken;

        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using(var rng=RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}

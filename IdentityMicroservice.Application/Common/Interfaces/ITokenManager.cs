using IdentityMicroservice.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMicroservice.Application.Common.Interfaces
{
    public interface ITokenManager
    {
        Task<Tuple<string, string>> TokenConfig(SymmetricSecurityKey signinKey,IdentityUser user, List<string>roles,JwtSecurityTokenHandler tokenHandler,string newJti);
        Task<IdentityUserTokenConfirmation> MailTokenConfig(IdentityUser user);
        string GenerateRefreshToken();
    }
}

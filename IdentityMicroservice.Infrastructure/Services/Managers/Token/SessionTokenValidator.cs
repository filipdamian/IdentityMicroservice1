using IdentityMicroservice.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice1.Services
{
    public class SessionTokenValidator 
    {
        
        public static async Task ValidateSessionToken(TokenValidatedContext context)
        {
           /* var repository = context.HttpContext.RequestServices.GetRequiredService<IRepositoryWrapper>();
            if (context.Principal.HasClaim(c => c.Type.Equals(JwtRegisteredClaimNames.Jti))) {

                var jti = context.Principal.Claims.SingleOrDefault(c => c.Type.Equals(JwtRegisteredClaimNames.Jti)).Value;

                var tokeninDb = await repository.SessionToken.GetByJti(jti);
                if(tokeninDb!=null && tokeninDb.ExpirationDate>DateTime.Now)
                {
                    return;
                }
            }
            context.Fail("");*/
        }
    }
}

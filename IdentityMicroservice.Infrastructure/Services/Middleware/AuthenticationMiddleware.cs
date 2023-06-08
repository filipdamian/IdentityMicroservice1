using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityMicroservice.Infrastructure.Services.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (authorizationHeader != null)
            {
                var jwt = authorizationHeader.Split(' ')[1];
                var handler = new JwtSecurityTokenHandler();

                var token = handler.ReadJwtToken(jwt);
                var userId = token.Claims.ToList()[2].ToString().Split(" ")[1];

                context.Items["UserId"] = userId;

            }

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }
    }
}

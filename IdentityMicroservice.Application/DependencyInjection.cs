using IdentityMicroservice.Application.Common.Configurations;
using IdentityMicroservice.Application.ViewModels.External.Email;
using IdentityMicroservice.Domain.Constants;
using IdentityMicroservice.Infrastructure.Services.HttpClients.EmailSender;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Reflection;
using System.Text;

namespace IdentityMicroservice.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddAuthentications(configuration);
            services.AddAuthorizations(configuration);
           
            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }

     
        private static IServiceCollection AddAuthorizations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(UserRoleType.Admin, policy => policy.RequireRole(UserRoleType.Admin));
                options.AddPolicy(UserRoleType.User, policy => policy.RequireRole(UserRoleType.User));

            });
            return services;
        }
        private static IServiceCollection AddAuthentications(this IServiceCollection services, IConfiguration configuration)
        {
            var hashingOptions = configuration.GetSection(SecretSettings.NAME).Get<SecretSettings>();
            Console.WriteLine(hashingOptions.HashingKey);
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(hashingOptions.HashingKey)),
                    ValidateIssuerSigningKey = true

                };
                options.Events = new JwtBearerEvents()
                {
                    // OnTokenValidated = Services.SessionTokenValidator.ValidateSessionToken
                };

            }
            );
            return services;
        }
    }
}

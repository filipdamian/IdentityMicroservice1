using IdentityMicroservice.Application.Common.Configurations;
using IdentityMicroservice.Application.Data.Constants;
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
using System.Threading.Tasks;

namespace IdentityMicroservice.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddAuthentications(configuration);
            services.AddAuthorizations(configuration);
            services.AddCors(configuration);
           
            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }

        private static IServiceCollection AddCors(this IServiceCollection services, IConfiguration configuration)
        {
            string _policyName = "CorsPolicy";
            services.AddCors(opt =>
            {
                opt.AddPolicy(name: _policyName, builder =>
                {
                    builder
                        .WithOrigins("https://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                        //.WithExposedHeaders(CustomHeader.XTokenExpired);
                });
            });
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
            var secretOptions = configuration.GetSection(SignInKeySetting.NAME).Get<SignInKeySetting>();
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretOptions.SecretSignInKeyForJwtToken)),
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero

                };
                options.Events = new JwtBearerEvents()
                {

                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add(CustomHeader.XTokenExpired, "True");
                        }
                        return Task.CompletedTask;
                    }
                };

            }
            );
            return services;
        }
    }
}

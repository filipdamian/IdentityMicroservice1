﻿using IdentityMicroservice.Application.Common.Interfaces;
using IdentityMicroservice.Infrastructure.Persistence.DbContexts.Identity;
using IdentityMicroservice.Infrastructure.Services.Managers.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IdentityMicroservice.Application.Common.Configurations;
using System;
using IdentityMicroservice.Application.Features.PasswordHashing;
using IdentityMicroservice.Application.ViewModels.External.Email;
using IdentityMicroservice.Infrastructure.Services.HttpClients.EmailSender;
using IdentityMicroservice.Infrastructure.Services.Managers.Token;
using IdentityMicroservice.Infrastructure.Services.Managers.Email;

namespace IdentityMicroservice.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
      
            services.AddDbContexts(configuration);
            services.AddScoped<IHashAlgo, HashAlgo>();
            services.AddScoped<ITokenManager, TokenManager>();

            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IEmailManager, EmailManager>();
            services.AddEmailConfiguration(configuration);
            
            // services.AddScoped<SeedDb>();
            services.AddRazorPages();

            return services;
        }
        private static IServiceCollection AddEmailConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var emailConfig = configuration.GetSection(EmailSenderSetting.NAME).Get<EmailSenderSetting>();
            // Console.WriteLine(emailConfig.Password);
            services.AddSingleton(emailConfig);
            
            return services;
        }
        private static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionOptions = configuration.GetSection(ConnectionStringSetting.NAME).Get<ConnectionStringSetting>();
           
            if(connectionOptions.ConnectionStringConfigs.TryGetValue(DatabaseIdentifier.IdentityDatabase, out var identityDbConfig) == false)
            {
                throw new ArgumentException($"{nameof(DatabaseIdentifier.IdentityDatabase)} was not found in the dbConfig!");
            }
           
            services.AddDbContext<IdentityDbContext>(options =>
            {
                options.UseSqlServer(identityDbConfig.ConnectionString, configOption =>
                {
                    configOption.CommandTimeout(identityDbConfig.TimeoutSeconds);
                });
            });

            return services;
        }



  









    }
}

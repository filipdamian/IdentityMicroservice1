using IdentityMicroservice.Application.Common.Interfaces;
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
using IdentityMicroservice.Infrastructure.Services.Managers.WebScraper;
using IdentityMicroservice.Infrastructure.Services.Managers.CnnModel;
using Hangfire;
using Hangfire.SqlServer;
using IdentityMicroservice.Infrastructure.Services.Managers.FishTanks;
using IdentityMicroservice.Infrastructure.Services.Seeder;
using IdentityMicroservice.Application.Common.Interfaces.HttpRequests;
using IdentityMicroservice.Infrastructure.Services.HttpClients;

namespace IdentityMicroservice.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContexts(configuration);
            services.AddHangfireConfig(configuration);
            services.AddScoped<IHashAlgo, HashAlgo>();
            services.AddSignInKeyConfiguration(configuration);
            //services.CEVA(configuration);
            services.AddRefreshTokenConfiguration(configuration);
            services.AddWebScraperConfiguration(configuration);
            services.Configure<WebScraperOptions>(configuration.GetSection(WebScraperOptions.NAME));
            services.AddLoginTokenConfiguration(configuration);
            services.AddScoped<ITokenManager, TokenManager>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IEmailManager, EmailManager>();
            services.AddScoped<IWebScraperManager, WebScraperManager>();
            services.AddScoped<IFishTankManager, FishTankManager>();
            services.AddScoped<FishSpecSeeder>();
            services.AddEmailConfiguration(configuration);
            services.AddSingleton<IDeepNeuralNetworkModel, DeepNeuralNetworkModel>();
            services.AddScoped<IFacebookApiLoginRequestService, FacebookApiLoginRequestService>();
            services.AddScoped<IGoogleApiLoginRequestService, GoogleApiLoginRequestService>();
            services.FishSpecSeeder(configuration);

            services.AddRazorPages();

            return services;
        }
        private static IServiceCollection CEVA(this IServiceCollection services, IConfiguration configuration)
        {
            DeepNeuralNetworkModel model = new DeepNeuralNetworkModel();
            model.LoadImagesInMemory();
            return services;
        }
        private static IServiceCollection AddWebScraperConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var webScraperConfig = configuration.GetSection(WebScraperOptions.NAME).Get<WebScraperOptions>();
            Console.WriteLine(webScraperConfig.ChromeDriverOptions.LocalPath);
            services.AddSingleton(webScraperConfig);
            return services;

        }

        private static IServiceCollection AddSignInKeyConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var signinConfig = configuration.GetSection(SignInKeySetting.NAME).Get<SignInKeySetting>();
            //Console.WriteLine(signinConfig.SecretSignInKeyForJwtToken);
            services.AddSingleton(signinConfig);
            return services;

        }
        private static IServiceCollection AddLoginTokenConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var logintokenConfig = configuration.GetSection(LoginTokenSetting.NAME).Get<LoginTokenSetting>();
            if (logintokenConfig.LoginTokenConfigs.TryGetValue(LoginTokenIdentifier.LoginToken, out var loginTokenConfig) == false)
            {

                throw new Exception();
            }
            // Console.WriteLine(loginTokenConfig.Minutes);
            services.AddSingleton(loginTokenConfig);
            return services;

        }
        private static IServiceCollection AddEmailConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var emailConfig = configuration.GetSection(EmailSenderSetting.NAME).Get<EmailSenderSetting>();

            // Console.WriteLine(emailConfig.Password);
            services.AddSingleton(emailConfig);

            return services;
        }
        private static IServiceCollection AddRefreshTokenConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var reftokenConfig = configuration.GetSection(RefreshTokenSetting.NAME).Get<RefreshTokenSetting>();
            if (reftokenConfig.RefreshTokenConfigs.TryGetValue(RefreshTokenIdentifier.RefreshToken, out var refreshTokenConfig) == false)
            {

                throw new Exception();
            }
            //Console.WriteLine(refreshTokenConfig.Issuer);
            services.AddSingleton(refreshTokenConfig);
            return services;
        }
        private static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionOptions = configuration.GetSection(ConnectionStringSetting.NAME).Get<ConnectionStringSetting>();


            if (connectionOptions.ConnectionStringConfigs.TryGetValue(DatabaseIdentifier.IdentityDatabase, out var identityDbConfig) == false)
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

        private static IServiceCollection FishSpecSeeder(this IServiceCollection services, IConfiguration configuration)
        {
            var dbSettings = configuration.GetSection(nameof(DbSettings)).Get<DbSettings>();
            Console.WriteLine(dbSettings.InitContainer);
            if (dbSettings.InitContainer)
            {
                using (var serviceProvider = services.BuildServiceProvider())
                using (var scope = serviceProvider.CreateScope())
                {
                    var seeder = scope.ServiceProvider.GetRequiredService<FishSpecSeeder>();
                    seeder.SeedFishSpecs();
                }
            }
          
            return services;
        }

        private static IServiceCollection AddHangfireConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionOptions = configuration.GetSection(ConnectionStringSetting.NAME).Get<ConnectionStringSetting>();


            if (connectionOptions.ConnectionStringConfigs.TryGetValue(DatabaseIdentifier.IdentityDatabase, out var identityDbConfig) == false)
            {

                throw new ArgumentException($"{nameof(DatabaseIdentifier.IdentityDatabase)} was not found in the dbConfig!");
            }

            var connectionString = identityDbConfig.ConnectionString;

            GlobalConfiguration.Configuration.UseDefaultActivator();

            services.AddHangfire(configuration =>
            {
                configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170);
                configuration.UseSimpleAssemblyNameTypeSerializer();
                configuration.UseRecommendedSerializerSettings();

                configuration.UseStorage(new SqlServerStorage(connectionString, new SqlServerStorageOptions()
                {
                    QueuePollInterval = TimeSpan.FromSeconds(2),
                }));
            });

            services.AddHangfireServer();

            return services;
        }
    }
}

using IdentityMicroservice.Application.Common.Interfaces;
using IdentityMicroservice.Application.ViewModels.AppInternal;
using IdentityMicroservice.Infrastructure.Persistence.DbContexts.Identity;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using IdentityMicroservice.Infrastructure;
using Microsoft.Extensions.Configuration;
using IdentityMicroservice.Application.Features.Auth.PasswordRecovery;
using FluentAssertions;
using IdentityMicroservice.Domain.Entities;

namespace Identity.Tests.UnitTests
{
    public class AccountUnitTests
    {
        private WebApplication _app;
        private readonly Mock<IMediator> _mediator;
        private readonly IUserManager _userManager;
        private readonly Mock<IEmailManager> _emailManager;
        private readonly ITokenManager _tokenManager;
        public IdentityDbContext _context;

        public AccountUnitTests()
        {
            var builder = WebApplication.CreateBuilder();

            AddConfiguration(builder);

            var services = builder.Services;

            _mediator = new Mock<IMediator>();

            _emailManager = new Mock<IEmailManager>();

            _emailManager.CallBase = true;

            _emailManager.Setup(x => x.SendEmailConfirmation(It.IsAny<IdentityUser>(), It.IsAny<IdentityUserTokenConfirmation>())).Returns(Task.CompletedTask);
            _emailManager.Setup(x => x.IsValidEmail(It.IsAny<string>())).Returns(true);

            services.Remove<DbContextOptions<IdentityDbContext>>();

            var dbName = $"{Guid.NewGuid()}";

            services.AddDbContext<IdentityDbContext>(options => options
             .UseInMemoryDatabase(databaseName: dbName)
             .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)));

            services.AddInfrastructure(builder.Configuration);

            _app = builder.Build();

            _context = _app.Services.GetRequiredService<IdentityDbContext>();

            _userManager = _app.Services.GetRequiredService<IUserManager>();

            _tokenManager = _app.Services.GetRequiredService<ITokenManager>();

            AddDataToContext(_context, _app.Services);

        }
        //Structural testing
        //Statement coverage (fiecare instructiune este parcursa macar odata)
        [Fact]
        public async Task Test1AccountRegisterAlreadyRegisted()
        {
            var request = new RegisterCommand()
            {
                FirstName = "Filip",
                LastName = "Constantin",
                Email = "filipdamian29@gmail.com",
                Password = "Parola123!",
                PhoneNumber = "0726139370",
                PhoneNumberCountryPrefix = "+40"
            };

            var user = _context.IdentityUsers.ToList();
            _mediator.Setup(m => m.Send(request, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var handler = new RegisterCommandHandler(_userManager, _emailManager.Object, _tokenManager);
            var result = handler.Handle(request, CancellationToken.None);

            //fluent assertion
            result.Exception?.InnerException?.Message.Should().Be("Username =filipdamian29@gmail.com already registered");
        }

        [Fact]
        public async Task Test1AccountRegisterInvalidEmail()
        {
            var request = new RegisterCommand()
            {
                FirstName = "Filip",
                LastName = "Constantin",
                Email = "`+%*@filipdamian29!@gmail.com",
                Password = "Parola123!",
                PhoneNumber = "0726139370",
                PhoneNumberCountryPrefix = "+40"
            };

            var user = _context.IdentityUsers.ToList();
            _mediator.Setup(m => m.Send(request, It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _emailManager.Setup(x => x.IsValidEmail(It.IsAny<string>())).Returns(false);

            var handler = new RegisterCommandHandler(_userManager, _emailManager.Object, _tokenManager);
            var result = handler.Handle(request, CancellationToken.None);

            //fluent assertion
            result.Exception?.InnerException?.Message.Should().Be("Username=`+%*@filipdamian29!@gmail.com already registered");
        }

        //[Fact]
        //public async Task Test1AccountRegister()
        //{
        //    var request = new RegisterCommand()
        //    {
        //        FirstName = "Maria",
        //        LastName = "Popescu",
        //        Email = "mariapopescu@gmail.com",
        //        Password = "Parola123!",
        //        PhoneNumber = "0786356218",
        //        PhoneNumberCountryPrefix = "+40"
        //    };

        //    _mediator.Setup(m => m.Send(request, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        //    var handler = new RegisterCommandHandler(_userManager, _emailManager.Object, _tokenManager);
        //    var result = handler.Handle(request, CancellationToken.None);

        //    var user = _context.IdentityUsers.ToList();

        //    //fluent assertion
        //    user.Should().HaveCount(2);

        //    result.Result.Should().Be(true);
        //}

        //Structural testing
        //Condition coverage (fiecare conditie individuala dintr-o decizie sa ia atat valoarea adevarat cat si valoarea fals)
        [Fact]
        public async Task Test2PasswordRecovery()
        {
            var request = new PasswordRecoveryCommand()
            {
                Email = "filipdamian29@gmail.com",
            };

            var user = _context.IdentityUsers.ToList();

            _mediator.Setup(m => m.Send(request, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            _emailManager.Setup(x => x.SendPasswordRecoveryEmail(It.IsAny<IdentityUser>(), It.IsAny<IdentityUserTokenConfirmation>())).ReturnsAsync(true);

            var handler = new PasswordRecoveryCommandHandler(_emailManager.Object, _userManager, _tokenManager);
            var result = handler.Handle(request, CancellationToken.None);

            result.Result.Should().Be(true);
        }

        [Fact]
        public async Task Test2PasswordRecoveryEmailSendingError()
        {
            var request = new PasswordRecoveryCommand()
            {
                Email = "filipdamian29@gmail.com",
            };

            var user = _context.IdentityUsers.ToList();

            _mediator.Setup(m => m.Send(request, It.IsAny<CancellationToken>())).ReturnsAsync(true);


            var handler = new PasswordRecoveryCommandHandler(_emailManager.Object, _userManager, _tokenManager);
            var result = handler.Handle(request, CancellationToken.None);

            result.Exception.InnerException.Message.Should().Be("email could not be sent ");
        }

        [Fact]
        public async Task Test2PasswordRecoveryInvalidEmail()
        {
            var request = new PasswordRecoveryCommand()
            {
                Email = "catalincata@gmail.com",
            };

            var user = _context.IdentityUsers.ToList();

            _mediator.Setup(m => m.Send(request, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            _emailManager.Setup(x => x.SendPasswordRecoveryEmail(It.IsAny<IdentityUser>(), It.IsAny<IdentityUserTokenConfirmation>())).ReturnsAsync(true);

            var handler = new PasswordRecoveryCommandHandler(_emailManager.Object, _userManager, _tokenManager);
            var result = handler.Handle(request, CancellationToken.None);

            result.Exception.InnerException.Message.Should().Be("User not found. Please register");
        }

        [Fact]
        public async Task Test2PasswordRecoveryIncompleteOnboarding()
        {
            _context.IdentityUsers.Add(new()
            {
                Id = Guid.NewGuid(),
                Username = "MariusM",
                Email = "MariusM@gmail.com",
                EmailConfirmed = false,
                LockoutEnabled = false,
                LockoutEnd = null,
                PhoneNumberCountryPrefix = "+40",
                PhoneNumber = "0726139370",
                PhoneNumberConfirmed = true,
                PasswordHash = "10000.ksU3Fz4AemMS/mULgFnuPw==.3uqcBq8RHfkDWLb5KPERE+4YbGZcIEXGxgbt6EM+4Gg=",
                TwoFactorEnabled = false,
                CreatedAt = DateTime.UtcNow,
                NumberOfFailLoginAttempts = 0,
                IsActive = false,
                IsDeleted = false
            });

            _context.SaveChanges();

            var request = new PasswordRecoveryCommand()
            {
                Email = "MariusM@gmail.com",
            };

            var user = _context.IdentityUsers.ToList();

            _mediator.Setup(m => m.Send(request, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            _emailManager.Setup(x => x.SendPasswordRecoveryEmail(It.IsAny<IdentityUser>(), It.IsAny<IdentityUserTokenConfirmation>())).ReturnsAsync(true);

            var handler = new PasswordRecoveryCommandHandler(_emailManager.Object, _userManager, _tokenManager);
            var result = handler.Handle(request, CancellationToken.None);

            result.Exception.InnerException.Message.Should().Be("please confirm your email first");
        }

        #region Configuration
        private void AddConfiguration(WebApplicationBuilder builder)
        {
            builder.Configuration
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "JWTToken:RefreshTokenConfigs:RefreshToken:NumberOfRefreshes", "3"},
                    { "JWTToken:RefreshTokenConfigs:RefreshToken:TimeLeftUntilRefreshTokenExpiresAfterTokenAlreadyExpired", "5"},
                    { "JWTToken:RefreshTokenConfigs:RefreshToken:Issuer", "https://localhost:5001/"},
                    { "JWTToken:RefreshTokenConfigs:RefreshToken:Audience", "https://localhost:5001/"},
                    { "JWTToken:LoginTokenConfigs:LoginToken:Minutes", "30"},
                    { "JWTToken:LoginTokenConfigs:LoginToken:Hours", "12"},
                    { "JWTToken:LoginTokenConfigs:LoginToken:Days", "1"},
                    { "WebScraperOptions:ChromeDriverOptions:LocalPath", "C:\\Users\\filip\\source\\repos\\IdentityMicroservice1"},
                    { "LinkedInCredentialsOptions:Username", "filip.constantin@softbinator.com"},
                    { "LinkedInCredentialsOptions:Password", "Parola123!"},
                    { "ConnectionStrings:ConnectionStringConfigs:IdentityDatabase:ConnectionString", "Data Source=LAPTOP-4RQNI2BA\\SQLEXPRESS;Initial Catalog=IdentityDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"},
                    { "ConnectionStrings:ConnectionStringConfigs:IdentityDatabase:TimeoutSeconds", "30"},
                    { "Hashing:HashingKey", "123456789"},
                    { "EmailSender:From", "coralisromaniab@gmail.com"},
                    { "EmailSender:SmtpServer", "smtp.gmail.com"},
                    { "EmailSender:Port", "465"},
                    { "EmailSender:Username", "coralisromaniab@gmail.com"},
                    { "EmailSender:Password", "vmatvwqtgmunxehp"},
                    { "SigninKey:SecretSignInKeyForJwtToken", "guidlikesecretkey"},
                });
        }
        #endregion

        #region Populate Context
        private void AddDataToContext(IdentityDbContext _context, IServiceProvider provider)
        {
            _context.IdentityUsers.Add(new()
            {
                Id = Guid.NewGuid(),
                Username = "FilipConstantin",
                Email = "filipdamian29@gmail.com",
                EmailConfirmed = true,
                LockoutEnabled = false,
                LockoutEnd = null,
                PhoneNumberCountryPrefix = "+40",
                PhoneNumber = "0726139370",
                PhoneNumberConfirmed = true,
                PasswordHash = "10000.ksU3Fz4AemMS/mULgFnuPw==.3uqcBq8RHfkDWLb5KPERE+4YbGZcIEXGxgbt6EM+4Gg=",
                TwoFactorEnabled = false,
                CreatedAt = DateTime.UtcNow,
                NumberOfFailLoginAttempts = 0,
                IsActive = false,
                IsDeleted = false
            });

            _context.SaveChanges();
        }
        #endregion
    }
}
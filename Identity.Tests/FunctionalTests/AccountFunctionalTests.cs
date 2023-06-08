//using FluentAssertions;
//using IdentityMicroservice.Application.Common.Interfaces;
//using IdentityMicroservice.Application.Features.Auth.Login;
//using IdentityMicroservice.Application.ViewModels.External.Auth;
//using IdentityMicroservice.Domain.Entities;
//using IdentityMicroservice.Infrastructure;
//using IdentityMicroservice.Infrastructure.Persistence.DbContexts.Identity;
//using IdentityMicroservice1;
////using IdentityMicroservice1.Controllers;
//using MediatR;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Diagnostics;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Moq;
//using System.IdentityModel.Tokens.Jwt;

//namespace Identity.Tests.FunctionalTests
//{
//    public class AccountFunctionalTests
//    {
//        private WebApplication _app;
//        private readonly IUserManager _userManager;
//        private readonly Mock<IEmailManager> _emailManager;
//        private readonly ITokenManager _tokenManager;
//      //  public AccountController _accountController;
//        public IdentityDbContext _context;

//        public AccountFunctionalTests()
//        {
//            var builder = WebApplication.CreateBuilder();

//            AddConfiguration(builder);

//            var services = builder.Services;

//            services
//               .AddControllers()
//        //       .AddApplicationPart(typeof(Startup).Assembly)
//               .AddControllersAsServices();

//            //injecting mediatR handlers here so mocking wont be necessary 
//            services.AddMediatR(typeof(LoginCommandHandler).Assembly);

//            _emailManager = new Mock<IEmailManager>();

//            _emailManager.CallBase = true;

//            _emailManager.Setup(x => x.SendEmailConfirmation(It.IsAny<IdentityUser>(), It.IsAny<IdentityUserTokenConfirmation>())).Returns(Task.CompletedTask);
//            _emailManager.Setup(x => x.IsValidEmail(It.IsAny<string>())).Returns(true);

//            services.Remove<DbContextOptions<IdentityDbContext>>();

//            var dbName = $"{Guid.NewGuid()}";

//            services.AddDbContext<IdentityDbContext>(options => options
//             .UseInMemoryDatabase(databaseName: dbName)
//             .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)));

//            services.AddInfrastructure(builder.Configuration);

//            _app = builder.Build();

//            _accountController = _app.Services.GetRequiredService<AccountController>();

//            _context = _app.Services.GetRequiredService<IdentityDbContext>();

//            _userManager = _app.Services.GetRequiredService<IUserManager>();

//            //_emailManager = _app.Services.GetRequiredService<IEmailManager>();

//            _tokenManager = _app.Services.GetRequiredService<ITokenManager>();

//            var httpContext = new DefaultHttpContext
//            {
//                RequestServices = _app.Services
//            };

//            _accountController.ControllerContext.HttpContext = httpContext;

//            AddDataToContext(_context, _app.Services);

//        }

//        //Functional tests (equivalence partitioning + boundary value analysis)
//        // attempt loging in with jwt and calling a protected endpoint -> check if jwt is still in bounderies -> if it fails -> redo login
//        //                                                             -> if  succeeds -> execute endpoint

//        [Fact]
//        public async Task Test1CallProtectedEndpoint()
//        {
//            var jwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6ImZpbGlwZGFtaWFuMjlAZ21haWwuY29tIiwidW5pcXVlX25hbWUiOiJzdHJpbmdzdHJpbmciLCJuYW1laWQiOiIyZDQzMjliMS0yZjIxLTQxNmQtYjhmOS1jMTc1NDE4ZjBmNTYiLCJqdGkiOiI5YzkwOTg1OC1hODc4LTRmZWEtYmRjYS1lMzAyYTE0MGJkNTciLCJOdW1iZXJPZkFsbG93ZWRSZWZyZXNoZXMiOiIzIiwiSW50ZXJ2YWxPZlVzZU9mUmVmcmVzaFRva2VuQWZ0ZXJUb2tlbkhhc0V4cGlyZWQiOiI1Iiwicm9sZSI6IkFkbWluIiwibmJmIjoxNjgxNDkwMDgxLCJleHAiOjE2ODE0OTAxNDEsImlhdCI6MTY4MTQ5MDA4MSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMS8iLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo1MDAxLyJ9._rBLr2TeqIVByKZFpvQp9QgtnnDmbAwRpVdKsJa__Fk";

//            var jwtHandler = new JwtSecurityTokenHandler();
//            var token = jwtHandler.ReadJwtToken(jwtToken);

//            DateTime tokenExpireTime = token.ValidTo;

//            //continue doing the request
//            //else generate another jwt
//            if (tokenExpireTime > DateTime.UtcNow) // this check is also done automatically by the httpcontext 
//            {
//                string profileUrl = "ceva";

//                _accountController.Request.Headers.Authorization = "Bearer " + jwtToken;

//                await _accountController.GetUserProfileInfo(profileUrl);
//            }
//            else
//            {
//                var loginCommand = new LoginCommand
//                {
//                    UniqueIdentifier = "filipdamian29@gmail.com",
//                    Password = "Parola123!"
//                };

//                var response = await _accountController.Login(loginCommand);
//                var jwtResponse = (ObjectResult)response;
//                var jwtObject = (TokenWrapper)jwtResponse.Value;

//                var jwt = jwtHandler.ReadJwtToken(jwtObject.Token);

//                Assert.True(jwt.ValidTo > DateTime.UtcNow);
//            }
//        }
//        #region Configuration
//        private void AddConfiguration(WebApplicationBuilder builder)
//        {
//            builder.Configuration
//                .AddInMemoryCollection(new Dictionary<string, string>
//                {
//                    { "JWTToken:RefreshTokenConfigs:RefreshToken:NumberOfRefreshes", "3"},
//                    { "JWTToken:RefreshTokenConfigs:RefreshToken:TimeLeftUntilRefreshTokenExpiresAfterTokenAlreadyExpired", "5"},
//                    { "JWTToken:RefreshTokenConfigs:RefreshToken:Issuer", "https://localhost:5001/"},
//                    { "JWTToken:RefreshTokenConfigs:RefreshToken:Audience", "https://localhost:5001/"},
//                    { "JWTToken:LoginTokenConfigs:LoginToken:Minutes", "30"},
//                    { "JWTToken:LoginTokenConfigs:LoginToken:Hours", "12"},
//                    { "JWTToken:LoginTokenConfigs:LoginToken:Days", "1"},
//                    { "WebScraperOptions:ChromeDriverOptions:LocalPath", "C:\\Users\\filip\\source\\repos\\IdentityMicroservice1"},
//                    { "LinkedInCredentialsOptions:Username", "filip.constantin@softbinator.com"},
//                    { "LinkedInCredentialsOptions:Password", "Parola123!"},
//                    { "ConnectionStrings:ConnectionStringConfigs:IdentityDatabase:ConnectionString", "Data Source=LAPTOP-4RQNI2BA\\SQLEXPRESS;Initial Catalog=IdentityDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"},
//                    { "ConnectionStrings:ConnectionStringConfigs:IdentityDatabase:TimeoutSeconds", "30"},
//                    { "Hashing:HashingKey", "123456789"},
//                    { "EmailSender:From", "coralisromaniab@gmail.com"},
//                    { "EmailSender:SmtpServer", "smtp.gmail.com"},
//                    { "EmailSender:Port", "465"},
//                    { "EmailSender:Username", "coralisromaniab@gmail.com"},
//                    { "EmailSender:Password", "vmatvwqtgmunxehp"},
//                    { "SigninKey:SecretSignInKeyForJwtToken", "guidlikesecretkey"},
//                });
//        }
//        #endregion

//        #region Populate Context
//        private void AddDataToContext(IdentityDbContext _context, IServiceProvider provider)
//        {
//            _context.IdentityUsers.Add(new()
//            {
//                Id = Guid.NewGuid(),
//                Username = "FilipConstantin",
//                Email = "filipdamian29@gmail.com",
//                EmailConfirmed = true,
//                LockoutEnabled = false,
//                LockoutEnd = null,
//                PhoneNumberCountryPrefix = "+40",
//                PhoneNumber = "0726139370",
//                PhoneNumberConfirmed = true,
//                PasswordHash = "10000.v31H7ESGZw7LhMLSFrupaQ==.+6NJsZciGDX7KB9LFsRDLiInQSP15SrmMKCL4KTN9RM=",
//                TwoFactorEnabled = false,
//                CreatedAt = DateTime.UtcNow,
//                NumberOfFailLoginAttempts = 0,
//                IsActive = false,
//                IsDeleted = false
//            });

//            _context.SaveChanges();
//        }
//        #endregion
//    }
//}

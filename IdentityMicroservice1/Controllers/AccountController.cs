using IdentityMicroservice.Application.Common.Exceptions;
using IdentityMicroservice.Application.Features.Auth.EmailConfirmation;
using IdentityMicroservice.Application.Features.Auth.Login;
using IdentityMicroservice.Application.Features.Auth.RefreshLoginToken;
using IdentityMicroservice.Application.ViewModels.AppInternal;
using IdentityMicroservice.Infrastructure.Persistence.DbContexts.Identity;
using IdentityMicroservice1.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseApplicationController
    {
        private readonly IdentityDbContext _context;

        public AccountController(IdentityDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task <IActionResult> Register([FromBody]RegisterCommand registerCommand)
        {
            try
            {
                var result = await Mediator.Send(registerCommand);
                return Ok(result);
            }
            catch (UserAlreadyRegisteredException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
            
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand loginCommand)
        {
            try
            {
                 var result = await Mediator.Send(loginCommand);
                 return Ok(result);
                //return Ok();
            }
            catch (NotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
            catch (IncorrectPasswordException ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
            catch(ExceededMaximumAmountOfLoginAttemptsException ex)
            {
                Console.WriteLine(ex.Message);
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("email-confirm")]
        
        public async Task<IActionResult> EmailConfirmation([FromBody] EmailConfirmationCommand confirmEmailCommand )
        {
            try
            {
                var result = await Mediator.Send(confirmEmailCommand);
                
                return Ok(result);
            }
            catch
            {
                Console.WriteLine("Failed to Confirm Mail. Try again later!");
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshLoginToken([FromBody] RefreshTokenCommand refreshTokenCommand)
        {
            try
            {
                var result = await Mediator.Send(refreshTokenCommand);
                return Ok(result);
            }
            catch 
            {
                //tratez exceptii pt refreshtoken..
                throw;
            }
        }
    }
}

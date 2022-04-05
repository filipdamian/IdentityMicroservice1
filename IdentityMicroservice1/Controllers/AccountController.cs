using IdentityMicroservice.Application.Common.Exceptions;
using IdentityMicroservice.Application.Features.Auth.EmailConfirmation;
using IdentityMicroservice.Application.Features.Auth.PasswordRecovery;
using IdentityMicroservice.Application.Features.Auth.Login;

using IdentityMicroservice.Application.ViewModels.AppInternal;
using IdentityMicroservice.Infrastructure.Persistence.DbContexts.Identity;
using IdentityMicroservice1.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityMicroservice.Application.Features.Auth.RefreshLoginToken;

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
                return BadRequest(ex.Message);
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
                return BadRequest(ex.Message);
            }
            catch (IncorrectPasswordException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (ExceededMaximumAmountOfLoginAttemptsException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
            catch(AccountStillLockedException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
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
            catch(ResendingEmailConfirmationException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
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
            catch(IntervalOfRefreshTokenExpiredException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
            catch(MaximumRefreshesExceededException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("forgot-password")]
        public async Task<IActionResult> RecoverPassword([FromBody] PasswordRecoveryCommand passwordRecoveryCommand)
        {
            try
            {
                var result = await Mediator.Send(passwordRecoveryCommand);
                return Ok(result);
            }
            catch (UserNotFoundException ex)
            {

                Console.WriteLine(ex.Message);
                return NotFound(ex.Message);
            }
            catch(CouldNotConfirmEmailException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
            catch(EmailConfirmationException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("password-recovery")]
        public async Task<IActionResult> UpdateUserPassword([FromBody] UpdateUserPasswordCommand updateUserPasswordCommand)
        {
            try
            {
                var result = await Mediator.Send(updateUserPasswordCommand);
                return Ok(result);
            }
            catch (UserNotFoundException ex)
            {

                Console.WriteLine(ex.Message);
                return NotFound(ex.Message);
            }
            catch(PasswordRecoveryTokenAlreadyUsedException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (IncorrectPasswordException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}

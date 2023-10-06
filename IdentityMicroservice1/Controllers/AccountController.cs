using IdentityMicroservice.Application.Common.Exceptions;
using IdentityMicroservice.Application.Features.Auth.EmailConfirmation;
using IdentityMicroservice.Application.Features.Auth.PasswordRecovery;
using IdentityMicroservice.Application.Features.Auth.Login;
using IdentityMicroservice.Application.ViewModels.AppInternal;
using IdentityMicroservice.Infrastructure.Persistence.DbContexts.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using IdentityMicroservice.Application.Features.Auth.RefreshLoginToken;
using IdentityMicroservice.Application.Features.LinkedinCrawler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using IdentityMicroservice.Application.Features.ImageLabelPrediction;
using IdentityMicroservice.Application.Features.TrainNewModel;
using IdentityMicroservice.Application.Features.Auth.Tank;
using IdentityMicroservice.Application.Features.Auth.Fish;
using IdentityMicroservice.Application.Features.Auth.Google;
using IdentityMicroservice.Application.Features.Auth.Facebook;

namespace IdentityMicroservice1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseApplicationController
    {
        // Stryker disable all
        public AccountController()
        {
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand registerCommand)
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
        // Stryker restore all
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
            catch (AccountStillLockedException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        // Stryker disable all
        [HttpPost]
        [Route("email-confirm")]

        public async Task<IActionResult> EmailConfirmation([FromBody] EmailConfirmationCommand confirmEmailCommand)
        {
            try
            {
                var result = await Mediator.Send(confirmEmailCommand);

                return Ok(result);
            }
            catch (ResendingEmailConfirmationException ex)
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
            catch (IntervalOfRefreshTokenExpiredException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (MaximumRefreshesExceededException ex)
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
            catch (CouldNotConfirmEmailException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (EmailConfirmationException ex)
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
            catch (PasswordRecoveryTokenAlreadyUsedException ex)
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

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("Linkedin-Crawler")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserProfileInfo([FromQuery] string profileUrl)
        {
            try
            {
                LinkedInUserProfileCommand linkedInUserProfileCommand = new LinkedInUserProfileCommand
                {
                    ProfileUrl = profileUrl
                };
                var result = await Mediator.Send(linkedInUserProfileCommand);
                return Ok(result);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        //[Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [Route("Image-Prediction")]
        public async Task<IActionResult> PredictImageLabel([FromForm] IFormFile File)
        {
            try
            {
                ImageLabelPredictionCommand imglabelprecit = new ImageLabelPredictionCommand
                {
                    File = File
                };
                var result = await Mediator.Send(imglabelprecit);
                return Ok(result);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        //[Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [Route("Train-New-Model")]
        public async Task<IActionResult> TrainNewModel()
        {
            try
            {
                TrainNewModelCommand trainNewModel = new();
                var result = await Mediator.Send(trainNewModel);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPost]
        [Route("create-edit-tank")]
        public async Task<IActionResult> CreateTank([FromBody] AddOrUpdateTankCommand addOrUpdateTankCommand)
        {
            try
            {
                addOrUpdateTankCommand.Model.UserId = new Guid(HttpContext.Items["UserId"] as string);
                var result = await Mediator.Send(addOrUpdateTankCommand);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPost]                                  /// get fishes and tanks endpoints
        [Route("add-or-remove-fish")]
        public async Task<IActionResult> AddFishToTank([FromBody] AddOrRemoveFishCommand addOrRemoveFishCommand)
        {
            try
            {
                addOrRemoveFishCommand.UserId = new Guid(HttpContext.Items["UserId"] as string);
                var result = await Mediator.Send(addOrRemoveFishCommand);
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpDelete("delete-tank")]
        public async Task<IActionResult> DeleteTank([FromHeader] int TankId)
        {
            try
            {
                var deleteTankCommand = new RemoveTankCommand
                {
                    TankId = TankId,
                    UserId = new Guid(HttpContext.Items["UserId"] as string)
                };

                var result = await Mediator.Send(deleteTankCommand);
                return Ok(result);

            }
            catch (Exception)
            {

                throw;
            }
        }
        [Authorize]
        [HttpGet("one-or-all-tanks")]
        public async Task<IActionResult> GetTanks([FromQuery] int? TankId = null)
        {
            try
            {
                var getTanksCommand = new GetTanksCommand
                {
                    TankId = TankId,
                    UserId = new Guid(HttpContext.Items["UserId"] as string)
                };

                var result = await Mediator.Send(getTanksCommand);
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("google-api-login")]
        public async Task<IActionResult> GoogleLogin([FromQuery] string accessToken)
        {
            var googleLoginCommand = new GoogleLoginApiCommand
            {
                AccessToken = accessToken
            };

            var res = await Mediator.Send(googleLoginCommand);

            return Ok(res);
        }

        [HttpGet("facebook-api-login")]
        public async Task<IActionResult> FacebookLogin([FromQuery] string accessToken)
        {
            var facebookLoginCommand = new FacebookLoginApiCommand
            {
                AccessToken = accessToken
            };

            var res = await Mediator.Send(facebookLoginCommand);

            return Ok(res);
        }
        [HttpGet("list-of-fish-in-db")]
        public async Task<IActionResult> AllFish()
        {
            var allFishCommand = new AllFishCommand();
            allFishCommand.UserId = new Guid(HttpContext.Items["UserId"] as string);
            var res = await Mediator.Send(allFishCommand);
            return Ok(res);
        }
    }
}

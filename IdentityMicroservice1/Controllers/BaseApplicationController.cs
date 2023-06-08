using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityMicroservice1.Controllers
{
    // Stryker disable all
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApplicationController : ControllerBase
    {
        private IMediator _mediator; 
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
    }
}

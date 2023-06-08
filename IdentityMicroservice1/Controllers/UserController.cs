using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice1.Controllers
{
    // Stryker disable all
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        /*
        private readonly IRepositoryWrapper _repository;
        public UserController(IRepositoryWrapper repository) { _repository = repository; }
        [HttpGet("allusers")]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _repository.User.GetAllUsers();
            return Ok(new { users });
        }
        */

        [HttpGet("")]
        [Authorize()]
        public IActionResult FakeGet()
        {
            return Ok(true);
        }

    }
}

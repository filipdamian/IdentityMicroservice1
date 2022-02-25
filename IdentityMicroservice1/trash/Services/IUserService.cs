using IdentityMicroservice.Application.ViewModels.AppInternal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice1.Services
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(RegisterCommand dto);
        Task<string> LoginUserAsync(LoginUserDTO dto);
    }
}

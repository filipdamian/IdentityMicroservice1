using IdentityMicroservice.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMicroservice.Application.Common.Interfaces
{
    public interface IEmailManager
    {
        Task SendEmailConfirmation(IdentityUser user,IdentityUserTokenConfirmation token);
        Task<bool> IsEmailConfirmed(string userIntroducedToken, IdentityUserTokenConfirmation TokenFromDb,IdentityUser user);
    }
}

using IdentityMicroservice.Application.Features.PasswordHashing;
using IdentityMicroservice.Application.ViewModels.AppInternal;
using IdentityMicroservice.Domain.Constants;
using IdentityMicroservice.Domain.Entities;
using IdentityMicroservice.Infrastructure.Persistence.DbContexts.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMicroservice1.Services
{
    public class UserService 
    {
        private readonly HashAlgo _hashAlgo;
        private readonly IdentityDbContext _context;
        public UserService(HashAlgo hashAlgo,IdentityDbContext context) { _hashAlgo = hashAlgo; _context = context; }
       /* public async Task<bool> RegisterUserAsync(RegisterUserDTO dto)
        {
            var registerUser = new IdentityUser();
            registerUser.Email = dto.Email;
            registerUser.Username = dto.FirstName + dto.LastName;//mapez username
            string result =  _hashAlgo.CalculateHashValueWithInput(dto.Password);  //await?
            registerUser.PasswordHash = result;
            if(result!=null)
            {
                _context.Set<IdentityUser>().Add(registerUser);
                await _context.SaveChangesAsync();

                var id= await _context.IdentityUsers.Where(u => u.Email.Equals(dto.Email)).Select(u => u.Id).SingleOrDefaultAsync();
                _context.Set<IdentityRole>().Add(new IdentityRole(UserRoleType.Admin,id));


                return true;
            }
            return false;

        }*/
      
        
        
        
        
      /*  public async Task<string> LoginUserAsync(LoginUserDTO dto)
        {
            IdentityUser user = await _context.IdentityUsers.Where(u => u.Email.Equals(dto.Email)).SingleOrDefaultAsync();
            if(user!=null)
            {
                bool userSuccessfullyLogged = _hashAlgo.CalculateReversedHashValue(dto.Password);
                if(userSuccessfullyLogged == true)
                {
                    user = await _context.IdentityUsers.Include(u => u.IdentityUserRoles).ThenInclude(ur => ur.IdentityRole).Where(u => u.Id.Equals(user.Id)).SingleOrDefaultAsync();
                    List<string> roles = user.IdentityUserRoles.Select(ur => ur.IdentityRole.Name).ToList();
                    var newJti = Guid.NewGuid().ToString();
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("guidlikesecretkey"));

                    var token = GenerateJwtToken(signinKey, user, roles, tokenHandler, newJti);

                    //sessiontoken.create()..
                    //savechangesasync

                    return tokenHandler.WriteToken(token);

                }

            }
            return "";
            
        }
        */
        private SecurityToken GenerateJwtToken(SymmetricSecurityKey signinKey, IdentityUser user, List<string> roles, JwtSecurityTokenHandler tokenHandler, string newJti)
        {
            throw new NotImplementedException();
        }
    }
}

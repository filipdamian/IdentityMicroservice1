using System;
using System.Collections.Generic;

#nullable disable

namespace IdentityMicroservice.Domain.Entities
{
    public partial class IdentityRole
    {
        public IdentityRole()
        {
            IdentityRoleClaims = new HashSet<IdentityRoleClaim>();
            IdentityUserRoles = new List<IdentityUserIdentityRole>();
            
        }
        //public IdentityRole(string name,Guid identityuserid)
        public IdentityRole(string name,Guid id)
        {
            this.Id = id;
            this.Name = name;
            // FK_IdentityRole_IdentityUser_IdentityUserId
            //this.IdentityUserRoles.Add(new IdentityUserIdentityRole(identityuserid, this.Id));
            
      
        }
        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<IdentityRoleClaim> IdentityRoleClaims { get; set; }
        public List<IdentityUserIdentityRole> IdentityUserRoles { get; set; }
    }
}

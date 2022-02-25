using System;

#nullable disable

namespace IdentityMicroservice.Domain.Entities
{
    public partial class IdentityUserClaim
    {
        public int Id { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public Guid UserId { get; set; }

        public virtual IdentityUser User { get; set; }
    }
}

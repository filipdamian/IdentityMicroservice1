using System;
using System.Collections.Generic;

#nullable disable

namespace IdentityMicroservice.Domain.Entities
{
    public partial class IdentityUserExternalLogin
    {
        public Guid Id { get; set; }
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string ProviderDisplayName { get; set; }
        public Guid UserId { get; set; }

        public virtual IdentityUser User { get; set; }
    }
}

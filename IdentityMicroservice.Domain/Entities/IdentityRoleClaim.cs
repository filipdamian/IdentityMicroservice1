﻿using System;

#nullable disable

namespace IdentityMicroservice.Domain.Entities
{
    public partial class IdentityRoleClaim
    {
        public int Id { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public Guid RoleId { get; set; }

        public virtual IdentityRole Role { get; set; }
    }
}

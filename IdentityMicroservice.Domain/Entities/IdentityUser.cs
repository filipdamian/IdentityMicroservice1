﻿using System;
using System.Collections.Generic;

#nullable disable

namespace IdentityMicroservice.Domain.Entities
{
    public partial class IdentityUser
    {
        public IdentityUser()
        {
            IdentityUserClaims = new HashSet<IdentityUserClaim>();
            IdentityUserExternalLogins = new HashSet<IdentityUserExternalLogin>();
            IdentityRoles = new HashSet<IdentityRole>();
            IdentityUserTokenConfirmations = new HashSet<IdentityUserTokenConfirmation>();
            IdentityUserTokens = new HashSet<IdentityUserToken>();
        }

        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public string PhoneNumberCountryPrefix { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; } 
        public string PasswordHash { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime CreatedAt { get; set; }
        public int NumberOfFailLoginAttempts { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<IdentityUserClaim> IdentityUserClaims { get; set; }
        public virtual ICollection<IdentityUserExternalLogin> IdentityUserExternalLogins { get; set; }
        public virtual ICollection<IdentityRole> IdentityRoles { get; set; }
        public virtual ICollection<IdentityUserTokenConfirmation> IdentityUserTokenConfirmations { get; set; }
        public virtual ICollection<IdentityUserToken> IdentityUserTokens { get; set; }
        public virtual ICollection<IdentityUserIdentityRole> IdentityUserRoles { get; set; }
        //public ICollection<WOType> WOTypes { get; set; } //modif
    }
}

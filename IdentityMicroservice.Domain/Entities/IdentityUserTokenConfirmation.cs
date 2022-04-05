using IdentityMicroservice.Domain.Enums;
using System;

namespace IdentityMicroservice.Domain.Entities
{
    public partial class IdentityUserTokenConfirmation
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public ConfirmationTokenType ConfirmationTypeId { get; set; }
        public string ConfirmationToken { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }

        

        public virtual IdentityUser User { get; set; }
    }
}

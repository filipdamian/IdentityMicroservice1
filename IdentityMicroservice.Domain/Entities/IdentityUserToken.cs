using System;


namespace IdentityMicroservice.Domain.Entities
{
    public partial class IdentityUserToken
    {
        public IdentityUserToken() { }
        public IdentityUserToken(Guid userid,string tokenvalue,string refreshtokenvalue,DateTime creationdate,DateTime expirationdate)
        {
            this.Id = Guid.NewGuid();
            this.UserId = userid;
            this.TokenValue = tokenvalue;
            this.RefreshTokenValue = refreshtokenvalue;
            this.CreationDate = creationdate;
            this.ExpirationDate = expirationdate;
        }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string TokenValue { get; set; }
        public string RefreshTokenValue { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsTokenRevoked { get; set; }
        public virtual IdentityUser User { get; set; }
    }
}

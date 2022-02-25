using IdentityMicroservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace IdentityMicroservice.Infrastructure.Persistence.DbContexts.Identity
{
    public class IdentityDbContext:DbContext
    {
       
        public IdentityDbContext(DbContextOptions options) : base(options) {}

        public DbSet<IdentityUser> IdentityUsers { get; set; }
        public DbSet<IdentityRole> IdentityRoles { get; set; }
        public DbSet<IdentityRoleClaim> IdentityRoleClaims { get; set; }
        public DbSet<IdentityUserClaim> IdentityUserClaims { get; set; }
        public DbSet<IdentityUserIdentityRole> IdentityUserIdentityRoles { get; set; }
        public DbSet<IdentityUserToken> IdentityUserTokens { get; set; }
        public DbSet<IdentityUserTokenConfirmation> IdentityUserTokenConfirmations { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}

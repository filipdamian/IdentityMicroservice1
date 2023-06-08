using System.Collections.Generic;

namespace IdentityMicroservice.Domain.Entities
{
    public class FishTank
    {
        public int Id { get; set; }
        public decimal Lenght { get; set; }
        public decimal Height { get; set; }
        public decimal Width { get; set; }
        public decimal Volume { get; set; }

        public virtual IdentityUser User { get; set; }
        public virtual ICollection<FishSpecs> FishSpecs { get; set; }
        public virtual ICollection<PetFish> PetFish { get; set; }
    }
}

using System.Collections.Generic;

namespace IdentityMicroservice.Domain.Entities
{
    public class PetFish
    {
        public int Id { get; set; }
        public string FishName { get; set; }
        public virtual ICollection<FishTank> FishTanks {get;set;}
    }
}

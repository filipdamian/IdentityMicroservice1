using IdentityMicroservice.Domain.Enums;
using System.Collections.Generic;

namespace IdentityMicroservice.Domain.Entities
{
    public class FishSpecs
    {
        public int Id { get; set; }
        public string Species { get; set; }
        public WaterType WaterType { get; set; }
        public Diet Diet { get; set; }
        public decimal WaterAcidity { get; set; }
        public decimal WaterTemperature { get; set; }
        public decimal FishSize { get; set; }

        public virtual ICollection<FishTank> FishTanks { get; set; }
    }
}

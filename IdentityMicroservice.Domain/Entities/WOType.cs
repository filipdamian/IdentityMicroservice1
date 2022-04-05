using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMicroservice.Domain.Entities
{
    public partial class WOType
    {
        public Guid Id { get; set; }
        public Guid ChangedByUserId { get; set; }
        public DateTime ChangeDate { get; set; }
        public string WOTypeCode { get; set; }
        public string WOTypeName { get; set; }
        public decimal DurationMinutes { get; set; }

        public ICollection<WOTypeScenario> WOTypeScenarios { get; set; }
        //public IdentityUser IdentityUser { get; set; }

    }
}

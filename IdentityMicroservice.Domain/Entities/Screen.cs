using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMicroservice.Domain.Entities
{
    public partial class Screen
    {
        public Guid Id { get; set; }
        public Guid ChangedByUserId { get; set; }
        public DateTime ChangeDate { get; set; }
        public string ScreenCode { get; set; }
        public string ScreenName { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Description { get; set; }
        public WOTypeScenarioStep WOTypeScenarioStep { get; set; }

        //user?
    }
}

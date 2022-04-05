using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMicroservice.Domain.Entities
{
    public partial class WOTypeScenario
    {
        public Guid Id { get; set; }
        public Guid WOTypeId { get; set; }
        public string ScenarioCode { get; set; }
        public string ScenarioName { get; set; }
        public int  State { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public WOType WOType { get; set; }
        public ICollection<WOTypeScenarioStep> WOTypeScenarioSteps { get; set; }

    }
}

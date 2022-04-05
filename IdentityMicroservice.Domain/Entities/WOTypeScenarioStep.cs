using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMicroservice.Domain.Entities
{
    public partial class WOTypeScenarioStep
    {
        public Guid Id { get; set; }
        public int StepNumber { get; set; }
        public string StepName { get; set; }
        public string ScreenCode { get; set; }
        public Guid WOScenarioId { get; set; }
        public Guid ScreenId { get; set; }
        public WOTypeScenario WOTypeScenario { get; set; }
        public Screen Screen { get; set; }

    }
}

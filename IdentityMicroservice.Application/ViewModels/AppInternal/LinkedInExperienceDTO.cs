using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMicroservice.Application.ViewModels.AppInternal
{
    public class LinkedInExperienceDTO
    {
        public string Employer { get; set; }
        public string Role { get; set; }
        public string Interval { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
    }
}
